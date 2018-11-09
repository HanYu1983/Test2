require! {
    request
    crypto
    moment
    "./private/apiKey": ApiKey
    "./brickpicker/tool": Tool
}

huobiSignedOption = (baseurl, path, args, method)->
    data = 
        AccessKeyId: ApiKey.huobi.AccessKey
        SignatureMethod: "HmacSHA256"
        SignatureVersion: 2
        Timestamp: moment.utc().format('YYYY-MM-DDTHH:mm:ss')
    
    # get，要加密，所以放在算法前面
    if method == "GET"
        for k, v of args
            data[k] = v
    
    p = Object.keys(data).reduce(((a, k)->a ++ ["#{k}=#{encodeURIComponent(data[k])}"]), []).sort().join("&")
    meta = [method, baseurl, path, p].join('\n');
    hash = crypto.createHmac('sha256', ApiKey.huobi.SecretKey).update(meta).digest('base64')
    signature = encodeURIComponent(hash)
    
    /*  
    sign = ApiKey.huobi.TradePassword + 'hello, moto'
    md5 = crypto.createHash('md5').update(sign).digest("hex").toLowerCase()
    authData = encodeURIComponent(JSON.stringify({assetPwd: md5}))
    */
    
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
    console.log opt
    opt

(err, res) <- Tool.getUrl huobiSignedOption("api.huobipro.com", "/v1/account/accounts", {}, "GET")
console.log res

info = JSON.parse(res)
console.log info
if info.status == "ok"
    account_id = info.data[0].id
    
    #(err, res) <- Tool.getUrl huobiSignedOption("api.huobipro.com", "/v1/account/accounts/#{account_id}/balance", {}, "GET")
    #console.log res
    
    #
    (err, res) <- Tool.getUrl huobiSignedOption("api.huobipro.com", "/v1/order/orders/place", {"account-id": account_id, type: "buy-limit", amount: 1, symbol:"btcusdt", price: 1}, "POST")
    console.log res

(err, res) <- Tool.getUrl huobiSignedOption("api.huobipro.com", "/v1/order/orders", {symbol:"btcusdt", states: "submitted,partial-filled"}, "GET")
console.log res


