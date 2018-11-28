window.onload = ->
    console = chrome.extension.getBackgroundPage().console
    console.log "popup.js onload"
    
    delay = (t)->
        new Promise (res, rej)->
            setTimeout res, t
    
    async function download srcs
        console.log srcs.length
        cnt = 100
        num = Math.floor(srcs.length/ cnt)
        for i in [0 to num]
            s = i* cnt
            e = Math.min s + cnt, srcs.length
            console.log s, e
            for src in srcs.slice s, e
                chrome.downloads.download {url: src},(downloadId)->
            await delay 10000
    
    $('#btnDownloadPics').click ->
        (ts) <- chrome.tabs.query {active: true, currentWindow: true}
        chrome.tabs.sendMessage ts[0].id, {ask: "getImgs"}, (msg)->
            if not msg
                return
            {ask, answer} = msg
            if ask == "getImgs"
                srcs = answer.map (src)-> "https://fftcg.square-enix-games.com" ++ src
                download srcs