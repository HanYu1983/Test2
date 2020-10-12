(ns core
  (:require [shadow.react-native :refer (render-root)]
            ["react-native" :as rn]
            ["react" :as react]
            ["rxjs" :as rxjs]
            ["rxjs/operators" :as rxjs-op]
            [reagent.core :as r]))

(defonce splash-img (js/require "../assets/shadow-cljs.png"))

(def styles
  ^js (-> {:container
           {:flex 1
            :backgroundColor "#fff"
            :alignItems "center"
            :justifyContent "center"}
           :title
           {:fontWeight "bold"
            :fontSize 24
            :color "blue"}}
          (clj->js)
          (rn/StyleSheet.create)))

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
  [:> rn/View {:style (.-container styles)}
   [:> rn/Text {:style (.-title styles)} "Hello! wow !!"]
   (count-view "wow")
   (count-view "gan!")
   [:> rn/Image {:source splash-img :style {:width 200 :height 200}}]])

(defn start
  {:dev/after-load true}
  []
  (render-root "reactDev" (r/as-element [root])))

(defn main []
  (start))

