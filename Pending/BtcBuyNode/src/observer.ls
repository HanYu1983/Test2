require! {
    websocket
}


export observe = ({observeRate, observeSeconds}:cfg, symbol, foundcb)->
    
    local = 
        history: []
    
    pushHistory = ({date:now}:trade)->
        local.history.push trade
    
    check = ->
        if local.history.length == 0
            return 0
            
        {date:now} = local.history[*-1]
        tmp = local.history.filter ({date:curr})->
            (now - curr) < (observeSeconds * 1000)
        prices = tmp.map ({price})->price
        min = prices |> Math.min.apply null, _
        max = prices |> Math.max.apply null, _
        rate = (max - min) / min
        local.history = tmp
        rate

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
                {p:price, T:date} = data
                pushHistory do
                    price: price
                    date: date
                
                rate = check()
                if rate > observeRate
                    foundcb do
                        history: local.history
                        rate: rate
    
    client.connect("wss://stream.binance.com:9443/ws/#{symbol}@trade")
    
    
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
    
    #client2.connect("wss://stream.binance.com:9443/ws/#{symbol}@miniTicker")