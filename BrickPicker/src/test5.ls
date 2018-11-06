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

Low = (data)->
    data.map(([_, low, _, _])->low)

High = (data)->
    data.map(([_, _, _, _, high])->high)
        
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

mapn = (f, ...args)->
    maxLength = Math.min.apply(null, args.map (.length))
    for i in [0 til maxLength]
        f.apply(null, args.map((ary)->ary[i]))
            
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
    mapn (-), EMA(n, data), EMA(m, data)

MACD-DEM = EMA


BBI = (n, m, o, p, data)->
    l1 = MA n, data
    l2 = MA m, data
    l3 = MA o, data
    l4 = MA p, data
    mapn do
        (...args)->
            args.reduce((+), 0)/args.length
        l1, l2, l3, l4

EBBI = (n, m, o, p, data)->
    l1 = EMA n, data
    l2 = EMA m, data
    l3 = EMA o, data
    l4 = EMA p, data
    mapn do
        (...args)->
            args.reduce((+), 0)/args.length
        l1, l2, l3, l4
        
AccDist = (data)->
    ret = reductions do
        (+)
        0
        data.map ([_, low, open, close, high, volume])->
            if high == low
                0
            else
                ((close - low) - (high - close)) * volume / (high - low)
    # 去掉reductions中產生的初始值0
    ret.slice(1, ret.length)

Chaikin = (n, m, data)->
    acc = AccDist data
    mapn (-), EMA(n, acc), EMA(m, acc)

TrueLow = (data)->
    ret = mapn do
        Math.min
        Close data
        Low data.slice(1, data.length)
    (Low [data[0]]).concat ret 

TrueWave = (data)->
    ret = mapn do
        (close, high, low)->
            Math.max high - low, Math.abs(high - close), Math.abs(low - close)
        Close data
        High(data).slice(1, data.length)
        Low(data).slice(1, data.length)
    (High [data[0]]).concat ret
        
UOS = (m, n, o, data)->
    tl = TrueLow data
    bp = mapn (-), Close(data), tl
    tr = TrueWave data
    ruo = mapn do
        (b1, b2, b3, t1, t2, t3)->
            (b1 / t1)*4 + (b2 / t2)*2 + b3 / t3
        MA(m, bp).map (* m)
        MA(n, bp).map (* n)
        MA(o, bp).map (* o)
        MA(m, tr).map (* m)
        MA(n, tr).map (* n)
        MA(o, tr).map (* o)
    uos = ruo.map (n)-> n*(100/7)
        
checkSignal2 = (line1, line2, data)->
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

checkSignal = (line, buyLine, sellLine, data)->
    orders = []
    for i in [0 til line.length]
        prevL = line[i-1]
        prevB = buyLine[i-1]
        prevS = sellLine[i-1]
        
        l = line[i]
        b = buyLine[i]
        s = sellLine[i]
        
        if prevL <= prevB && l > b && i<line.length-1
            date = data[i][0]
            open = data[i][2]
            buyPrice = open
            orders.push do
                action:"buy",
                price:buyPrice,
                date:date
                
        if prevL >= prevS && l < s && i<line.length-1
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
        line25 = [25 for til stockData.length]
        line50 = [50 for til stockData.length]
        line75 = [75 for til stockData.length]
        line0 = [0 for til stockData.length]
        
        close = stockData |> Close
        ma5 = close |> MA 5, _
        ma10 = close |> MA 10, _
        [kdK, kdD] = stockData |> RSV 9, _ |> KD _
        ema5 = close |> EMA 2, _
        ema10 = close |> EMA 20, _
        dif = close |> MACD-DIF 12, 26, _
        dem = dif |> MACD-DEM 9, _
        bbi = close |> BBI 3, 6, 12, 24, _
        chaikin = stockData |> Chaikin 3, 10, _
        uos = stockData |> UOS 5, 10, 20, _
        
        checks = [
            ["ma", ma5, ma10, ma10]
            ["kd", kdK, kdD, kdD]
            ["macd", dif, dem, dem]
            ["bbi", close, bbi, bbi]
            ["chaikin", chaikin, line0, line0]
            ["uos", uos, line50, line50]
        ]
        
        for [name, l, l2, l3] in checks
            console.log name
            checkSignal l, l2, l3, stockData |>
            checkEarn |>
            console.log  _
            
        /*
        for [line1, line2] in [[ma5, ma10], [kd[0], kd[1]], [ema5, ema10], [dif, dem], [close, bbi]]
            checkSignal line1, line2, stockData |>
            checkEarn |>
            console.log
        */
        res.render do
            "kline2",
            data: JSON.stringify stockData
            close: JSON.stringify close
            ma5: JSON.stringify ma5
            ma10: JSON.stringify ma10
            kdK: JSON.stringify kdK
            kdD: JSON.stringify kdD
            ema5: JSON.stringify ema5
            ema10: JSON.stringify ema10
            macdDem: JSON.stringify dem
            macdDif: JSON.stringify dif
            bbi: JSON.stringify bbi
            chaikin: JSON.stringify chaikin
            uos: JSON.stringify uos
    catch e
        console.log e
        res.json 'error'
            
            
app.get '/', (req, res)->
    res.render do
        'index'
        title: "[['Mon', 20, 28, 38, 45]]"
        data: "[['Mon', 20, 28, 38, 45],['Tue', 31, 38, 55, 66],['Wed', 50, 55, 77, 80],['Thu', 77, 77, 66, 50],['Fri', 68, 66, 22, 15]]"

app.listen 8080