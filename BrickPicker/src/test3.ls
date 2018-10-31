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
    request(options, callback)

updatePrice = (cb) ->
    storage = {}
    
    (err, results) <- async.parallel [
        getUrl 'https://api.binance.com/api/v1/depth?symbol=BTCUSDT'
        getUrl 'https://api.huobipro.com/market/depth?symbol=btcusdt&type=step0'
        getUrl 'https://bittrex.com/api/v1.1/public/getorderbook?market=USDT-BTC&type=both'
        getUrl 'https://poloniex.com/public?command=returnOrderBook&currencyPair=USDT_BTC&depth=10'
    ]
    if err 
        return cb err
    [binanceDepth, huobiDepth, bittrexDepth, poloniexDepth] = results
    
    # binance
    {bids, asks} = binanceDepth
    # 買單為高到低排序
    bids = bids.map (ary)->
        ary.pop()
        ary.map parseFloat
    # 賣單為低到高排序
    asks = asks.map (ary)->
        ary.pop()
        ary.map parseFloat
    
    storage.binance = 
        bids : bids
        asks : asks
    
    # huobi
    storage.huobi = huobiDepth.tick
    
    # bittrex
    {success, message, {buy, sell}:result} = bittrexDepth
    if success
        storage.bittrex = 
            bids: buy.map ({Quantity, Rate})->[Rate, Quantity]
            asks: sell.map ({Quantity, Rate})->[Rate, Quantity]
    else
        console.log message
        
    # poloniex
    {bids, asks} = poloniexDepth
    bids = bids.map (ary)->
        ary.map parseFloat
    # 賣單為低到高排序
    asks = asks.map (ary)->
        ary.map parseFloat
    storage.poloniex = 
        bids : bids
        asks : asks
    
    # result
    cb null, storage

checkBuy = (storage, ma, mb, dir = '>')->
    # 賣一價。出價最低的賣單
    sell1a = storage[ma].asks[0]
    # 買一價。出價最高的買單
    # 買單的價一定小於賣單，不然就成交了
    buy1a = storage[ma].bids[0]
    
    sell1b = storage[mb].asks[0]
    buy1b = storage[mb].bids[0]
    
    # 如果是 sell1a > buy1a > sell1b > buy1b
    # 套利空間為 buy1a - sell1b
    
    order = [sell1a, buy1a, sell1b, buy1b]
    
    # 判斷是左搬到右，還是右搬到左
    buyCost = if dir == '>' then sell1b else sell1a
    sellEarn = if dir == '>' then buy1a else buy1b
    
    # 獲利空間
    space = sellEarn[0] - buyCost[0]
    # 最小最大量
    volumn = [sellEarn[1], buyCost[1]]
        ..sort!
    # 預估獲利[美元, 台幣]
    guess = space* volumn[0]
    
    return
        dir: "#{ma} #{dir} #{mb}"
        buyCost: buyCost[0]
        sellEarn: sellEarn[0]
        space: space
        volumn: volumn
        guess: guess

markets = ['binance', 'huobi', 'bittrex', 'poloniex']
    ..sort()

(err, storage) <- updatePrice

earn = 
    [[ma, mb] for ma in markets for mb in markets when ma != mb] |>
    (Array.prototype.map.call _, ([ma,mb])->[ma, mb, if ma > mb then '>' else '<']) |>
    (Array.prototype.map.call _, (args)->(checkBuy.apply null, [storage].concat(args))) |>
    (Array.prototype.filter.call _, (info)->info.space > 0) |>
    (Array.prototype.reduce.call _, (acc, info)->(acc+info.guess), 0)

console.log earn, earn* 30

/*
(err, storage) <- updatePrice

info = checkBuy storage, 'binance', 'huobi'
console.log info

info = checkBuy storage, 'binance', 'huobi', '<'
console.log info

info = checkBuy storage, 'binance', 'bittrex'
console.log info

info = checkBuy storage, 'binance', 'bittrex', '<'
console.log info

info = checkBuy storage, 'binance', 'poloniex'
console.log info

info = checkBuy storage, 'binance', 'poloniex', '<'
console.log info

info = checkBuy storage, 'huobi', 'bittrex'
console.log info

info = checkBuy storage, 'huobi', 'bittrex', '<'
console.log info

info = checkBuy storage, 'huobi', 'poloniex'
console.log info

info = checkBuy storage, 'huobi', 'poloniex', '<'
console.log info

info = checkBuy storage, 'bittrex', 'poloniex'
console.log info

info = checkBuy storage, 'bittrex', 'poloniex', '<'
console.log info
*/