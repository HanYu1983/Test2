require! {
    "./observer"
    "./config.json": cfg
    "./private/binanceKey.json": apiKey
    "request"
    "crypto"
    websocket
}

sendUrl = (opt, cb) -->
    callback = (error, response, body) ->
        if error
            cb error
        else
            cb null, body
    request(opt, callback)

# https://support.binance.com/hc/en-us/articles/115003372072
binanceSignedOption = (url, data, method)->
    data.timestamp = new Date().getTime()
    data.recvWindow = 5000
    query = Object.keys(data).reduce(((a, k)->a ++ ["#{k}=#{encodeURIComponent(data[k])}"]), []).join("&")
    signature = crypto.createHmac('sha256', apiKey.SecretKey).update(query).digest('hex')
    opt = 
        url: url + '?' + query + '&signature=' + signature
        qs: data
        method: method
        timeout: 5000
        headers:
            'Content-type': "application/x-www-form-urlencoded"
            'X-MBX-APIKEY': apiKey.ApiKey

binanceApiOption = (url, data, method)->
    query = Object.keys(data).reduce(((a, k)->a ++ ["#{k}=#{encodeURIComponent(data[k])}"]), []).join("&")
    opt = 
        url: url + '?' + query
        qs: data
        method: method
        timeout: 5000
        headers:
            'Content-type': "application/x-www-form-urlencoded"
            'X-MBX-APIKEY': apiKey.ApiKey

observeAccount = (cb)->
    (err, result) <- sendUrl binanceApiOption("https://api.binance.com/api/v1/userDataStream", {}, "POST")
    if err
        return cb err
    
    {listenKey} = result |> JSON.parse
    client = new websocket.client()

    client.on 'connectFailed', (error)->
        console.log('Connect Error: ' + error.toString())

    client.on 'connect', (connection)->
        console.log('WebSocket Client Connected')
    
        connection.on 'error', (error)->
            console.log "Connection Error: " + error.toString()

        connection.on 'close', ->
            console.log 'echo-protocol Connection Closed'

        connection.on 'message', (message)->
            if message.type == 'utf8'
                data = message.utf8Data |> JSON.parse _
                console.log data
                cb null, data
    
    client.connect "wss://stream.binance.com:9443/ws/#{listenKey}"



local =
    orders: []

observeAccount (err, res)->
    console.log err, res
    if err
        return console.log err
        
    {e:evt, r:errCode} = res
    if errCode != "NONE"
        return console.log errCode
        
    switch evt
        | "executionReport" =>
            for order in local.orders
                order.onReport res
                
        | otherwise =>
    

symbol = "xrpbtc"

# 取得存款
(err, result) <- sendUrl binanceSignedOption("https://api.binance.com/api/v3/account", {}, "GET")
console.log err, result

# 測試下單
# '{"code":-1013,"msg":"Filter failure: MIN_NOTIONAL"}'
# https://www.reddit.com/r/binance/comments/74ocol/api_errorfilter_failure_min_notional/
(err, result) <- sendUrl binanceSignedOption("https://api.binance.com/api/v3/order/test", {symbol: symbol.toUpperCase(), side: "BUY", type: "MARKET", quantity: 1}, "POST")
console.log err, result

# 啟動監聽
observer.observe cfg, symbol, (history)->
    (err, result) <- sendUrl binanceSignedOption("https://api.binance.com/api/v3/order/test", {symbol: symbol.toUpperCase(), side: "BUY", type: "MARKET", quantity: 1}, "POST")
    local.orders.push do
        info: result,
        onReport: ({i:orderId, X:orderStatus, p:price})->
            # 若訂單是等得成交狀態，等到訂單成交後就賣
            if i == this.info.orderId && orderStatus == "FILLED"
                sellPrice = p * cfg.earnRate
                (err, result) <- sendUrl binanceSignedOption("https://api.binance.com/api/v3/order/test", {symbol: symbol.toUpperCase(), side: "SELL", type: "LIMIT", quantity: 1, price: sellPrice}, "POST")
                