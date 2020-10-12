(ns core
  (:require [shadow.react-native :refer (render-root)]
            ["react-native" :as rn]
            ["react" :as react]
            ["rxjs" :as rxjs]
            ["rxjs/operators" :as rxjs-op]
            [reagent.core :as r]))

(defonce splash-img (js/require "../assets/shadow-cljs.png"))

(def model (r/atom {:count 0}))
(def input (rxjs/Subject.))

(let [_ (-> (rxjs/Subject.)
            (.pipe (rxjs-op/merge input)
                   (rxjs-op/filter (fn [e]
                                     (= [:on-click-count-view] e))))
            (.subscribe (fn []
                          (js/console.log rxjs/merge) ; rxjs/merge is nil, why?
                          (js/console.log rxjs)
                          (swap! model (fn [model]
                                         (update model :count dec))))))])

(defn count-view [name]
  [:> rn/TouchableOpacity {:style {:backgroundColor "#0f0"}
                           :onPress (fn []
                                      (.next input [:on-click-count-view]))}
   [:> rn/Text {} (str name "-" (:count @model))]])

(defn root []
  [:> rn/View {:style {:flex 1}}
   [:> rn/ScrollView nil
    (map (fn [data]
           ; 使用[count-view ...]建構component, 而不是使用(count-view ...)
           ; 使用metadata ^{:key id} 給予id 
           ; https://reagent-project.github.io/
           ^{:key (:id data)} [count-view (:title data)])
         [{:id "1" :title "wow"}
          {:id "2" :title "gan"}
          {:id "3" :title "kan"}])]
   [:> rn/Image {:source splash-img :style {:width 200 :height 200}}]])

(defn start
  ;{:dev/after-load true}
  []
  (render-root "reactDev" (r/as-element [root])))

(defn main []
  (start))

