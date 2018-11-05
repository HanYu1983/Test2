express = require('express')
path = require('path')
request = require('request')
async = require('async')
crypto = require('crypto')
fs = require('fs')

/*
file = "./test.db"
sqlite3 = require("sqlite3").verbose()
db = new sqlite3.Database file

db.serialize ->
    db.run "CREATE TABLE IF NOT EXISTS  Stuff (thing TEXT)"
    stmt = db.prepare "INSERT INTO Stuff VALUES (?)"

    for _, i in [0 to 10]
        stmt.run "staff_number#{i}"
    
    stmt.finalize()
    db.each "SELECT rowid AS id, thing FROM Stuff", (err, row)->
        console.log row.id + ": " + row.thing

db.close()

*/


fetch = (url, dontUseCache, cb) -->
    urlKey = crypto.createHash('md5').update(url).digest('hex')
    path = "cache/"+urlKey+".html"
    
    if (!!dontUseCache) == false && fs.existsSync path
        fs.readFile path, 'utf8', cb
    else
        ws = fs.createWriteStream path
            .on('error', cb)
            .on('finish', ->
                if (fs.existsSync path) == false
                    cb 'save lost'
                else
                    fs.readFile path, 'utf8', cb
            )
        request
            .get(url)
            .on('error', cb)
            .pipe ws

fetchStockData = (stockId, years, months, cb)->
    urls = [[y, m] for y in years for m in months] |>
        Array.prototype.map.call _, ([y, m])->"http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=#{y}#{(m+'').padStart(2, '0')}01&stockNo=#{stockId}"

    (err, results) <- async.series do
        urls.map (url)-> fetch url, false
    
    cb err, results

formatStockData = (data) ->
    data = data
        .filter (r)->r.trim() != ""
        .map JSON.parse
        .filter ({stat})-> stat=="OK"
        .reduce ((acc, {data})->acc ++ data), []

    format = ([openTime, _, _, open, high, low, close, _, volumn])->
        [new Date(openTime).toString()] ++ ([low, open, close, high, volumn].map parseFloat)
    
    return data.map format

Close = (data)->
    data.map(([_, _, _, close])->close)


MA = (cnt, data)->
    ret = 
        for i in [cnt-1 til data.length]
            avg = data.slice(i-(cnt-1), i+1)
                .reduce(((acc, curr)->acc+curr), 0)/cnt
    [ret[0] for til (cnt-1)].concat ret

RSV = (cnt, data)->
    ret = 
        for i in [cnt-1 til data.length]
            [openTime, low, open, close, high] = data[i]
            before9k = data.slice i-(cnt-1), i+1
            min9k = Math.min.apply null, before9k.map(([_, low])->low)
            max9k = Math.max.apply null, before9k.map(([_, _, _, _, high])->high)
            rsv = (close - min9k)*100/(max9k - min9k)
    [ret[0] for til (cnt-1)].concat ret

KD = (data)->
    kline = []
    dline = []
    for i in [0 til data.length]
        rsv = data[i]
        prevK = if i > 0 then kline[i-1] else 50
        prevD = if i > 0 then dline[i-1] else 50
        k = prevK * (2/3) + rsv/3
        d = prevD * (2/3) + k/3
        kline.push(k)
        dline.push(d)
    [kline, dline]

reductions = (f, i, seq)->
    seq.reduce do
        (acc, v)->
            prev = acc[*-1]
            curr = f(prev, v)
            acc ++ [curr]
        [i]

map2 = (f, vs1, vs2)->
    maxLength = Math.max(vs1.length, vs2.length)
    for i in [0 til maxLength]
        f do
            if i < vs1.length then vs1[i] else vs1[*-1]
            if i < vs2.length then vs2[i] else vs2[*-1]
            
YuMA = (n, data)->
    if data.length >= n
        fv = data.slice(0, n).reduce((+), 0)/n
        ret = reductions do
            (ma, v)->
                ma*((n-1)/n) + v/n
            fv
            data.slice(n, data.length)
        [ret[0] for til (n - 1)].concat ret

EMA = (n, data)->
    if data.length >= n
        fv = data.slice(0, n).reduce((+), 0)/n
        alpha = 2/(n+1)
        ret = reductions do
            (ema, v)->
                (v - ema)*alpha + ema
            fv
            data.slice(n, data.length)
        [ret[0] for til (n - 1)].concat ret

MACD-DIF = (n, m, data)->
    map2 (-), EMA(n, data), EMA(m, data)

MACD-DEM = EMA

