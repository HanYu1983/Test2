require! {
    request
    crypto
    "../private/apiKey": ApiKey
}

export getUrl = (opt, cb) -->
    callback = (error, response, body) ->
        if error
            cb error
        else
            cb null, body
    request(opt, callback);

export binanceApiOption = (url, data, method)->
    query = Object.keys(data).reduce(((a, k)->a ++ ["#{k}=#{encodeURIComponent(data[k])}"]), []).join("&")
    opt = 
        url: url + '?' + query
        qs: data
        method: method
        timeout: 5000
        headers:
            'Content-type': "application/x-www-form-urlencoded"
            'X-MBX-APIKEY': ApiKey.binance.ApiKey

export binanceSignedOption = (url, data, method)->
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