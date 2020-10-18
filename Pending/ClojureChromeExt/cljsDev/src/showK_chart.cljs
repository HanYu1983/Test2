(ns showK-chart
  (:require [reagent.core :as r]))

(def click-count (r/atom 0))

(defn counting-component
  []
  [:div {:style {:position "absolute"
                 :top 50
                 :width 300
                 :height 300
                 :background-color "gray"}}
   [:div {:style {:display "flex"}}
    [:div {:style {:background-color "red"
                   :flex 1}}]
    "The state has a value: " @click-count
    [:input {:type "button"
             :value "Click me!"
             :on-click #(swap! click-count inc)}]]])

(defn root []
  (counting-component))

(defn main []
  (let [rootDom (js/document.createElement "div")
        _ (.appendChild js/document.body rootDom)
        _ (r/render [root] rootDom)
        _ (println "injected!!")]))