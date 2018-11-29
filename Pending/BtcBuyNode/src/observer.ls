require! {
    websocket
}


export observe = ({observeRate, observeSeconds, observeVolumeRate}:cfg, symbol, foundcb)->
    
    local = 
        history: [],
        lastAvgV: 999999999
    
    pushHistory = ({date:now}:trade)->
        local.history.push trade
    
    check = ->
        if local.history.length == 0
            return [0, 0]
        
        # 取出最後20秒以內的交易
        {T:now} = local.history[*-1]
        tmp = local.history.filter ({T:curr})->
            (now - curr) < (observeSeconds * 1000)
        
        # 計算平均交易量
        avgV = tmp.map(({q}) -> parseFloat(q)).reduce((+), 0) / tmp.length
        console.log avgV
        avgVRate = (avgV - local.lastAvgV) / local.lastAvgV
        # 平均交易量的上升率
        local.lastAvgV = avgV
        
        # 計算價格在20秒內最高上升率
        prices = tmp.map ({p})-> parseFloat(p)
        min = prices |> Math.min.apply null, _
        max = prices |> Math.max.apply null, _
        rate = (max - min) / min
        
        local.history = tmp
        [rate, avgVRate]

    client = new websocket.client()

    client.on 'connectFailed', (error)->
        console.log('Connect Error: ' + error.toString())

    client.on 'connect', (connection)->
        console.log('WebSocket Client Connected')
    
        connection.on 'error', (error)->
            console.log "Connection Error: " + error.toString()

        connection.on 'close', ->
            console.log 'echo-protocol Connection Closed'

        connection.on 'message', (message)->
            if message.type == 'utf8'
                data = message.utf8Data |> JSON.parse _
                console.log data
                pushHistory data
                
                [rate, avgVRate]:result = check()
                console.log rate, avgVRate
                if rate > observeRate && avgVRate > observeVolumeRate
                    foundcb do
                        history: local.history
                        result: result
    
    client.connect("wss://stream.binance.com:9443/ws/#{symbol}@trade")
    
    /*
    client2 = new websocket.client()

    client2.on 'connectFailed', (error)->
        console.log('Connect Error: ' + error.toString())

    client2.on 'connect', (connection)->
        console.log('WebSocket Client Connected')
    
        connection.on 'error', (error)->
            console.log "Connection Error: " + error.toString()

        connection.on 'close', ->
            console.log 'echo-protocol Connection Closed'

        connection.on 'message', (message)->
            if message.type == 'utf8'
                data = message.utf8Data |> JSON.parse _
                console.log data
    */
    #client2.connect("wss://stream.binance.com:9443/ws/#{symbol}@miniTicker")