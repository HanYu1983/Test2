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

observer.observe cfg, 'xrpbtc', (history)->
    
    (err, result) <- sendUrl binanceSignedOption("https://api.binance.com/api/v3/order/test", {symbol: "BTCUSDT", side: "SELL", type: "MARKET", quantity: 1}, "POST")
    console.log err, result
    #console.log history