checkSignal = (line1, line2, data)->
    orders = []
    for i in [0 til line1.length]
        prevK = line1[i-1]
        prevD = line2[i-1]
        k = line1[i]
        d = line2[i]
        if prevK <= prevD && k > d && i<line1.length-1
            date = data[i][0]
            open = data[i][2]
            buyPrice = open
            orders.push do
                action:"buy",
                price:buyPrice,
                date:date
        if prevK >= prevD && k < d && i<line1.length-1
            date = data[i][0]
            open = data[i][2]
            buyPrice = open
            orders.push do
                action:"sell",
                price:buyPrice,
                date:date
    orders


checkEarn = (orders)->
    storage = 0
    money = 0
    useMoney = 0
    rate = []
    gas = 0.001425
    for order in orders
        if order.action == "buy"
            if storage != 0
                console.log "has storage"
            else
                price = order.price
                cost = price + price * gas
                money -= cost
                useMoney = cost
                storage = price
                
        if order.action == "sell"
            if storage == 0
                console.log "no storage"
            else
                price = order.price
                earn = price - price * gas
                money += earn
                earnRate = ((earn - useMoney)/useMoney)
                storage = 0
                rate.push earnRate
                
    earnRateAvg = rate.reduce(((a,b)->a+b), 0)/rate.length
    transactionTime = rate.length
    useMoneyPerTranaction = 100000
    totalEarn = (useMoneyPerTranaction*earnRateAvg)*transactionTime
    totalEarnRate = (totalEarn + useMoneyPerTranaction)/useMoneyPerTranaction
    
    return
        price: storage
        amount: if storage != 0 then useMoneyPerTranaction/storage else 0
        moneyFlow: money + storage
        ratePerTx: earnRateAvg
        earn: totalEarn
        earnRate: totalEarnRate
        times: rate.length

/*
(err, data) <- fetchStockData 2475, [2017], [1 to 3]
if err
    return console.log err

stockData = data |> formatStockData
close = stockData |> Close

kd = stockData |> RSV 9, _ |> KD
orders = checkSignal kd[0], kd[1], stockData
result = orders |> checkEarn
#console.log orders
console.log result

orders = checkSignal (close |> MA 5, _), (close |> MA 10, _), stockData
result = orders |> checkEarn
#console.log orders
console.log result

orders = checkSignal (close |> MA 2, _), (close |> MA 20, _), stockData
result = orders |> checkEarn
#console.log orders
console.log result


dif = MACD-DIF(12, 26, close.slice(0, -1).reverse())
dem = MACD-DEM(9, dif)

dif = dif.concat([0 for til close.length - dif.length]).reverse()
dem = dem.concat([0 for til close.length - dem.length]).reverse()

orders = checkSignal dem, dif, stockData
result = orders |> checkEarn
#console.log orders
console.log result
*/


app = express()

app.set 'port', 8080
app.set 'views', path.join( __dirname, '/views')
app.set 'view engine', 'vash'

app.get '/view/stock/:year/:cnt/:stockId', (req, res)->
    stockId = req.params.stockId
    year = req.params.year
    cnt = parseInt(req.params.cnt)
    
    (err, data) <- fetchStockData stockId, [year], [1 to cnt]
    if err
        return res.json err
        
    try
        stockData = data |> formatStockData
        close = stockData |> Close
        ma5 = close |> MA 5, _
        ma10 = close |> MA 10, _
        kd = stockData |> RSV 9, _ |> KD _
        ema5 = close |> EMA 5, _
        ema10 = close |> EMA 10, _
        dif = close |> MACD-DIF 12, 26, _
        dem = dif |> MACD-DEM 9, _
        
        for [line1, line2] in [[ma5, ma10], [kd[0], kd[1]], [ema5, ema10], [dif, dem]]
            checkSignal line1, line2, stockData |>
            checkEarn |>
            console.log
        
        res.render do
            "kline2",
            data: JSON.stringify stockData
            close: JSON.stringify close
            ma5: JSON.stringify ma5
            ma10: JSON.stringify ma10
            kdK: JSON.stringify kd[0]
            kdD: JSON.stringify kd[1]
            ema5: JSON.stringify ema5
            ema10: JSON.stringify ema10
            macdDem: JSON.stringify dem
            macdDif: JSON.stringify dif
    catch e
        console.log e
        res.json 'error'
            
            
app.get '/', (req, res)->
    res.render do
        'index'
        title: "[['Mon', 20, 28, 38, 45]]"
        data: "[['Mon', 20, 28, 38, 45],['Tue', 31, 38, 55, 66],['Wed', 50, 55, 77, 80],['Thu', 77, 77, 66, 50],['Fri', 68, 66, 22, 15]]"

app.listen 8080