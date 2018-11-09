require! {
    request
    crypto
    moment
    "./brickpicker/tool": Tool
    "ws": WebSocket
    pako
    "signalr-client": signalR
    zlib
    "./private/apiKey": ApiKey
}

/*
(err, res) <- Tool.getUrl Tool.huobiSignedOption("api.huobipro.com", "/v1/account/accounts", {}, "GET")
console.log res

info = JSON.parse(res)
console.log info
if info.status == "ok"
    account_id = info.data[0].id
    
    (err, res) <- Tool.getUrl Tool.huobiSignedOption("api.huobipro.com", "/v1/account/accounts/#{account_id}/balance", {}, "GET")
    console.log res
    
    (err, res) <- Tool.getUrl Tool.huobiSignedOption("api.huobipro.com", "/v1/order/orders/place", {"account-id": account_id, type: "buy-limit", amount: 1, symbol:"btcusdt", price: 1}, "POST")
    console.log res

(err, res) <- Tool.getUrl Tool.huobiSignedOption("api.huobipro.com", "/v1/order/orders", {symbol:"btcusdt", states: "submitted,partial-filled"}, "GET")
console.log res


*/


/*
huobiWs = new WebSocket 'wss://api.huobi.pro/ws'
    ..on 'open', ->
        console.log 'open'
        sendData = {sub: 'market.btcusdt.trade.detail', id: 'xx20'}
        huobiWs.send(JSON.stringify(sendData), (err)->console.log(err))
        
    ..on 'close', ->
        console.log 'close'

    ..on 'message', (buf) ->
        data = 
            pako.inflate(buf, {to:'string'}) |> 
            JSON.parse _
        console.log data
        
        if data.hasOwnProperty "ping"
            {ping} = data
            sendData = 
                {pong: ping} |>
                JSON.stringify _
            huobiWs.send(sendData, (err)->console.log(err))
        
        if data['ch'] == 'market.btcusdt.depth.step0'
            {tick:{bids, asks}} = data
            console.log bids
        
        if data['ch'] == 'market.btcusdt.trade.detail'
            {tick:{data:data2}} = data
            console.log data2
    ..on 'error', (err) ->
        console.log err
*/

#(err, res) <- Tool.getUrl Tool.huobiSignedOption("api.huobi.pro", "/ws/v1", {op:"auth", cid: ""}, "GET")
#console.log res


baseurl = "api.huobi.com"
path = "/ws/v1"
url = "wss://#{baseurl}#{path}"
console.log url

huobiWs2 = new WebSocket url
    ..on 'open', ->
        console.log 'open'
        
        method = "GET"
        data = 
            AccessKeyId: ApiKey.huobi.AccessKey
            SignatureMethod: "HmacSHA256"
            SignatureVersion: "2"
            Timestamp: moment.utc().format('YYYY-MM-DDTHH:mm:ss')
    
        p = Object.keys(data).reduce(((a, k)->a ++ ["#{k}=#{encodeURIComponent(data[k])}"]), []).sort().join("&")
        meta = [method, baseurl, path, p].join('\n');
        hash = crypto.createHmac('sha256', ApiKey.huobi.SecretKey).update(meta).digest('base64')
        signature = hash
        data.Signature = signature

        pkg = {op: "auth"}
        for k, v of data
            pkg[k] = v
            
        pkg = {"op":"auth","AccessKeyId":"14516cdf-1ce835ac-d941009c-48bdf","SignatureMethod":"HmacSHA256","SignatureVersion":"2","Timestamp":"2018-11-09T20:22:20","Signature":"3wncWAYB1WkMPNpvfRbNukQrtKP9pbEVXuyEmDOY2mU="}
        sendData = JSON.stringify(pkg)

        console.log sendData
        huobiWs2.send(sendData, (err)->console.log(err))
        
    ..on 'close', ->
        console.log 'close'

    ..on 'message', (buf) ->
        data = 
            pako.inflate(buf, {to:'string'}) |> 
            JSON.parse _
        console.log data
        
    ..on 'error', (err) ->
        console.log err