(ns tool.stock.formula
  (:require [clojure.spec.alpha :as s])
  (:require [tool.stock.tool]
            [tool.stock.spec]))

(s/def ::multi-kline ::tool.stock.spec/multi-kline)
(s/def ::values (s/coll-of number? :kind seqable?))

(defn average [vs]
  (s/assert ::values vs)
  (s/assert
   number?
   (-> (apply + vs)
       (/ (count vs)))))

(defn offset-seq [vs]
  (s/assert ::values vs)
  (s/assert
   ::values
   (map #(- %2 %1) vs (rest vs))))

; lazy-seq
; https://stackoverflow.com/questions/44095400/how-to-understand-clojures-lazy-seq
(defn nkline [cnt kline]
  (s/assert pos-int? cnt)
  (s/assert seqable? kline)
  (s/assert
   seqable?
   (let [group (take cnt kline)]
     (when-not (zero? (count group))
       (let [[date open _ _ _ _] (last group)
             high (apply max (tool.stock.tool/high group))
             low (apply min (tool.stock.tool/low group))
             close (tool.stock.tool/close (first group))
             volume (apply + (tool.stock.tool/volume group))]
         (lazy-seq (cons [date open high low close volume] (nkline cnt (drop cnt kline)))))))))

(defn sma-seq
  "移動平均線"
  [n vs]
  (s/assert pos-int? n)
  (s/assert ::values vs)
  (s/assert
   seqable?
   (when (>= (count vs) n)
     (let [fv (average (take n vs))]
       (reductions (fn [ma v]
                     (+ (* ma (/ (dec n) n)) (/ v n)))
                   fv
                   (drop n vs))))))

(defn ema-seq
  "指數移動平均線"
  [n vs]
  (s/assert pos-int? n)
  (s/assert ::values vs)
  (s/assert
   seqable?
   (let [fv (first (sma-seq n vs))
         alpha (/ 2 (inc n))]
     (reductions
      (fn [ema v]
        (+ (* (- v ema) alpha) ema))
      fv
      (drop n vs)))))

(defn macd-dif
  "指數差離指標"
  [n m kline]
  (s/assert pos-int? n)
  (s/assert pos-int? m)
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (map
    -
    (reverse (ema-seq n (reverse (tool.stock.tool/close kline))))
    (reverse (ema-seq m (reverse (tool.stock.tool/close kline)))))))


(defn StandardDeviation
  "Standard Deviation 標準差"
  [avg vs]
  (s/assert number? avg)
  (s/assert ::values vs)
  (s/assert
   number?
   (if (zero? (dec (count vs)))
     0
     (->> (apply + (map #(.pow js/Math (- % avg) 2)
                        vs))
          (* (/ 1 (dec (count vs))))
          (.sqrt js/Math)))))

(defn z-score [avg sd vs]
  (s/assert number? avg)
  (s/assert number? sd)
  (s/assert ::values vs)
  (s/assert
   seqable?
   (let [offsets (map (fn [v]
                        (- v avg))
                      vs)
         vs (map #(/ % sd)
                 offsets)]
     vs)))

(defn yu-clock
  "余氏背離線"
  [n kline]
  (s/assert pos-int? n)
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (let [ps (sma-seq n (tool.stock.tool/mid kline))
         ps-avg (average ps)
         ps-sd (StandardDeviation ps-avg ps)
         ps-z (z-score ps-avg ps-sd ps)

         vs (sma-seq n (tool.stock.tool/volume kline))
         vs-avg (average vs)
         vs-sd (StandardDeviation vs-avg vs)
         vs-z (z-score vs-avg vs-sd vs)

         ps (map vector vs-z ps-z)

         axis [0.707 -0.707]

         dot (fn [l1 l2]
               (apply + (map * l1 l2)))

         length (fn [line]
                  (.sqrt js/Math (dot line line)))

         normalize (fn [line]
                     (map #(/ % (length line)) line))

         projs (map
                (fn [prev curr]
                  (let [dir (mapv - curr prev)]
                    (dot (normalize dir) axis)))
                ps
                (rest ps))]
     projs)))

(defn clock [n kline]
  (s/assert pos-int? n)
  (s/assert ::multi-kline kline)
  (s/assert
   any?
   (let [cs (sma-seq n (tool.stock.tool/mid kline))
         avg (average cs)
         sd (StandardDeviation avg cs)
         z (z-score avg sd cs)

         vs (sma-seq n (tool.stock.tool/volume kline))
         v-avg (average vs)
         v-sd (StandardDeviation v-avg vs)
         v-z (z-score v-avg v-sd vs)]
     {:sma cs
      :avg avg
      :sd sd
      :z z
      :v-sma vs
      :v-avg v-avg
      :v-sd v-sd
      :v-z v-z})))

(defn xy-direction [n x y]
  (s/assert number? n)
  (s/assert number? x)
  (s/assert number? y)
  (s/assert
   int?
   (let [v (-> (.atan2 js/Math y x)
               (+ (/ -3.14 n))
               (* (/ 1 (/ 6.28 n)))
               int)]
     v)))

(defn clock-direction [x-seq y-seq]
  (s/assert ::values x-seq)
  (s/assert ::values y-seq)
  (s/assert
   seqable?
   (map (partial xy-direction 8) (offset-seq x-seq) (offset-seq y-seq))))

(defn BBI
  "Bull and Bear Index 多空指標
  利用ema(5)和BBI(12)的差離值(macd)的圖形，和rsv(100)後的sma(3)和sma(9)的曲線圖形幾乎無二致!!"
  [n m o p vs]
  (s/assert pos-int? n)
  (s/assert pos-int? m)
  (s/assert pos-int? o)
  (s/assert pos-int? p)
  (s/assert ::values vs)
  (s/assert
   seqable?
   (let [n1 (reverse (sma-seq n (reverse vs)))
         n2 (reverse (sma-seq m (reverse vs)))
         n3 (reverse (sma-seq o (reverse vs)))
         n4 (reverse (sma-seq p (reverse vs)))]
     (map (fn [& args]
            (-> (apply + args) (/ 4)))
          n1 n2 n3 n4))))

(defn EBBI
  "指數多空指標"
  [n m o p vs]
  (s/assert pos-int? n)
  (s/assert pos-int? m)
  (s/assert pos-int? o)
  (s/assert pos-int? p)
  (s/assert ::values vs)
  (s/assert
   seqable?
   (let [n1 (reverse (ema-seq n (reverse vs)))
         n2 (reverse (ema-seq m (reverse vs)))
         n3 (reverse (ema-seq o (reverse vs)))
         n4 (reverse (ema-seq p (reverse vs)))]
     (map (fn [& args]
            (-> (apply + args) (/ 4)))
          n1 n2 n3 n4))))

(defn sar-seq
  "拋物線指標"
  [n reverse-kline]
  (s/assert pos-int? n)
  (s/assert ::multi-kline reverse-kline)
  (s/assert
   seqable?
   (when (>= (count reverse-kline) n)
     (let [low (apply min (tool.stock.tool/low (take n reverse-kline)))]
       (->> (iterate (fn [[value ori prev curr act af]]
                       (let [[_ _ ph pl _ _] (first prev)
                             [_ _ ch cl _ _] (first curr)
                             should-turn (condp = act
                                           :buy
                                           (> value pl)
                                           :sell
                                           (< value ph))
                             next-value (if should-turn
                                          (condp = act
                                            :buy
                                            (apply max (map (fn [[_ _ high _ _ _]]
                                                              high)
                                                            (take n ori)))
                                            :sell
                                            (apply min (map (fn [[_ _ _ low _ _]]
                                                              low)
                                                            (take n ori))))
                                          (+ value (* af (- pl value))))


                             next-af (condp = act
                                       :buy
                                       (if should-turn
                                         0.02
                                         (if (> ch ph)
                                           (max (+ 0.04 af) 0.2)
                                           (max (+ 0.02 af) 0.2)))
                                       :sell
                                       (if should-turn
                                         0.02
                                         (if (< cl pl)
                                           (max (+ 0.04 af) 0.2)
                                           (max (+ 0.02 af) 0.2)))
                                       :else
                                       af)

                             next-act (if should-turn
                                        (condp = act
                                          :buy :sell
                                          :sell :buy)
                                        act)]
                         [next-value (rest ori) (rest prev) (rest curr) next-act next-af]))
                     [low reverse-kline (drop (dec n) reverse-kline) (drop n reverse-kline) :buy 0.2])
            (map first)
            (take (count reverse-kline))
            (drop-last (dec n)))))))


(defn AccDist
  "累積/派發線"
  [kline]
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (reductions + 0 (map (fn [[_ _ high low close volume]]
                          (if (= high low)
                            0
                            (* (- (- close low) (- high close)) (/ volume (- high low)))))
                        kline))))

(defn Chaikin
  "蔡金指標"
  [n m kline]
  (s/assert pos-int? n)
  (s/assert pos-int? m)
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (map -
        (reverse (ema-seq n (AccDist (reverse kline))))
        (reverse (ema-seq m (AccDist (reverse kline)))))))


(defn EOM
  "Ease Of Movement (EOM) 簡易波動指標"
  [n kline]
  (s/assert pos-int? n)
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (let [mid-move (map  #(- %1 %2)
                        (tool.stock.tool/mid kline)
                        (rest (tool.stock.tool/mid kline)))
         BoxRatio (map (fn [[_ _ high low _ volume]]
                         (/ volume (- high low)))
                       kline)
         eom (map #(/ %1 %2)
                  mid-move
                  (rest BoxRatio))]
     (reverse (sma-seq n (reverse eom))))))


