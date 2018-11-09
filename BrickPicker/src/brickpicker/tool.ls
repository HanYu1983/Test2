require! {
    request
    crypto
    "../private/apiKey": ApiKey
    
    moment
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

export huobiSignedOption = (baseurl, path, args, method)->
    data = 
        AccessKeyId: ApiKey.huobi.AccessKey
        SignatureMethod: "HmacSHA256"
        SignatureVersion: "2"
        Timestamp: moment.utc().format('YYYY-MM-DDTHH:mm:ss')

    # get，要加密，所以放在算法前面
    if method == "GET"
        for k, v of args
            data[k] = v

    p = Object.keys(data).reduce(((a, k)->a ++ ["#{k}=#{encodeURIComponent(data[k])}"]), []).sort().join("&")
    meta = [method, baseurl, path, p].join('\n');
    hash = crypto.createHmac('sha256', ApiKey.huobi.SecretKey).update(meta).digest('base64')
    signature = encodeURIComponent(hash)

    # post的話，不必加密，所以放在算法後面
    if method == "POST"
        for k, v of args
            data[k] = v

    opt = 
        url: "https://"+ baseurl + path + "?" + p + '&Signature=' + signature
        body: JSON.stringify(data)
        method: method
        timeout: 5000
        headers:
            'Content-type': "application/json"