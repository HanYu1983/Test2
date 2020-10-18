(ns background)

(defn main []
  (.addListener js/chrome.runtime.onInstalled
                (fn []
                  (js/console.log "xx")
                  (js/console.log "on install")))
  (println "abc"))