(defn yu-gv
  "地量"
  [n kline]
  (s/assert pos-int? n)
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   #_(when (>= (count kline) n)
       (let [group (take n kline)
             vs (tool.stock.tool/volume group)
             avg (average vs)
             sd (StandardDeviation avg vs)
             z (z-score avg sd vs)]
         (cons (first z) (lazy-seq (yu-gv n (rest kline))))))
   (when (>= (count kline) n)
     (let [group (take n kline)
           vs (tool.stock.tool/volume group)
           avg (average vs)
           sd (StandardDeviation avg vs)
           z (z-score avg sd vs)]
       (lazy-seq (cons (first z) (yu-gv n (rest kline))))))))


(defn maxN-seq
  [n f vs]
  (s/assert pos-int? n)
  (s/assert fn? f)
  (s/assert ::values vs)
  (s/assert
   seqable?
   (when (>= (count vs) n)
     (let [g (take n vs)
           k (f g)]
       (lazy-seq (cons k  (maxN-seq n f (rest vs))))))))

(defn rsv-seq
  "未成熟隨機值"
  [n kline]
  (s/assert pos-int? n)
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (let [h9 (maxN-seq n #(apply max %) (tool.stock.tool/high kline))
         l9 (maxN-seq n #(apply min %) (tool.stock.tool/low kline))
         c (tool.stock.tool/close kline)]
     (map (fn [c l h]
            (* (- c l) (/ 1 (- h l))))
          c l9 h9))))

(defn yu-car
  "余氏方向盤指標"
  [n w d reverse-kline]
  (s/assert pos-int? n)
  (s/assert number? w)
  (s/assert number? d)
  (s/assert ::multi-kline reverse-kline)
  (s/assert
   any?
   (let [normal (-> (.pow js/Math 1.07 n)
                    (- 1))
         up-seq (map (partial * (/ 1 normal))
                     (offset-seq (tool.stock.tool/mid reverse-kline)))
         vs (->> (reductions (fn [[prev ran] up-offset]
                               (let [max-v (+ prev (if (pos? up-offset) ran (/ ran 2)))
                                     min-v (- prev (if (neg? up-offset) ran (/ ran 2)))]
                                 (if (> max-v up-offset min-v)
                                   [up-offset (* ran d)]
                                   [(if (> up-offset max-v)
                                      max-v
                                      min-v)
                                    (+ ran (* (max (- up-offset max-v)
                                                   (- min-v up-offset))
                                              w))])))
                             [(first up-seq)
                              normal]
                             (rest up-seq)))]
     ; vector的first和list的first為倒反
     [(map second vs) (map first vs)])))

(defn volatility-seq
  "計算波動
  從後面計算"
  [n vs]
  (s/assert pos-int? n)
  (s/assert ::values vs)
  (s/assert
   seqable?
   (map #(/ (- %1 %2) %2)
        vs
        (drop n vs))))

(defn osc-seq
  "振盪量指標osc
  可以取代mtm動量指標"
  [n vs]
  (s/assert pos-int? n)
  (s/assert ::values vs)
  (s/assert
   seqable?
   (map #(/ %1 %2)
        vs
        (drop n vs))))

(defn dpo-seq
  "Detrended Price Oscillator (DPO) 區間震蕩線"
  [n kline]
  (s/assert pos-int? n)
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (let [ma (reverse (sma-seq n (tool.stock.tool/close (reverse kline))))
         c (tool.stock.tool/close kline)]
     (map -
          c
          (drop (inc (/ n 2)) ma)))))

