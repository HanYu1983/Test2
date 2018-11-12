
require! {
    express
    path
    async
    fs
    "../../stock/formula": Formula
    "../../stock/tool": Tool
    "../../stock/earn": Earn
    "init-config": Config
}

# =========== helper =============#

writeFile = (fileName, content, cb)->
    fs.writeFile fileName, content, cb

readFile = (fileName, cb)->
    fs.readFile fileName, 'utf8', cb
    
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
    
    app = express()
    app.set 'port', 8080
    app.set 'views', path.join( __dirname, '/views')
    app.set 'view engine', 'vash'

    app.get '/fn/userdata', (req, res)->
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        res.json [null, userdata]

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
        
    app.get '/fn/addStockId/:stockId', (req, res)->
        stockId = req.params.stockId
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        userdata.stockIds[stockId] = 0
        (err) <- saveUserData userdata
        res.json [err, userdata]
    
    app.get '/fn/removeStockId/:stockId', (req, res)->
        stockId = req.params.stockId
        (err, userdata) <- loadUserData
        if err
            return res.json [err]
        delete userdata.stockIds[stockId]
        (err) <- saveUserData userdata
        res.json [err, userdata]

    app.get '/fn/compute/:year', (req, res)->
        year = req.params.year
    
        computeOne = (userdata, stockId, cb)-->
            (err, data) <- Tool.fetchStockData stockId, [year], [1 to 2], cfg.cacheDir
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
                cb null, {stockId: stockId, year: year, style: style, results: results}
            catch e
                console.log e
                cb e.error
                
        (err, userdata) <- loadUserData
        if err
            console.log err
            return res.json [err]
        
        console.log userdata
        fns = for stockId, _ of userdata.stockIds
            computeOne userdata, stockId
        
        (err, data) <- async.series fns
        if err
            console.log err
            return res.json [err]
        
        res.json [null, data]
    
    app.get '/fn/test/:stockId/:year/:count/:earnRate', (req, res)->
        stockId = req.params.stockId
        year = req.params.year
        count = parseInt req.params.count
        earnRate = parseInt req.params.earnRate

        (err, data) <- Tool.fetchStockData stockId, [year], [1 to 12], cfg.cacheDir
        if err
            return res.json [err]
        stockData = data |> Tool.formatStockData
        
        # limit to count form last
        cnt = Math.min count, stockData.length
        stockData = stockData.slice(stockData.length - cnt, stockData.length)
        
        earnInfo = Earn.checkLowHighEarn(earnRate, stockData)
        earnInfo.style = Earn.checkStyle stockData
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