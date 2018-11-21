
require! {
    express
    path
    async
    fs
    "../../stock/formula": Formula
    "../../stock/tool": Tool
    "../../stock/earn": Earn
    "init-config": Config
    "body-parser": bodyParser
    request
    "http-cache": HttpCache
}

# =========== helper =============#

writeFile = (fileName, content, cb)->
    fs.writeFile fileName, content, cb

readFile = (fileName, cb)->
    fs.readFile fileName, 'utf8', cb

mapn = (f, ...args)->
    maxLength = Math.min.apply(null, args.map (.length))
    for i in [0 til maxLength]
        f.apply(null, args.map((ary)->ary[i]))
        
# ========= model ========== #

formulaKey = (formula)->
    formula.map((.toString())).reduce((+), "")

saveUserData_ = ({saveDir}:cfg, userdata, cb)-->
    fileName = "#{saveDir}/userdata.json"
    writeFile fileName, JSON.stringify(userdata), cb

loadUserData_ = ({saveDir}:cfg, cb)-->
    fileName = "#{saveDir}/userdata.json"
    if fs.existsSync(fileName) == false
        return saveUserData do
            cb
            stockIds: []
            formulas: []
            earnRateSettings: []
    
    (err, data) <- readFile fileName
    if err
        return cb err
    try
        loadData = JSON.parse data
        cb null, loadData
    catch e
        cb e


# ========= control ========== #

