(ns tool.stock.tool
  (:require [clojure.spec.alpha :as s])
  (:require [tool.stock.spec]))

(s/def ::kline ::tool.stock.spec/kline)

(defn date [kline]
  (s/assert ::kline kline)
  (if (seq? kline)
    (map date kline)
    (let [[date _ _ _ _ _] kline]
      date)))
      
(defn high [kline]
  (s/assert ::kline kline)
  (if (seq? kline)
    (map high kline)
    (let [[_ _ high _ _ _] kline]
      high)))

(defn open [kline]
  (s/assert ::kline kline)
  (if (seq? kline)
    (map open kline)
    (let [[_ open _ _ _ _] kline]
      open)))
      
(defn close [kline]
  (s/assert ::kline kline)
  (if (seq? kline)
    (map close kline)
    (let [[_ _ _ _ close _] kline]
      close)))
      
(defn low [kline]
  (s/assert ::kline kline)
  (if (seq? kline)
    (map low kline)
    (let [[_ _ _ low _ _] kline]
      low)))
    
(defn volume [kline]
  (s/assert ::kline kline)
  (if (seq? kline)
    (map volume kline)
    (let [[_ _ _ _ _ volume] kline]
      volume)))
    
(defn mid [kline]
  (s/assert ::kline kline)
  (if (seq? kline)
    (map mid kline)
    (let [[_ _ high low _ _] kline]
      (/ (+ high low) 2))))
      
(defn typical [kline]
  (s/assert ::kline kline)
  (if (seq? kline)
    (map typical kline)
    (let [[_ _ high low close _] kline]
      (/ (+ high low close) 3))))


(defn test-it []
  (let [kline (s/assert ::kline [["" 0 0 0 0 0]])
        _ (high kline)
        kline (s/assert ::kline ["" 0 0 0 0 0])
        _ (high kline)]))