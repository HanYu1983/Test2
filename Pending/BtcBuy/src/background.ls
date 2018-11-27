chrome.runtime.onInstalled.addListener ->
    console.log('nothing to do')
    
    
    /*
    // 確定background也可以使用websocket
    ws = new WebSocket 'wss://stream.binance.com:9443/ws/xrpbtc@trade'
        ..onopen = (evt) ->
            console.log evt

        ..onclose = (evt) ->
            console.log evt
    
        ..onmessage = ({data}) ->
            {p} = JSON.parse(data)
            console.log p
        
        ..onerror = (evt) ->
            console.log evt
    */
    
    
    /*
    chrome.windows.getCurrent (w)->
        chrome.tabs.create {url: 'https://www.binance.com/tw/trade/XRP_BTC', windowId: w.id}, (t)->
    */
    
    setInterval do
        ->
            # 測試送訊息給tab
            # query url 需要加入 tabs permission
            chrome.tabs.query {url:'https://www.binance.com/tw/trade/XRP_BTC', currentWindow: true}, (t)->
                console.log "send Message"
                (res) <- chrome.tabs.sendMessage t[0].id, { message: 'buy' }
                console.log res
        5000