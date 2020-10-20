(ns tool.stock.formula-drawer
  (:require [clojure.spec.alpha :as s])
  (:require [tool.stock.spec]
            [tool.stock.drawer]
            [tool.stock.tool :as stl]
            [tool.stock.formula :as stf]))

(s/def ::yu-car (s/tuple #{:yu-car} pos-int? number? number?))
(s/def ::yu-macd (s/tuple #{:yu-macd} pos-int? pos-int? pos-int?))
(s/def ::yu-clock (s/tuple #{:yu-clock} pos-int? pos-int?))
(s/def ::yu-sd (s/tuple #{:yu-sd} int? pos-int?))
(s/def ::yu-kd (s/tuple #{:yu-kd} int? pos-int?))
(s/def ::yu-money (s/tuple #{:yu-money} pos-int? pos-int?))
(s/def ::ma (s/cat :type #{:ma} :args (s/* pos-int?)))
(s/def ::ema (s/cat :type #{:ema} :args (s/* pos-int?)))
(s/def ::bbi (s/tuple #{:bbi} pos-int? pos-int? pos-int? pos-int?))
(s/def ::ebbi (s/tuple #{:ebbi} pos-int? pos-int? pos-int? pos-int?))
(s/def ::macd (s/tuple #{:macd} pos-int? pos-int? pos-int?))
(s/def ::kd (s/tuple #{:kd} pos-int? pos-int? pos-int?))
(s/def ::chaikin (s/tuple #{:chaikin} pos-int? pos-int? pos-int?))
(s/def ::cv (s/tuple #{:cv} pos-int? pos-int?))
(s/def ::eom (s/tuple #{:eom} pos-int? pos-int?))
(s/def ::sar (s/tuple #{:sar} pos-int?))
(s/def ::osc (s/tuple #{:osc} pos-int? pos-int?))
(s/def ::rsi (s/tuple #{:rsi} pos-int? pos-int?))
(s/def ::atr (s/tuple #{:atr} pos-int? pos-int?))
(s/def ::dmi (s/tuple #{:dmi} pos-int? pos-int?))
(s/def ::acc-dist (s/tuple #{:acc-dist} pos-int?))
(s/def ::cci (s/tuple #{:cci} pos-int?))
(s/def ::dpo (s/tuple #{:dpo} pos-int?))
(s/def ::trix (s/tuple #{:trix} pos-int? pos-int?))
(s/def ::uos (s/tuple #{:uos} pos-int? pos-int? pos-int? pos-int?))
(s/def ::nkline (s/tuple #{:nkline} pos-int?))
(s/def ::volume #{:volume})
(s/def ::clock #{:clock})
(s/def ::kline #{:kline})
(s/def ::formula-drawer-data (s/or :yu-car ::yu-car
                                   :yu-macd ::yu-macd
                                   :yu-clock ::yu-clock
                                   :yu-sd ::yu-sd
                                   :yu-kd ::yu-kd
                                   :yu-money ::yu-money
                                   :ma ::ma
                                   :ema ::ema
                                   :bbi ::bbi
                                   :ebbi ::ebbi
                                   :macd ::macd
                                   :kd ::kd
                                   :chaikin ::chaikin
                                   :cv ::cv
                                   :eom ::eom
                                   :sar ::sar
                                   :osc ::osc
                                   :rsi ::rsi
                                   :atr ::atr
                                   :dmi ::dmi
                                   :acc-dist ::acc-dist
                                   :cci ::cci
                                   :dpo ::dpo
                                   :trix ::trix
                                   :uos ::uos
                                   :nkline ::nkline
                                   :kline ::kline
                                   :clock ::clock
                                   :volume ::volume))

(defn data->drawer [kline data]
  (s/assert ::tool.stock.spec/multi-kline kline)
  (s/assert ::formula-drawer-data data)
  (s/assert
   ::tool.stock.drawer/drawers
   (let [[t data] (s/conform ::formula-drawer-data data)
         gridColor "#555"
         c4 "#FF00FF"
         c3 "#0000FF"
         c2 "#00FFFF"
         c1 "#FFFF00"
         colors (->> (range)
                     (mapcat (constantly [c1 c2 c3 c4])))]
     (condp = t
       :yu-car
       (let [[_ n m o] data
             [_ ranges] (reverse (stf/yu-car n m o (reverse kline)))
             _ (stf/average (stl/mid kline))]
         [{:type :line :line (map + (stl/mid kline) (reverse ranges)) :color c1 :offset -1}
          {:type :line :line (map - (stl/mid kline) (reverse ranges)) :color c1 :offset -1}])

       :yu-macd
       (let [[_ n m o] data
             vs (stl/close kline)
             ema (reverse (stf/ema-seq n (reverse vs)))
             ebbi (stf/EBBI m (* m 2) (* m 4) (* m 8) vs)
             dif (map - ema ebbi)]
         [{:type :line :line dif :color c1}
          {:type :line :line (reverse (stf/sma-seq o (reverse dif))) :color c2}
          {:type :grid :line dif :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :yu-clock
       (let [[_ n m] data
             vs (stf/sma-seq m (stf/yu-clock n (reverse kline)))]
         [{:type :line :line (reverse vs) :color c1}
          {:type :grid :line (reverse vs) :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :yu-sd
       (let [[_ n m] data
             group (take m (drop n kline))
             vs (stl/open group)
             ; 每天和隔天的價差
             offsets (stf/offset-seq (reverse vs))
             ; 價差的平均數
             offsets-avg (stf/average offsets)
             ; 價差的標準差
             sd (stf/StandardDeviation offsets-avg offsets)
             sd2 (* sd 2)
             predict (-> (take n kline)
                         (stl/open))]
         [{:type :line :line (cons 0 (reverse predict)) :color "white" :offset -1}
          {:type :line :line (concat (repeat (inc n) 0) (reverse offsets)) :color "white" :offset -1}
          {:type :line :line (repeat (count kline) 0) :color "white"}
          {:type :line :line (repeat (count kline) offsets-avg) :color "white"}
          {:type :line :line (repeat (count kline) (+ (+ sd) offsets-avg)) :color c1}
          {:type :line :line (repeat (count kline) (+ (- sd) offsets-avg)) :color c1}
          {:type :line :line (repeat (count kline) (+ (+ sd2) offsets-avg)) :color c2}
          {:type :line :line (repeat (count kline) (+ (- sd2) offsets-avg)) :color c2}])

       :yu-kd
       (let [[_ n] data
             h9 (stf/maxN-seq n #(apply max %) (stl/high kline))
             l9 (stf/maxN-seq n #(apply min %) (stl/low kline))
             c (stl/close kline)]
         [{:type :line :line h9 :color c1}
          {:type :line :line l9 :color c1}
          {:type :line :line c :color c2}])

       :yu-money
       (let [[_ n m] (:args data)
             line (stf/money-line n (stl/close kline))]
         [{:type :line :line line :color c1}
          {:type :line :line (reverse (stf/sma-seq m (reverse line))) :color c2}
          {:type :grid :line line :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :ma
       (let [args (:args data)
             vs (stl/close kline)]
         (map (fn [n color]
                {:type :line :line (reverse (stf/sma-seq n (reverse vs))) :color color})
              args
              colors))

       :ema
       (let [args (:args data)
             vs (stl/close kline)]
         (map (fn [n color]
                {:type :line :line (reverse (stf/ema-seq n (reverse vs))) :color color})
              args
              colors))

       :bbi
       (let [[_ n m o p] data
             vs (stl/close kline)]
         [{:type :line :line (stf/BBI n m o p vs) :color c2}])

       :ebbi
       (let [[_ n m o p] data
             vs (stl/close kline)]
         [{:type :line :line (stf/EBBI n m o p vs) :color c2}])

       :macd
       (let [[_ n m o] data
             dif (stf/macd-dif n m kline)]
         [{:type :line :line dif :color c1}
          {:type :line :line (reverse (stf/sma-seq o (reverse dif))) :color c2}
          {:type :line :line (repeat (count kline) 0) :color "white"}
          {:type :grid :line dif :centerY 0 :color gridColor}])

       :kd
       (let [[_ n m o] data
             rsv (stf/rsv-seq n kline)
             k (reverse (stf/sma-seq m (reverse rsv)))
             d (reverse (stf/sma-seq o (reverse k)))]
         [{:type :line :line k :color c1}
          {:type :line :line d :color c2}
          {:type :grid :line rsv :center 0.5 :color gridColor}
          {:type :line :line (repeat (count kline) 0.5) :color "white"}])

       :chaikin
       (let [[_ n m o] data
             vs (stf/Chaikin n m kline)]
         [{:type :line :line vs :color c1}
          {:type :line :line (reverse (stf/sma-seq o (reverse vs))) :color c2}
          {:type :grid :line vs :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :cv
       (let [[_ n m] data
             rema (->> (map -
                            (stf/maxN-seq n #(apply max %) (stl/high kline))
                            (stf/maxN-seq n #(apply min %) (stl/low kline)))
                       reverse
                       (stf/ema-seq 1)
                       reverse)
             vs (stf/volatility-seq m rema)]
         [{:type :line :line vs :color c1}
          {:type :grid :line vs :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :eom
       (let [[_ n m] data
             vs (stf/EOM n kline)]
         [{:type :line :line vs :color c1}
          {:type :line :line (reverse (stf/sma-seq m (reverse vs))) :color c2}
          {:type :grid :line vs :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :sar
       (let [[_ n] data
             sar (reverse (stf/sar-seq n (reverse kline)))]
         [{:type :line :line sar :color c1 :style :dot}])

       :osc
       (let [[_ n m] data
             line (stf/osc-seq n (stl/close kline))]
         [{:type :line :line line :color c1}
          {:type :line :line (reverse (stf/sma-seq m (reverse line))) :color c2}
          {:type :grid :line line :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :rsi
       (let [[_ n m] data
             line (stf/rsi-seq n (stl/close (reverse kline)))]
         [{:type :line :line (reverse line) :color c1}
          {:type :line :line (reverse (stf/sma-seq m line)) :color c2}
          {:type :grid :line (reverse line) :centerY 0.5 :color gridColor}
          {:type :line :line (repeat (count kline) 0.5) :color "white"}])

       :atr
       (let [[_ n m] data
             line (stf/atr-seq n (reverse kline))
             line2 (stf/sma-seq m line)
             avg (stf/average line)]
         [{:type :line :line (reverse line) :color c1}
          {:type :line :line (reverse line2) :color c2}
          {:type :grid :line (reverse line) :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :dmi
       (let [[_ n m] data
             atr (stf/tr-seq (reverse kline))
             dm (stf/dm-seq (reverse kline))
             dip (map (fn [v v2]
                        (if (pos? v)
                          (/ v v2)
                          0))
                      dm
                      atr)
             did (map (fn [v v2]
                        (if (neg? v)
                          (/ (.abs js/Math v) v2)
                          0))
                      dm
                      atr)
             adip (stf/sma-seq n dip)
             adid (stf/sma-seq n did)
             dx (map (fn [v1 v2]
                       (if (zero? (+ v1 v2))
                         0
                         (/ (.abs js/Math (- v1 v2)) (+ v1 v2))))
                     adip
                     adid)]
         [{:type :line :line (reverse adip) :color "red"}
          {:type :line :line (reverse adid) :color "green"}
          {:type :line :line (reverse (stf/sma-seq m dx)) :color c1}
          {:type :grid :line (reverse (stf/sma-seq m dx)) :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :acc-dist
       (let [[_ n] data
             line (stf/AccDist (reverse kline))]
         [{:type :line :line (reverse line) :color c1}
          {:type :line :line (reverse (stf/sma-seq n line)) :color c2}
          {:type :grid :line line :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :cci
       (let [[_ n] data
             line (stf/cci-seq n (reverse kline))]
         [{:type :line :line (reverse line) :color c1}
          {:type :grid :line line :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :dpo
       (let [[_ n] data
             line (stf/dpo-seq n kline)]
         [{:type :line :line line :color c1}
          {:type :grid :line line :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :trix
       (let [[_ n m] data
             line (stf/trix-seq n (stl/close kline))]
         [{:type :line :line line :color c1}
          {:type :line :line (reverse (stf/sma-seq m (reverse line))) :color c2}
          {:type :grid :line line :centerY 0 :color gridColor}
          {:type :line :line (repeat (count kline) 0) :color "white"}])

       :uos
       (let [[_ n m o p] data
             line (stf/uos-seq n m o (reverse kline))
             ma (reverse (stf/sma-seq p line))
             ma2 (reverse (stf/sma-seq p (reverse ma)))]
         [{:type :line :line (reverse line) :color c1}
          {:type :line :line ma :color c2}
          {:type :line :line ma2 :color c3}
          {:type :grid :line line :centerY 50 :color gridColor}
          {:type :line :line (repeat (count kline) 50) :color "white"}])

       :nkline
       (let [[_ n] data
             kline (->> (stf/nkline n kline)
                        (take (int (/ (count kline) n))))]
         [{:type :grid :kline kline :color gridColor}
          {:type :kline :kline kline}])

       :volume
       [{:type :line :line (stl/volume kline) :color "red"}
        {:type :grid :line (stl/volume kline) :color "#555" :hideY true}]

       :clock
       (let [{cs :sma z :z v-z :v-z} (stf/clock 10 kline)]
         [{:type :clock :cz z :vz v-z :color "white"}])

       :kline
       [{:type :grid :kline kline :color "#555"}
        {:type :kline :kline kline}]))))