(defn rsi-seq
  "強弱指標"
  [n vs]
  (s/assert pos-int? n)
  (s/assert ::values vs)
  (s/assert
   seqable?
   (let [offsets (offset-seq vs)
         upavg (sma-seq n (map (fn [v] (if (pos? v) v 0)) offsets))
         downavg (sma-seq n (map (fn [v] (if (neg? v) (.abs js/Math v) 0)) offsets))]
     (map (fn [u d]
            (/ u (+ u d)))
          upavg
          downavg))))


(defn tr-seq
  "真實波幅
  從前面計算"
  [kline]
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (map (fn [close high low]
          (max (- high low) (.abs js/Math (- high close)) (.abs js/Math (- low close))))
        (tool.stock.tool/close kline)
        (rest (tool.stock.tool/high kline))
        (rest (tool.stock.tool/low kline)))))

(defn tl-seq
  "真實低價
  從前面計算"
  [kline]
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (map
    (partial min)
    (tool.stock.tool/close kline)
    (rest (tool.stock.tool/low kline)))))

(defn uos-seq
  "終極指標
  從前面計算"
  [m n o kline]
  (s/assert pos-int? m)
  (s/assert pos-int? n)
  (s/assert pos-int? o)
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (let [tl (tl-seq kline)
         bp (map - (rest (tool.stock.tool/close kline)) tl)
         tr (tr-seq kline)
         ruo (map (fn [b1 b2 b3 t1 t2 t3]
                    (+ (* 4 (/ b1 t1)) (* 2 (/ b2 t2)) (/ b3 t3)))
                  (reverse (map #(* m %) (sma-seq m bp)))
                  (reverse (map #(* n %) (sma-seq n bp)))
                  (reverse (map #(* o %) (sma-seq o bp)))
                  (reverse (map #(* m %) (sma-seq m tr)))
                  (reverse (map #(* n %) (sma-seq n tr)))
                  (reverse (map #(* o %) (sma-seq o tr))))
         uos (map #(* (/ 100 7) %)
                  ruo)]
     (reverse uos))))

(defn dm-seq
  "趨向變動值
  從前面計算"
  [kline]
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (->> (map (fn [a b c d]
               [(max 0 (- c a)) (max 0 (- b d))])
             (tool.stock.tool/high kline)
             (tool.stock.tool/low kline)
             (rest (tool.stock.tool/high kline))
             (rest (tool.stock.tool/low kline)))
        (map (fn [[v1 v2]]
               (condp = (max v1 v2)
                 v1 v1
                 v2 (- v2)
                 0))))))


(defn atr-seq
  "真實波幅"
  [n kline]
  (s/assert pos-int? n)
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (sma-seq n (tr-seq kline))))


(defn cci-seq
  "Commodity Channel Index (CCI) 順勢指標"
  [n kline]
  (s/assert pos-int? n)
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (when (>= (count kline) n)
     (let [factor (/ 1 0.015)
           ps (take n (tool.stock.tool/typical kline))
           ps-avg (average ps)
           ps-sd (StandardDeviation ps-avg ps)
           z (last (z-score ps-avg ps-sd ps))
           v (* factor z)]
       (lazy-seq (cons v  (cci-seq n (rest kline))))))))

(defn trix-seq
  "Triple Exponential (TRIX) 三重指數平滑移動平均指標
  從後面算"
  [n vs]
  (s/assert pos-int? n)
  (s/assert ::values vs)
  (s/assert
   seqable?
   (let [ax (ema-seq n (reverse vs))
         bx (ema-seq n ax)
         cx (ema-seq n bx)
         vs (volatility-seq 1 (reverse cx))]
     vs)))

(defn money-line [n vs]
  (s/assert pos-int? n)
  (s/assert ::values vs)
  (s/assert
   seqable? 
   (when (>= (count vs) n)
     (let [group (take n vs)
           start (last group)
           offset (map #(- % start)
                       group)
           v (-> (apply + offset)
                 (/ n)
                 (/ start))]
       (lazy-seq (cons v (money-line n (rest vs))))))))

(defn kline-red [kline]
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (map (fn [[_ o h l c _]]
          (+ (max 0 (- h o)) (max 0 (- c l))))
        kline)))

(defn kline-green [kline]
  (s/assert ::multi-kline kline)
  (s/assert
   seqable?
   (map (fn [[_ o h l c _]]
          (+ (max 0 (- o l)) (max 0 (- h c))))
        kline)))

(defn up-rate [vs]
  (s/assert ::values vs)
  (s/assert
   seqable?
   (map (fn [p c]
          (/ (- c p) p))
        vs
        (rest vs))))