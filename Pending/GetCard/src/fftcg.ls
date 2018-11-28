console.log "addListener"

chrome.runtime.onMessage.addListener ({ask}:message, sender, sendResponse)->
    console.log message
    switch ask
        | "getImgs" =>
            imgs = $('img[class="thumb"]')
            srcs = for img in imgs
                $(img).attr "src"
            srcs = srcs.map (src)->
                src.replace /thumbs/, "full"
            console.log imgs
            console.log srcs
            sendResponse({ ask: ask, answer:srcs })
        | otherwise =>
            sendResponse({ ask: ask, answer:ask })
    