(ns tool.stock.drawer
  (:require [clojure.spec.alpha :as s])
  (:require [tool.stock.tool :as stl]
            [tool.stock.spec]))

(s/def ::type #{:grid :line :kline :clock})
(s/def ::line (s/coll-of number? :kine seqable?))
(s/def ::color string?)
(s/def ::hideY boolean?)
(s/def ::centerY number?)
(s/def ::style #{:dot})
(s/def ::drawer (s/keys :req-un [::type]
                        :opt-un [::line ::tool.stock.spec/kline ::color ::style ::hideY ::centerY]))
(s/def ::drawers (s/coll-of ::drawer :kind seqable?))
(s/def ::draw-data (s/keys :opt-un [::drawers]))

(defmulti max-v (fn [{t :type}] t))
(defmulti min-v (fn [{t :type}] t))
(defmulti length (fn [{t :type}] t))
(defmulti draw-it (fn [{t :type} base ctx] t))

(defn graphic-base [w h drawers]
  (s/assert pos-int? w)
  (s/assert pos-int? h)
  (s/assert ::drawers drawers)
  (s/assert
   (s/tuple pos-int? pos-int? number? number? number? number? fn?)
   (let [max-v (apply max (map max-v drawers))
         min-v (apply min (map min-v drawers))
         len-v (apply max (map length drawers))
         offset-v (- max-v min-v)
         offset-x (/ w (inc len-v))
         pos-y (fn [v]
                 (* h (- 1 (/ (- v min-v) offset-v))))]
     [w h max-v min-v offset-v offset-x pos-y])))

(defn draw [draw-data w h ctx]
  (s/assert ::draw-data draw-data)
  (s/assert pos-int? w)
  (s/assert pos-int? h)
  (when-let [drawers (:drawers draw-data)]
    (let [base (graphic-base w h drawers)]
      (aset ctx "fillStyle" "black")
      (.fillRect ctx 0 0 w h)
      (doseq [drawer drawers]
        (.save ctx)
        (draw-it drawer base ctx)
        (.restore ctx)))))

(defmethod max-v :default [] 0)
(defmethod min-v :default [] 0)
(defmethod length :default [] 0)
(defmethod draw-it :default [])

(defmethod max-v :grid [{line :line kline :kline}]
  (s/assert (s/nilable ::line) line)
  (s/assert ::tool.stock.spec/kline kline)
  (apply max (or line (stl/high kline))))

(defmethod min-v :grid [{line :line kline :kline}]
  (s/assert (s/nilable ::line) line)
  (s/assert ::tool.stock.spec/kline kline)
  (apply min (or line (stl/low kline))))

(defmethod length :grid [{line :line kline :kline}]
  (s/assert (s/nilable ::line) line)
  (s/assert ::tool.stock.spec/kline kline)
  (count (or line kline)))

(defmethod draw-it :grid [{line :line kline :kline color :color hideY :hideY centerY :centerY} base ctx]
  (let [[w h max-v min-v offset-v offset-x pos-y] base
        cnt 10
        cntx 5
        offset (-> (- max-v min-v) (/ cnt))]
    (aset ctx "strokeStyle" color)
    (aset ctx "fillStyle" color)
    (aset ctx "lineWidth" 1)
    (.beginPath ctx)

    (when-not hideY
      (doseq [i (range cnt)]
        (let [v (- (or centerY min-v) (* i offset))]
          (.fillText ctx (str (.toFixed v 2)) (* w (/ 1 3)) (pos-y v))
          (.fillText ctx (str (.toFixed v 2)) (* w (/ 2 3)) (pos-y v))
          (.moveTo ctx 0 (pos-y v))
          (.lineTo ctx w (pos-y v)))
        (let [v (+ (or centerY min-v) (* i offset))]
          (.fillText ctx (str (.toFixed v 2)) (* w (/ 1 3)) (pos-y v))
          (.fillText ctx (str (.toFixed v 2)) (* w (/ 2 3)) (pos-y v))
          (.moveTo ctx 0 (pos-y v))
          (.lineTo ctx w (pos-y v)))))

    (when kline
      (doseq [i (range (count kline))]
        (when (zero? (mod i cntx))
          (let [posx (+ (/ offset-x 2) (* i offset-x))]
            (.moveTo ctx posx 0)
            (.lineTo ctx posx h)))))

    (when line
      (doseq [i (range (count line))]
        (when (zero? (mod i cntx))
          (let [posx (+ (/ offset-x 2) (* i offset-x))]
            (.moveTo ctx posx 0)
            (.lineTo ctx posx h)))))

    (.stroke ctx)))


(defmethod max-v :line [{line :line}]
  (s/assert ::line line)
  (apply max line))
(defmethod min-v :line [{line :line}]
  (s/assert ::line line)
  (apply min line))
(defmethod length :line [{line :line}]
  (s/assert ::line line)
  (count line))
(defmethod draw-it :line [{line :line color :color offset :offset style :style} base ctx]
  (let [[w h max-v min-v offset-v offset-x pos-y] base
        offset (or offset 0)]
    (aset ctx "strokeStyle" color)
    (aset ctx "lineWidth" 1)
    (condp = style
      :dot
      (do
        (aset ctx "fillStyle" color)
        (doseq [[idx value] (map vector
                                 (map inc (range (count line)))
                                 line)]
          (.beginPath ctx)
          (.arc ctx (* (+ idx offset) offset-x) (pos-y value) (/ offset-x 4) 0 6.28 false)
          (.closePath ctx)
          (.fill ctx)))

      (do
        (.beginPath ctx)
        (doseq [[idx prev curr] (map (fn [& args] args)
                                     (map inc (range (count line)))
                                     line
                                     (rest line))]
          (.moveTo ctx (* (+ idx offset) offset-x) (pos-y prev))
          (.lineTo ctx (* (inc (+ idx offset)) offset-x) (pos-y curr)))
        (.stroke ctx)))))

; k line
(defmethod max-v :kline [{kline :kline}]
  (s/assert ::tool.stock.spec/kline kline)
  (apply max (stl/high kline)))

(defmethod min-v :kline [{kline :kline}]
  (s/assert ::tool.stock.spec/kline kline)
  (apply min (stl/low kline)))

(defmethod length :kline [{kline :kline}]
  (s/assert ::tool.stock.spec/kline kline)
  (count kline))

(defmethod draw-it :kline [{kline :kline info :info} base ctx]
  (let [[w h max-v min-v offset-v offset-x pos-y] base]
    (aset ctx "fillStyle" "black")
    (doseq [[idx [date open high low close volume] info] (map (fn [& args] args)
                                                              (map inc (range (count kline)))
                                                              kline
                                                              (or info kline))]
      (aset ctx "strokeStyle" (if (> close open) "red" "green"))
      (aset ctx "lineWidth" 2)
      (.beginPath ctx)
      (.moveTo ctx (* idx offset-x) (pos-y low))
      (.lineTo ctx (* idx offset-x) (pos-y high))
      (.stroke ctx)

      (aset ctx "strokeStyle" (if (> close open) "red" "green"))
      (aset ctx "lineWidth" offset-x)
      (.beginPath ctx)
      (.moveTo ctx (* idx offset-x) (pos-y open))
      (.lineTo ctx (* idx offset-x) (inc (pos-y open)))
      (.lineTo ctx (* idx offset-x) (pos-y close))
      (.stroke ctx))))


(defmethod max-v :clock [{cz :cz}] (apply max cz))
(defmethod min-v :clock [{cz :cz}] (apply min cz))
(defmethod draw-it :clock [{cz :cz vz :vz color :color} base ctx]
  (let [[w h max-v min-v offset-v offset-x pos-y] base
        proj-x (fn [v]
                 (-> v
                     (* (/ w 8))
                     (+ (/ w 2))))
        proj-y (fn [v]
                 (-> v
                     (* (/ h 8) -1)
                     (+ (/ h 2))))]
    (aset ctx "fillStyle" color)
    (aset ctx "strokeStyle" color)
    (doseq
     [[idx pc cc pv cv] (map (fn [& args] args)
                             (range (count cz))
                             cz
                             (rest cz)
                             vz
                             (rest vz))]
      (.fillText ctx (str idx) (proj-x pv) (proj-y pc))
      (.beginPath ctx)
      (.moveTo ctx (proj-x pv) (proj-y pc))
      (.lineTo ctx (proj-x cv) (proj-y cc))
      (.stroke ctx))))
