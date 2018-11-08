WebSocket = require('ws')
pako = require('pako');
signalR = require ('signalr-client')
zlib = require('zlib')

/*
binanceWs = new WebSocket 'wss://stream.binance.com:9443/ws/btcusdt@depth5'
    ..on 'open', ->
        console.log 'open'

    ..on 'close', ->
        console.log 'close'
    
    ..on 'message', (data) ->
        console.log JSON.parse(data)
        
    ..on 'error', (err) ->
        console.log err
*/

/*
binanceWs = new WebSocket 'wss://stream.binance.com:9443/ws/btcusdt@kline_1m'
    ..on 'open', ->
        console.log 'open'

    ..on 'close', ->
        console.log 'close'

    ..on 'message', (data) ->
        console.log JSON.parse(data)

    ..on 'error', (err) ->
        console.log err
*/

# 用這個查即時現價
binanceWs = new WebSocket 'wss://stream.binance.com:9443/ws/btcusdt@aggTrade'
    ..on 'open', ->
        console.log 'open'

    ..on 'close', ->
        console.log 'close'

    ..on 'message', (data) ->
        console.log JSON.parse(data)

    ..on 'error', (err) ->
        console.log err
        
/*
huobiWs = new WebSocket 'wss://api.huobi.pro/ws'
    ..on 'open', ->
        console.log 'open'
        sendData = 
            {sub: 'market.btcusdt.depth.step0', id: 'xx20'} |>
            JSON.stringify _
        huobiWs.send(sendData, (err)->console.log(err))
        
    ..on 'close', ->
        console.log 'close'

    ..on 'message', (buf) ->
        data = 
            pako.inflate(buf, {to:'string'}) |> 
            JSON.parse _
        # console.log data
        
        if data.hasOwnProperty "ping"
            {ping} = data
            sendData = 
                {pong: ping} |>
                JSON.stringify _
            huobiWs.send(sendData, (err)->console.log(err))
        
        if data['ch'] == 'market.btcusdt.depth.step0'
            {tick:{bids, asks}} = data
            console.log bids
            
    ..on 'error', (err) ->
        console.log err
*/

/*
bittrexWs  = new signalR.client 'wss://socket.bittrex.com/signalr' ['c2']
    ..serviceHandlers.connected = (conn)->
        console.log 'connected'
        # 取得總表
        bittrexWs.call 'c2' 'QueryExchangeState' 'USDT-BTC'
            ..done (err, result)->
                if err?
                    return console.log err
                (err, inflated) <- zlib.inflateRaw new Buffer.from(result, 'base64')
                data = JSON.parse inflated.toString('utf8')
                console.log data
        
        # 定閱變更
        bittrexWs.call 'c2' 'SubscribeToExchangeDeltas' 'USDT-BTC'
            ..done (err, result)->
                if err?
                    return console.log err
        
    ..serviceHandlers.messageReceived = (msg)->
        # console.log msg
        data = JSON.parse msg.utf8Data
        # console.log data
        if data.M?[0]?.A?[0]?
            b64 = data.M[0].A[0]
            buf = new Buffer.from b64, 'base64'
            (err, inflated) <- zlib.inflateRaw buf
            if err
                console.log err
            data = JSON.parse inflated.toString ('utf8')
            console.log data
*/


/*
poloniexWs = new WebSocket 'wss://api2.poloniex.com'
    ..on 'open', ->
        console.log 'open'
        sendData = 
            {subscribe: 'subscribe', channel: 1000} |>
            JSON.stringify _, 2
        console.log sendData
        poloniexWs.send(sendData, (err)->console.log(err))
        
    ..on 'close', ->
        console.log 'close'

    ..on 'message', (data) ->
        console.log data
        
    ..on 'error', (err) ->
        console.log err
*/