express = require('express')
path = require('path')
request = require('request')
async = require('async')

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
    
    (err, results) <- async.series do
        [1 to cnt].map (m)->getUrl "http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=2017#{(m+'').padStart(2, '0')}01&stockNo=#{stockId}"
        
    
    # (err, {data}) <- getUrl "http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=20170401&stockNo=#{stockId}"
    if err
        res.json err
    else
        data = results.reduce ((acc, {data})->acc ++ data), []
        
        format = ([openTime, _, _2, open, high, low, close])->
            [new Date(openTime).toString()] ++ ([low, open, close, high].map parseFloat)
            
        data = data |>
            (Array.prototype.map.call _, format) |>
            JSON.stringify _
            
        res.render do
            "kline",
            data: data
            

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