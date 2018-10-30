request = require('request')
async = require('async')

# --> : use for curry
getUrl = (url, cb) -->
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
    request(options, callback);
    


(err, {bids, asks}) <- getUrl 'https://api.binance.com/api/v1/depth?symbol=BTCUSDT'
console.log(bids)
sell1 = parseFloat(asks[0][0])


(err, {tick:{bids, asks}}) <- getUrl 'https://api.huobipro.com/market/depth?symbol=btcusdt&type=step0'
console.log(bids)
buy1 = parseFloat(bids[0][0])

console.log(buy1, sell1)


sellPrice = buy1
buyPrice = sell1

if sellPrice > buyPrice
    console.log 'you can move!'
else
    console.log 'can not move!'

/*
(err, body) <- getUrl 'https://api.binance.com/api/v3/ticker/bookTicker?symbol=BTCUSDT'
console.log(body)

(err, body) <- getUrl 'https://api.huobipro.com/market/detail?symbol=btcusdt'
console.log(body)

(err, body) <- getUrl 'https://api.huobipro.com/market/depth?symbol=btcusdt&type=step1'
console.log(body)
*/

updatePrice = (cb) ->
    (err, results) <- async.parallel [
        getUrl 'https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT'
        getUrl 'https://api.binance.com/api/v3/ticker/bookTicker?symbol=BTCUSDT'
        getUrl 'https://api.huobipro.com/market/detail?symbol=btcusdt'
        getUrl 'https://api.huobipro.com/market/depth?symbol=btcusdt&type=step1'
    ]
    [binanceNow, binanceBid, huobiNow, huobiBid] = results
    console.log binanceNow.price
    console.log binanceBid.bidPrice
    console.log huobiNow.tick.close
    console.log huobiBid.tick.bids[0][0]

# <- updatePrice