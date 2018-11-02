express = require('express')
path = require('path')
request = require('request')
async = require('async')
crypto = require('crypto')
fs = require('fs')

# --> : use for curry
fetch = (url, dontUseCache)->
    new Promise (res, rej)->
        urlKey = crypto.createHash('md5').update(url).digest('hex')
        path = "cache/"+urlKey+".html"
        if (!!dontUseCache) == false
            if fs.existsSync path
                fs.readFile path, 'utf8', (err, info)->if err then rej(err) else res(info)
            else
                request
                    .get(url)
                    .on('error', (err) -> rej err)
                    .on('end', ->
                        if (fs.existsSync path) == false
                            rej 'save lost'
                        else
                            fs.readFile path, 'utf8', (err, info)->if err then rej(err) else res(info)
                    )
                    .pipe(fs.createWriteStream path)

# --> : use for curry
getUrl = (url, cb) -->
    console.log url
    options =
        url: url
        method: 'GET'
        headers:
            'User-Agent': 'request'
    callback = (error, response, body) ->
        if error
            cb error
        else
            cb null, JSON.parse(body)
    request(options, callback)
    
app = express()

app.set 'port', 8080
app.set 'views', path.join( __dirname, '/views')
app.set 'view engine', 'vash'

app.get '/view/stock/:cnt/:stockId', (req, res)->
    stockId = req.params.stockId
    cnt = parseInt(req.params.cnt)
    
    callPromise = (url, cb) -->
        (fetch url, false).catch((err)->cb err).then((res)->cb null, res)
    
    (err, results) <- async.series do
        [1 to cnt].map (m)-> callPromise "http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=2017#{(m+'').padStart(2, '0')}01&stockNo=#{stockId}"
        
    if err
        res.json err
    else
        try
            data = results
                .filter (r)->r.trim() != ""
                .map JSON.parse
                .reduce ((acc, {data})->acc ++ data), []
        
            format = ([openTime, _, _2, open, high, low, close])->
                [new Date(openTime).toString()] ++ ([low, open, close, high].map parseFloat)
            
            data = data |>
                (Array.prototype.map.call _, format) |>
                JSON.stringify _
            
            res.render do
                "kline",
                data: data
        catch e
            res.json results
            

app.get '/view/kline/:ma/:mb/:range/:count', (req, res)->
    count = req.params.count
    ma = req.params.ma
    mb = req.params.mb
    range = req.params.range
    (err, data) <- getUrl "https://api.binance.com/api/v1/klines?interval=#{range}&limit=#{count}&symbol="+mb.toUpperCase()+ma.toUpperCase()
    if err
        res.render 'error'
    else
        format = ([openTime, open, high, low, close])->
            [new Date(openTime).toString()] ++ ([low, open, close, high].map parseFloat)
            
        data = data |>
            (Array.prototype.map.call _, format) |>
            JSON.stringify _
            
        res.render do
            "kline",
            data: data
            
app.get '/', (req, res)->
    res.render do
        'index'
        title: "[['Mon', 20, 28, 38, 45]]"
        data: "[['Mon', 20, 28, 38, 45],['Tue', 31, 38, 55, 66],['Wed', 50, 55, 77, 80],['Thu', 77, 77, 66, 50],['Fri', 68, 66, 22, 15]]"

app.listen 8080