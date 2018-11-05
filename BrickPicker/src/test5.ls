path = require('path')
request = require('request')
async = require('async')
crypto = require('crypto')
fs = require('fs')

file = "./test.db"


/*
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

testFetch = (stockId, cnt)->
    (err, results) <- async.series do
        [1 to cnt].map (m)-> fetch "http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=2017#{(m+'').padStart(2, '0')}01&stockNo=#{stockId}", false
    if err
        console.log err
    else
        console.log results

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
        .reduce ((acc, {data})->acc ++ data), []

    format = ([openTime, _, _, open, high, low, close, _, volumn])->
        [new Date(openTime).toString()] ++ ([low, open, close, high, volumn].map parseFloat)
    
    return data.map format

Close = (data)->
    data.map(([_, _, _, close])->close)

MA = (data, cnt)->
    for i in [cnt-1 til data.length]
        avg = data.slice(i-(cnt-1), i+1)
            .reduce(((acc, curr)->acc+curr), 0)/cnt
            

RSV = (data, cnt)->
    ret = 
        for i in [cnt-1 til data.length]
            [openTime, low, open, close, high] = data[i]
            before9k = data.slice i-(cnt-1), i+1
            min9k = Math.min.apply null, before9k.map(([_, low])->low)
            max9k = Math.max.apply null, before9k.map(([_, _, _, _, high])->high)
            rsv = (close - min9k)*100/(max9k - min9k)
    [0 for til (cnt-1)].concat ret


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


(err, data) <- fetchStockData 2330, [2018], [1]
if err
    return console.log err

data = data |> formatStockData _ |> RSV _, 9 |> KD
console.log data


