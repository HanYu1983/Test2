(ns showK-chart)

(defn main []  
  (let [btn (js/document.createElement "button")
        _ (set! (.-innerHTML btn) "CLICK ME")
        _ (.appendChild js/document.body btn)])
  (println "injected!!"))