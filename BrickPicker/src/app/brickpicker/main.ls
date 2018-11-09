
require! {
    express
    path
    async
    fs
    "../../stock/formula": Formula
    "../../stock/tool": Tool
    "../../stock/earn": Earn
    ws: WebSocket
}


guess = (cb)->
    count = 240
    ma = "usdt"
    mb = "btc"
    range = "1h"
    earnRate = 100
    
    # console.log "https://api.binance.com/api/v1/klines?interval=#{range}&limit=#{count}&symbol="+mb.toUpperCase()+ma.toUpperCase()
    (err, data) <- Tool.fetch "https://api.binance.com/api/v1/klines?interval=#{range}&limit=#{count}&symbol="+mb.toUpperCase()+ma.toUpperCase(), true
    if err
        return cb err.error

    format = ([openTime, open, high, low, close])->
        [new Date(openTime).toString()] ++ ([low, open, close, high].map parseFloat)
        
    stockData = data |> JSON.parse _ |> Array.prototype.map.call _, format
    earnInfo = Earn.checkLowHighEarn(earnRate, stockData)
    earnInfo.style = Earn.checkStyle stockData
    cb null, earnInfo

(err, earnInfo) <- guess
if err
    return err

{txRate, {avg, sd}:price, tx, style, check} = earnInfo
console.log "style", style
console.log check
if tx.length == 0
    console.log 'no tx'
    return

console.log txRate, avg, sd

if txRate < 0.7
    console.log "txRate to low: #{txRate}"
    return 
    


/*
if txRate < 0.7
    return console.log "txRate < 0.7"
*/

root = 
    storage: []
    money: 0
    lastOrderTime: 0
    price: 999999

# 用這個查即時現價
tradeStream = new WebSocket 'wss://stream.binance.com:9443/ws/btcusdt@aggTrade'
    ..on 'open', ->
        console.log 'open'

    ..on 'close', ->
        console.log 'close'

    ..on 'message', (data) ->
        data = JSON.parse(data)
        {p} = data
        p = parseFloat p
        root.price = p
        
        total = root.money + root.storage.reduce((+), 0)
        currZ = (root.price - avg) / sd
        console.log "totalMoney: #{total}, currPrice: #{root.price}, z: #{currZ} len: #{root.storage.length} wait..."
        
        sellOk = false
        if root.storage.length > 0
            for i in [0 til root.storage.length].reverse()
                sp = root.storage[i]
                if (root.price - sp)/sp > 0.0001
                    root.money = root.money + root.price
                    #root.money -= p*0.001
                    sellOk = true
                    root.storage = root.storage.slice(0, i) ++ root.storage.slice(i+1, root.storage.length)
                    console.log "*******"
                    console.log "sell #{sp}"
                    console.log "money: #{root.money}"
                    console.log "storage.len #{root.storage.length}"
        
        if sellOk == false
            if (new Date().getTime() - root.lastOrderTime) < 60000
                console.log "wait until 1 minute"
                
            else if root.price > (avg - (sd)) && root.price <= (avg + (sd))
                root.storage.push root.price
                root.money = root.money - root.price
                root.lastOrderTime = new Date().getTime()
                #root.money -= p*0.001
                console.log "buy", root.price
                console.log "money: #{root.money}"
                console.log "storage.len #{root.storage.length}"
        
        
    ..on 'error', (err) ->
        console.log err
        
        
        /*
setInterval do
    ->
        total = root.money + root.storage.reduce((+), 0)
        currZ = (root.price - avg) / sd
        console.log "totalMoney: #{total}, currPrice: #{root.price}, z: #{currZ} wait..."
        
        sellOk = false
        if root.storage.length > 0
            for i in [0 til root.storage.length].reverse()
                sp = root.storage[i]
                if (root.price - sp)/sp > 0.0001
                    root.money = root.money + root.price
                    #root.money -= p*0.001
                    sellOk = true
                    root.storage = root.storage.slice(0, i) ++ root.storage.slice(i+1, root.storage.length)
                    console.log "*******"
                    console.log "sell #{sp}"
                    console.log "money: #{root.money}"
                    console.log "storage.len #{root.storage.length}"
        
        if sellOk == false
            if (new Date().getTime() - root.lastOrderTime) < 60000
                console.log "wait until 1 minute"
                
            else if root.price > (avg - (sd)) && root.price <= (avg + (sd))
                root.storage.push root.price
                root.money = root.money - root.price
                root.lastOrderTime = new Date().getTime()
                #root.money -= p*0.001
                console.log "buy", root.price
                console.log "money: #{root.money}"
                console.log "storage.len #{root.storage.length}"
    1000
        
        */