startExpress = (cfg)->
    saveUserData = saveUserData_ cfg
    loadUserData = loadUserData_ cfg
    
    FSProvider = require("./fsprovider").FSProvider
    httpCache = new HttpCache do
        ttl: 60* 60* 24* 365* 5
        varyByParam: ["stockNo", "date"]
        provider: new FSProvider {cacheDir: cfg.cacheDir}
    httpCache4Hour = new HttpCache do
        ttl: 60* 60* 4
        varyByParam: ["stockNo", "date"]
        provider: new FSProvider {cacheDir: cfg.cacheDir}
    
    app = express()
    app.set 'port', 8080
    app.set 'views', path.join( __dirname, '/view')
    app.set 'view engine', 'vash'
    app.use bodyParser.urlencoded do
        extended: true
    
    app.use('/', express.static(path.join( __dirname, '/www')));
    
    fetchStockData = (stockId, years, months, cacheDir, cb)-->
        # origin host is http://www.twse.com.tw
        fns = [[y, m] for y in years for m in months]
            .map ([y, m])->
                Tool.fetch "http://localhost:8080/exchangeReport/STOCK_DAY?response=json&date=#{y}#{(m+'').padStart(2, '0')}01&stockNo=#{stockId}", null
        (err, results) <- async.series fns
        cb err, results
    
    # 代理
    app.get '/exchangeReport/STOCK_DAY', (req, res)->
        res.header do
          'Content-Type': 'application/json;charset=UTF-8'
        
        # 處理當月的網頁快取, 每次快取4個小時
        # 其餘的月份會自動快取於檔案中
        catchHandler = (req, res, cb)->
            now = new Date
            date = req.query.date
            year = parseInt date.slice(0, 4)
            month = parseInt date.slice(4, 6)
            if year == now.getFullYear() && month == now.getMonth()+1
                console.log "4hour"
                httpCache4Hour req, res, cb
            else
                console.log "5 year"
                httpCache req, res, cb
        
        <- catchHandler req, res
        
        stockNo = req.query.stockNo
        date = req.query.date
        url = "http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=#{date}&stockNo=#{stockNo}"
        (err, res_, body) <- request.get url
        if err
            console.log err
            return res.send body
        /*
        # 必須加上Content-Length
        # 不然chrome會報錯(ERR_INCOMPLETE_CHUNKED_ENCODING)
        res.header do
          'Content-Length': Buffer.from(body, 'utf-8').length
          "Cache-Control": "max-age=#{1000*60*60*4}"
        */
        res.send body

    app.get '/fn/userdata', (req, res)->
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        res.render "edit", {data: userdata}

    app.get '/fn/addFormula/kd/:arg1', (req, res)->
        arg1 = parseInt req.params.arg1
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        userdata.formulas.push ['kd', arg1]
        (err) <- saveUserData userdata
        res.json [err, userdata]
    
    app.get '/fn/addFormula/ma/:arg1/:arg2', (req, res)->
        arg1 = parseInt req.params.arg1
        arg2 = parseInt req.params.arg2
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        userdata.formulas.push ['ma', arg1, arg2]
        (err) <- saveUserData userdata
        res.json [err, userdata]

    app.get '/fn/addFormula/bbi/:arg1/:arg2/:arg3/:arg4', (req, res)->
        arg1 = parseInt req.params.arg1
        arg2 = parseInt req.params.arg2
        arg3 = parseInt req.params.arg3
        arg4 = parseInt req.params.arg4
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        userdata.formulas.push ['bbi', arg1, arg2, arg3, arg4]
        (err) <- saveUserData userdata
        res.json [err, userdata]
        
    app.get '/fn/removeFormula/:name', (req, res)->
        name = req.params.name
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        userdata.formulas = userdata.formulas.filter (f)->
            formulaKey(f) != name
        (err) <- saveUserData userdata
        res.json [err, userdata]
        
    app.post '/fn/addStockId', (req, res)->
        stockId = req.body.stockId
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        isExist = (userdata.stockIds.filter (id)->id == stockId) |>
            (.length) |>
            (> 0)
        if stockId.trim() == ""
            res.json "no stockId"
            return
        if not isExist
            userdata.stockIds.push stockId
        (err) <- saveUserData userdata
        res.redirect "/fn/userdata"
    
    app.post '/fn/removeStockId', (req, res)->
        stockId = req.body.stockId
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        userdata.stockIds = userdata.stockIds.filter (id)->id != stockId
        (err) <- saveUserData userdata
        res.redirect "/fn/userdata"
        
    app.get '/fn/dashboard', (req, res)->
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        
        year = new Date().getFullYear()
        fns = userdata.stockIds.map (stockId)-> fetchStockData stockId, [year], [1 to 12], null
        
        (err, results) <- async.parallel fns
        if err
            console.log err
            return res.json [err]
        
        results = for [stockId, data] in (mapn (...args)->args, userdata.stockIds, results)
            try
                stockData = data |> Tool.formatStockData
                styles = [120, 60, 20, 10, 5].map (cnt)->
                    minCnt = Math.min cnt, stockData.length
                    stockData.slice(stockData.length - minCnt, stockData.length) |>
                    Earn.checkStyle _
                {
                    stock: stockId
                    styles: styles
                    price: stockData[*-1]
                }
            catch e
                console.log e
                {
                    stock: stockId
                    msg: 'some thing wrong'
                }
        
        results.sort ({styles:a}, {styles:b})->
            return b[*-1] - a[*-1]
            
        res.render "dashboard", {data: results}
    
    app.get '/fn/earnRates', (req, res)->
        (err, userdata) <- loadUserData
        if err
            console.log err
            return res.json [err]

        fns = userdata.earnRateSettings.map ({stockId, year, count, earnRate}:setting)->
            (cb)-> async.waterfall do 
                [
                    fetchStockData stockId, [year], [1 to 12], null
                    (data, cb)->
                        stockData = data |> Tool.formatStockData
                        cnt = Math.min count, stockData.length
                        stockData = stockData.slice(stockData.length - cnt, stockData.length)
                        earnInfo = Earn.checkLowHighEarn(earnRate, stockData)
                        earnInfo.style = Earn.checkStyle stockData
                        earnInfo.setting = setting
                        cb null, earnInfo
                ] 
                cb
        
        (err, results) <- async.series fns
        if err
            console.log err
            return res.json [err]
        res.render "earnRate", {userdata: userdata, result: results}

    app.post '/fn/earnRates/add', (req, res)->
        stockId = req.body.stockId
        year = parseInt req.body.year
        count = parseInt req.body.count
        earnRate = parseFloat req.body.earnRate
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        
        userdata.earnRateSettings.push do
            stockId: stockId
            year: year
            count: count
            earnRate: earnRate
        
        (err) <- saveUserData userdata
        if err
            return res.json [err]
        res.redirect "/fn/earnRates"
    
    app.post '/fn/earnRates/smartAdd', (req, res)->
        stockId = req.body.stockId
        year = parseInt req.body.year
        minRate = parseFloat req.body.minRate
        
        (err, userdata) <- loadUserData
        if err
            console.log err
            return res.json [err]
    
        (err, data) <- fetchStockData stockId, [year], [1 to 12], null
        if err
            console.log err
            return res.json [err]
            
        stockData = data |> Tool.formatStockData
        cnt = Math.min count, stockData.length
        
        counts = [5, 10, 20, 60]
        earnRates = [0.002, 0.005, 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.8, 0.9, 1, 1.2, 1.4, 1.6, 1.8, 2, 2.5, 3, 4]
        
        compute = ([count, earnRate]:setting)->
            cnt = Math.min count, stockData.length
            tmpData = stockData.slice(stockData.length - cnt, stockData.length)
            earnInfo = Earn.checkLowHighEarn(earnRate, tmpData)
            [setting, earnInfo.txRate + earnRate*100, earnInfo.txRate]
        
        pairs = [[count, earnRate] for count in counts for earnRate in earnRates]
        results = pairs.map(compute)

        filterMinEarnRate = results.filter(([_, _, earnRate])->earnRate >= minRate).sort(([_, a], [_, b])-> b - a)
        if filterMinEarnRate.length > 0
            console.log filterMinEarnRate
            console.log "filterMin"
            console.log filterMinEarnRate[0]
            [[count, earnRate]] = filterMinEarnRate[0]
            userdata.earnRateSettings.push do
                stockId: stockId
                year: year
                count: count
                earnRate: earnRate
        else
            filterNormal = results.sort(([_, a], [_, b])-> b - a)
            [[count, earnRate]] = filterNormal[0]
            userdata.earnRateSettings.push do
                stockId: stockId
                year: year
                count: count
                earnRate: earnRate
            
        (err) <- saveUserData userdata
        if err
            console.log err
            return res.json [err]
            
        res.redirect "/fn/earnRates"

    app.post '/fn/earnRates/remove', (req, res)->
        stockId = req.body.stockId
        year = parseInt req.body.year
        count = parseInt req.body.count
        earnRate = parseFloat req.body.earnRate
        
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        
        userdata.earnRateSettings = userdata.earnRateSettings.filter (info)->
            if not info
                return false
            return 
                info.stockId != stockId ||
                info.year != year ||
                info.count != count ||
                info.earnRate != earnRate
        
        (err) <- saveUserData userdata
        if err
            return res.json [err]
        res.redirect "/fn/earnRates"
        
    
    app.post '/fn/orders/add', (req, res)->
        stockId = req.body.stockId
        year = parseInt req.body.year
        count = parseInt req.body.count
        earnRate = parseFloat req.body.earnRate
        key = req.body.key
        
        order = 
            stockId: stockId
            year: year
            count: count
            earnRate: earnRate
            key: key
            
        (err, userdata) <- loadUserData
        if err
            console.log err
            return res.json [err]
            
        userdata.orders.push order
        
        (err) <- saveUserData userdata
        if err
            console.log err
            return res.json [err]
        res.redirect "/fn/orders"
    
    app.get '/fn/orders', (req, res)->
        (err, userdata) <- loadUserData
        if err
            console.log err
            return res.json [err]
            
        fns = userdata.orders.map ({stockId, year, count, earnRate, key}:setting)->
            (cb)-> async.waterfall do 
                [
                    fetchStockData stockId, [year], [1 to 12], null
                    (data, cb)->
                        stockData = data |> Tool.formatStockData
                        idx = 0
                        for i in [0 til stockData.length]
                            idx = i
                            if stockData[i][0] == key
                                break
                        if idx == stockData.length
                            idx = stockData.length - 1
                        start = Math.max 0, (idx+1)-count
                        stockData = stockData.slice(start, idx+1)
                        earnInfo = Earn.checkLowHighEarn(earnRate, stockData)
                        earnInfo.style = Earn.checkStyle stockData
                        earnInfo.setting = setting
                        cb null, earnInfo
                ] 
                cb
        
        (err, results) <- async.series fns
        if err
            return res.json [err]
        res.render "earnRate", {userdata: userdata, result: results}
    
    app.get '/fn/compute/:year', (req, res)->
        year = req.params.year
    
        computeOne = (userdata, stockId, cb)-->
            (err, data) <- fetchStockData stockId, [year], [1 to 12], null
            if err
                return cb err
            try
                stockData = data |> Tool.formatStockData
                close = stockData |> Formula.Close
                style = Earn.checkStyle stockData
                results = userdata.formulas.map ([name, arg1, arg2, arg3, arg4]:formula)->
                    switch name
                        | "kd" =>
                            [kdK, kdD] = stockData |> Formula.RSV arg1, _ |> Formula.KD _
                            signals = Earn.checkSignal kdK, kdD, kdD, stockData
                            earn = Earn.checkEarn stockData, signals
                            [formula, earn, signals]
                        | "bbi" =>
                            bbi = close |> Formula.BBI arg1, arg2, arg3, arg4, _
                            signals = Earn.checkSignal close, bbi, bbi, stockData
                            earn = Earn.checkEarn stockData, signals
                            [formula, earn, signals]
                        | "ma" =>
                            ma1 = close |> Formula.MA arg1, _
                            ma2 = close |> Formula.MA arg2, _
                            signals = Earn.checkSignal ma1, ma2, ma2, stockData
                            earn = Earn.checkEarn stockData, signals
                            [formula, earn, signals]
                        | otherwise =>
                cb null, {stockId: stockId, year: year, style: style, kline: stockData, results: results}
            catch e
                console.log e
                cb e.error
                
        (err, userdata) <- loadUserData
        if err
            console.log err
            return res.json [err]
        
        console.log userdata
        fns = userdata.stockIds.map (stockId)->computeOne userdata, stockId
        
        (err, data) <- async.series fns
        if err
            console.log err
            return res.json [err]
        
        # res.json [null, data]
        res.render do
            "stock"
            data: JSON.stringify(data)
    
    app.get '/fn/test/:stockId/:year/:count/:earnRate/:add', (req, res)->
        stockId = req.params.stockId
        year = req.params.year
        count = parseInt req.params.count
        earnRate = parseInt req.params.earnRate
        add = parseInt req.params.add

        (err, data) <- fetchStockData stockId, [year], [1 to 12], null
        if err
            return res.json [err]
        stockData = data |> Tool.formatStockData
        
        # limit to count form last
        cnt = Math.min count, stockData.length
        stockData = stockData.slice(stockData.length - cnt, stockData.length)
        
        earnInfo = Earn.checkLowHighEarn(earnRate/10000, stockData)
        earnInfo.style = Earn.checkStyle stockData
        
        if add
            order = 
                stockId: stockId
                year: year
                count: count
                earnRate: earnRate/10000
                key: stockData[*-1][0]
            
            (err, userdata) <- loadUserData
            if err
                console.log err
                return res.json [err]
            
            userdata.orders.push order
        
            (err) <- saveUserData userdata
            if err
                console.log err
                return res.json [err]
        
        
        res.json [null, earnInfo]
        
    app.get '/fn/block/:ma/:mb/:range/:count/:earnRate', (req, res)->
        count = req.params.count
        ma = req.params.ma
        mb = req.params.mb
        range = req.params.range
        earnRate = parseInt req.params.earnRate
        
        (err, data) <- Tool.fetch "https://api.binance.com/api/v1/klines?interval=#{range}&limit=#{count}&symbol="+mb.toUpperCase()+ma.toUpperCase(), cfg.cacheDir
        if err
            return res.json [err.error]
        
        format = ([openTime, open, high, low, close])->
            [new Date(openTime).toString()] ++ ([low, open, close, high].map parseFloat)
            
        stockData = data |> JSON.parse _ |> Array.prototype.map.call _, format
        earnInfo = Earn.checkLowHighEarn(earnRate, stockData)
        earnInfo.style = Earn.checkStyle stockData
        res.json [null, earnInfo]
        
    app.listen 8080


loadConfig = ->
    defaults = Config.Defaults do
      configPath: Config.Value('./config.json', Config.CLI(['configPath', 'cp'], 'path to load config'))
    config1 = Config.init(defaults)
    try
        config2 = Config.init(config1.configPath)
        return [null, config2]
    catch e
        console.log defaults.toTerminal()
        return [e]

startApp = ->
    [err, config] = loadConfig()
    if err 
        return console.log err
    console.log config
    startExpress config
    console.log 'startApp'

startApp()