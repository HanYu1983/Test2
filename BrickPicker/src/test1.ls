require! {
    request
    async
    crypto
    ws: WebSocket
    pako
    "signalr-client": signalR
    zlib
    "./private/apiKey": ApiKey
}

console.log ApiKey

# --> : use for curry
getUrl = (opt, cb) -->
    callback = (error, response, body) ->
        if error
            cb error
        else
            cb null, body
    request(opt, callback);


binanceApiOption = (url, data, method)->
    query = Object.keys(data).reduce(((a, k)->a ++ ["#{k}=#{encodeURIComponent(data[k])}"]), []).join("&")
    opt = 
        url: url + '?' + query
        qs: data
        method: method
        timeout: 5000
        headers:
            'Content-type': "application/x-www-form-urlencoded"
            'X-MBX-APIKEY': ApiKey.binance.ApiKey

binanceSignedOption = (url, data, method)->
    data.timestamp = new Date().getTime()
    data.recvWindow = 5000
    query = Object.keys(data).reduce(((a, k)->a ++ ["#{k}=#{encodeURIComponent(data[k])}"]), []).join("&")
    signature = crypto.createHmac('sha256', ApiKey.binance.SecretKey).update(query).digest('hex')
    opt = 
        url: url + '?' + query + '&signature=' + signature
        qs: data
        method: method
        timeout: 5000
        headers:
            'Content-type': "application/x-www-form-urlencoded"
            'X-MBX-APIKEY': ApiKey.binance.ApiKey

/*
(err, {bids, asks}) <- getUrl 'https://api.binance.com/api/v1/depth?symbol=BTCUSDT'
console.log(bids)
sell1 = parseFloat(asks[0][0])


(err, {tick:{bids, asks}}) <- getUrl 'https://api.huobipro.com/market/depth?symbol=btcusdt&type=step0'
console.log(bids)
buy1 = parseFloat(bids[0][0])

console.log(buy1, sell1)


sellPrice = buy1
buyPrice = sell1

if sellPrice > buyPrice
    console.log 'you can move!'
else
    console.log 'can not move!'
*/

/*
(err, body) <- getUrl 'https://api.binance.com/api/v3/ticker/bookTicker?symbol=BTCUSDT'
console.log(body)

(err, body) <- getUrl 'https://api.huobipro.com/market/detail?symbol=btcusdt'
console.log(body)

(err, body) <- getUrl 'https://api.huobipro.com/market/depth?symbol=btcusdt&type=step1'
console.log(body)
*/


/*
updatePrice = (cb) ->
    (err, results) <- async.parallel [
        getUrl 'https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT'
        getUrl 'https://api.binance.com/api/v3/ticker/bookTicker?symbol=BTCUSDT'
        getUrl 'https://api.huobipro.com/market/detail?symbol=btcusdt'
        getUrl 'https://api.huobipro.com/market/depth?symbol=btcusdt&type=step1'
    ]
    [binanceNow, binanceBid, huobiNow, huobiBid] = results
    console.log binanceNow.price
    console.log binanceBid.bidPrice
    console.log huobiNow.tick.close
    console.log huobiBid.tick.bids[0][0]
*/

# <- updatePrice


(err, result) <- getUrl binanceSignedOption("https://api.binance.com/api/v3/account", {}, "GET")
console.log err, result

(err, result) <- getUrl binanceSignedOption("https://api.binance.com/api/v3/openOrders", {}, "GET")
console.log err, result

(err, result) <- getUrl binanceSignedOption("https://api.binance.com/api/v3/myTrades", {symbol: "BTCUSDT"}, "GET")
console.log err, result

(err, result) <- getUrl binanceSignedOption("https://api.binance.com/api/v3/order/test", {symbol: "BTCUSDT", side: "SELL", type: "MARKET", quantity: 1}, "POST")
console.log err, result

(err, result) <- getUrl binanceApiOption("https://api.binance.com/api/v1/userDataStream", {}, "POST")
{listenKey} = result |> JSON.parse
console.log err, result

dataStream = new WebSocket "wss://stream.binance.com:9443/ws/#{listenKey}"
    ..on 'open', ->
        console.log 'open'

    ..on 'close', ->
        console.log 'close'

    ..on 'message', (data) ->
        console.log JSON.parse(data)

    ..on 'error', (err) ->
        console.log err

/*
(err, result) <- getUrl binanceApiOption("https://api.binance.com/api/v1/userDataStream", {listenKey: listenKey}, "PUT")
console.log err, result

(err, result) <- getUrl binanceApiOption("https://api.binance.com/api/v1/userDataStream", {listenKey: listenKey}, "DELETE")
console.log err, result
*/