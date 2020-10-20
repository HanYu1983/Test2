(ns showK-chart
  (:require [reagent.core :as r]
            [clojure.core.async :as a]
            [clojure.spec.alpha :as s]
            [clojure.test.check.generators :as g]
            [clojure.core.match :refer [match]]
            ["rxjs" :as rxjs]
            ["rxjs/operators" :as rxjs-op])
  (:require [tool.stock.spec]
            [tool.stock.formula]
            [tool.stock.formula-drawer]
            [tool.stock.drawer]))

(s/check-asserts true)

(s/def ::enable boolean?)
(s/def ::kline ::tool.stock.spec/kline)
(s/def ::model (s/keys :req-un [::enable]
                       :opt-un [::kline]))

(defn fetch-data []
  (a/go
    (let [table (js/$ "#divK_ChartDetail > div > div > table")
          rows (.find table "tbody > tr")
          ; jquery dom list
          parse (-> rows
                    (.map (fn [i]
                            (let [info (-> (aget rows i)
                                           (js/$)
                                           (.find "td"))
                                  date (aget info 0)
                                  open (aget info 1)
                                  high (aget info 2)
                                  low (aget info 3)
                                  close (aget info 4)
                                  volume (aget info 7)
                                  [d & rest] (map (fn [dom]
                                                    (-> (js/$ dom)
                                                        (.find "nobr")
                                                        (.text)
                                                        (.replace #"," "")))
                                                  [date open high low close volume])
                                  parse (->> (cons d (map js/parseFloat rest))
                                             (into []))]
                              parse)))
                    seq)]
      parse)))

(defn auto-click []
  (a/go
    (let [; https://goodinfo.tw/StockInfo/ShowK_Chart.asp?STOCK_ID=2303
          _ (a/<! (a/timeout 3000))
          ; 修改範圍
          _ (-> (js/$ "#selK_ChartPeriod")
                (.val "365"))
          ; 發送修改事件觸發網頁的js
          ; 不能透過jquery來trigger，因為這是在sendbox，必須直接操做dom
          _ (-> (js/$ "#selK_ChartPeriod")
                (aget 0)
                (.dispatchEvent (js/Event. "change")))
          ; 等待資料讀取
          _ (a/<! (a/timeout 5000))])))


(defn Canvas [{:keys [width height render]}]
  (let [state (atom nil)]
    (r/create-class
     {:reagent-render (fn []
                        (let [update-size (fn [el]
                                            (when el
                                              (let [ctx (.getContext el "2d")]
                                                (render ctx))))]
                          (fn [] (let [{:keys [size]} @state]
                                   [:canvas {:style  {:width width :height height}
                                             :ref    update-size
                                             :width  (nth size 0)
                                             :height (nth size 1)}]))))
      :component-did-mount (fn [] (reset! state {:size nil}))})))

(defn view [atom-ctx output]
  (let [root (fn []
               (let [ctx @atom-ctx]
                 (if (:enable ctx)
                   [:div {:style {:position "fixed"
                                  :top 50
                                  :width 300
                                  :height 300
                                  :z-index 1000
                                  :background-color "gray"}}
                    [:div {:style {:display "flex"
                                   :flex-direction "column"}}
                     [:div {:on-click #(.next output :on-click-enable)} "close"]
                     [:canvas {:style {:flex 1
                                       :background-color "red"}
                               :ref (fn [dom]
                                      (println "render")
                                      (when dom
                                        (let [canvas-ctx (.getContext dom "2d")
                                              canvas-w (.-width dom)
                                              canvas-h (.-height dom)
                                              rand-kline (g/let [d (g/return "")
                                                                 v1 g/nat
                                                                 v2 (g/choose 5 20)
                                                                 v3 (g/choose 0 10)
                                                                 v4 (g/choose 0 10)
                                                                 v5 g/nat]
                                                           [d v1 v2 v3 v4 v5])
                                              kline (s/assert
                                                     ::tool.stock.spec/kline
                                                     (or (-> @ctx :kline)
                                                         (->> (repeatedly #(g/generate rand-kline))
                                                              (tool.stock.formula/nkline 3)
                                                              (take 40))))
                                              drawers `(~@(tool.stock.formula-drawer/data->drawer kline :kline)
                                                        ~@(tool.stock.formula-drawer/data->drawer kline [:sar 5])
                                                        ~@(tool.stock.formula-drawer/data->drawer kline [:ema 5 10 15 20 25]))
                                              _ (tool.stock.drawer/draw {:drawers drawers} canvas-w canvas-h canvas-ctx)])))}]
                     [:input {:type "button"
                              :value "load"
                              :on-click #(.next output :on-click-load)}]
                     [:div {} (str (count (:data @atom-ctx)))]]]

                   [:div {:style {:position "fixed"
                                  :top 50
                                  :width 300
                                  :height 300
                                  :z-index 1000
                                  :background-color "green"}
                          :on-click #(.next output :on-click-enable)}])))
        rootDom (js/document.createElement "div")
        _ (.appendChild js/document.body rootDom)
        _ (r/render [root] rootDom)]))

(defn main []
  (let [view-evt (rxjs/Subject.)
        control-evt (rxjs/Subject.)
        input (-> (rxjs/merge view-evt
                              control-evt)
                  (.pipe (rxjs-op/scan (fn [ctx evt]
                                         (println evt ctx)
                                         (match evt
                                           :on-click-enable
                                           (update ctx :enable not)
                                           
                                           :on-click-load
                                           (let [_ (a/go
                                                     (let [data (a/<! (fetch-data))
                                                           _ (.next control-evt [:fetch-data data])]))]
                                             ctx)

                                           [:fetch-data data]
                                           (assoc ctx :kline data)

                                           :else
                                           ctx))
                                       (s/assert ::model {:enable false}))
                         (rxjs-op/tap (fn [ctx]
                                        (s/assert ::model ctx)))))

        atom-ctx (r/atom nil)
        _ (.subscribe input
                      (fn [ctx]
                        (reset! atom-ctx ctx)))
        _ (view atom-ctx view-evt)]))