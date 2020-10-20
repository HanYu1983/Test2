(ns tool.stock.spec
  (:require [clojure.spec.alpha :as s]))

(s/def ::date string?)
(s/def ::high number?)
(s/def ::low number?)
(s/def ::open number?)
(s/def ::close number?)
(s/def ::volume number?)
(s/def ::single-kline (s/tuple ::date ::open ::high ::low ::close ::volume))
(s/def ::multi-kline (s/coll-of ::single-kline :kind seqable?))
(s/def ::kline (s/or :multi ::multi-kline
                     :single ::single-kline))
