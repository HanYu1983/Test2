WebSocket = require('ws')
pako = require('pako');
/*
binanceWs = new WebSocket 'wss://stream.binance.com:9443/ws/btcusdt@depth10'
    ..on 'open', ->
        console.log 'open'

    ..on 'close', ->
        console.log 'close'
    
    ..on 'message', (data) ->
        console.log JSON.parse(data)
        
    ..on 'error', (err) ->
        console.log err

*/

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
        
        if !!data["ping"]
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
