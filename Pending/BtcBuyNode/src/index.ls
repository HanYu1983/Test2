require! {
    "./observer"
    "./config.json": cfg
    "./private/binanceKey.json": apiKey
    "request"
    "crypto"
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
    console.log err, result
    #console.log history