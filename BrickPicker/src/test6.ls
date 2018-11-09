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
    
    websocket: {client: WebSocketClient}
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


baseurl = "api.huobi.com"
path = "/ws/v1"
url = "wss://#{baseurl}#{path}"
console.log url

callbackPool = {}
callbackSeq = 0

sendAuth = (connection, cb)->
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

    pkg = {op: "auth", cid:callbackSeq++}
    for k, v of data
        pkg[k] = v
    
    sendData = JSON.stringify(pkg)
    connection.sendUTF(sendData)
    callbackPool[pkg.cid] = cb


sendPkg = (connection, pkg, cb)->
    if pkg.op != "sub"
        pkg.cid = callbackSeq++
        callbackPool[pkg.cid] = cb
    sendData = JSON.stringify(pkg)
    connection.sendUTF(sendData)
    

client = new WebSocketClient()

client.on 'connectFailed', (error)->
    console.log('Connect Error: ' + error.toString())

client.on 'connect', (connection)->
    console.log('WebSocket Client Connected')
    
    connection.on 'error', (error)->
        console.log "Connection Error: " + error.toString()

    connection.on 'close', ->
        console.log 'echo-protocol Connection Closed'

    connection.on 'message', (message)->
        if message.type === 'binary'
            res = 
                pako.inflate(message.binaryData, {to:'string'}) |> 
                JSON.parse _
            console.log res
            {op, cid, data} = res
            if op != "sub" && callbackPool.hasOwnProperty(cid)
                callbackPool[cid](res)
                delete callbackPool[cid]

    data <- sendAuth connection
    console.log data
    sendPkg connection, {op: "sub", topic: "accounts"}
    sendPkg connection, {op: "sub", topic: "orders.btcusdt"}
    
client.connect(url);