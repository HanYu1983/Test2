
require! {
    express
    path
    async
    fs
    "../../stock/formula": Formula
    "../../stock/tool": Tool
    "../../stock/earn": Earn
}

# ========= model ========== #

userdata = 
    stockIds: {
        "2331":0,
        "2332":0
    },
    formulas: [
        ["kd", 9],
        ["bbi", 3, 6, 12, 24]
    ]

writeFile = (fileName, content, cb)->
    fs.writeFile fileName, content, cb

readFile = (fileName, cb)->
    fs.readFile fileName, 'utf8', cb

formulaKey = (formula)->
    formula.map((.toString())).reduce((+), "")

saveUserData = (cb)->
    fileName = 'save/stock/userdata.json'
    writeFile fileName, JSON.stringify(userdata), cb

loadUserData = (cb)->
    fileName = 'save/stock/userdata.json'
    if fs.existsSync(fileName) == false
        return saveUserData cb
    
    (err, data) <- readFile fileName
    if err
        return cb err
    try
        loadData = JSON.parse data
        userdata.stockIds = loadData.stockIds
        userdata.formulas = loadData.formulas
        cb()
    catch e
        cb e


# ========= control ========== #

startExpress = ->
    app = express()
    app.set 'port', 8080
    app.set 'views', path.join( __dirname, '/views')
    app.set 'view engine', 'vash'

    app.get '/fn/userdata', (req, res)->
        res.json [null, userdata]

    app.get '/fn/addFormula/kd/:arg1', (req, res)->
        arg1 = parseInt req.params.arg1
        userdata.formulas.push ['kd', arg1]
        (err) <- saveUserData
        res.json [err, userdata]
    
    app.get '/fn/addFormula/ma/:arg1/:arg2', (req, res)->
        arg1 = parseInt req.params.arg1
        arg2 = parseInt req.params.arg2
        userdata.formulas.push ['ma', arg1, arg2]
        (err) <- saveUserData
        res.json [err, userdata]

    app.get '/fn/addFormula/bbi/:arg1/:arg2/:arg3/:arg4', (req, res)->
        arg1 = parseInt req.params.arg1
        arg2 = parseInt req.params.arg2
        arg3 = parseInt req.params.arg3
        arg4 = parseInt req.params.arg4
        userdata.formulas.push ['bbi', arg1, arg2, arg3, arg4]
        (err) <- saveUserData
        res.json [err, userdata]
        
    app.get '/fn/removeFormula/:name', (req, res)->
        name = req.params.name
        userdata.formulas = userdata.formulas.filter (f)->
            formulaKey(f) != name
        (err) <- saveUserData
        res.json [err, userdata]
        
    app.get '/fn/addStockId/:stockId', (req, res)->
        stockId = req.params.stockId
        userdata.stockIds[stockId] = 0
        (err) <- saveUserData
        res.json [err, userdata]
    
    app.get '/fn/removeStockId/:stockId', (req, res)->
        stockId = req.params.stockId
        delete userdata.stockIds[stockId]
        (err) <- saveUserData
        res.json [err, userdata]

    app.get '/fn/compute/:year', (req, res)->
        year = req.params.year
    
        computeOne = (stockId, cb)-->
            (err, data) <- Tool.fetchStockData stockId, [year], [1 to 12]
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
                            /*
                            [kdK, kdD] = stockData |> Formula.RSV arg1, _ |> Formula.KD _
                            buys = Earn.checkSignal(kdK, kdD, kdD, stockData).filter(({action})->action == "buy")
                            
                            ma1 = close |> Formula.MA 2, _
                            ma2 = close |> Formula.MA 20, _
                            sells = Earn.checkSignal(ma1, ma2, ma2, stockData)
                            
                            signals = []
                            lastSellIdx = -1
                            for b in buys
                                if b.idx <= lastSellIdx
                                    continue
                                signals.push b
                                
                                firstBuy = false
                                for s in sells
                                    if s.idx <= b.idx
                                        continue
                                    if firstBuy == false && s.action == "buy"
                                        firstBuy = true
                                    if firstBuy == true && s.action == "sell"
                                        signals.push s
                                        lastSellIdx = s.idx
                                        break
                                        
                            earn = Earn.checkEarn stockData, signals
                            [formula, earn, signals]
                            */
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
            
        fns = for stockId, _ of userdata.stockIds
            computeOne stockId
        
        (err, data) <- async.series fns
        if err
            console.log err
            return res.json [err]
        
        res.json [null, data]
    
    
    app.get '/fn/test/:stockId/:year/:earnRate', (req, res)->
        stockId = req.params.stockId
        year = req.params.year
        earnRate = parseInt req.params.earnRate
        
        (err, data) <- Tool.fetchStockData stockId, [year], [1 to 12]
        if err
            return res.json [err]
            
        stockData = data |> Tool.formatStockData
        style = Earn.checkStyle stockData

        stocks = []
        tx = []
        for day in stockData
            if stocks.length == 0
                stocks.push day
            else
                [_, low, open, close, high] = day
                sellOk = false
                
                for i in [0 til stocks.length].reverse()
                    [_, _, prevOpen, _, _] = stocks[i]
                    rate = (open - prevOpen)*100 / prevOpen
                    if rate >= earnRate
                        tx.push [stocks[i], day]
                        stocks = stocks.slice(0, i) ++ stocks.slice(i+1, stocks.length)
                        sellOk = true
                
                if sellOk == false
                    stocks.push day
        
        txPrice = tx.map(([first])->first) |> Formula.Open
        avg = txPrice |> Formula.avg _
        sd = txPrice |> Formula.StandardDeviation avg, _
        
        res.json [null, {
            style: style, 
            txRate: tx.length/(tx.length + stocks.length),
            earnRate: Math.pow(((earnRate/100) - 0.001425)+1, tx.length)
            price:{
                avg: avg
                sd: sd
            }
            stocks: stocks
            tx: tx
        }]
    
    
    app.get '/fn/block/:ma/:mb/:range/:count/:earnRate', (req, res)->
        count = req.params.count
        ma = req.params.ma
        mb = req.params.mb
        range = req.params.range
        earnRate = parseInt req.params.earnRate
        
        (err, data) <- Tool.fetch "https://api.binance.com/api/v1/klines?interval=#{range}&limit=#{count}&symbol="+mb.toUpperCase()+ma.toUpperCase(), false
        if err
            return res.json [err.error]
        
        format = ([openTime, open, high, low, close])->
            [new Date(openTime).toString()] ++ ([low, open, close, high].map parseFloat)
            
        stockData = data |> JSON.parse _ |> Array.prototype.map.call _, format
        
        style = Earn.checkStyle stockData
        
        stocks = []
        tx = []
        for day in stockData
            if stocks.length == 0
                stocks.push day
            else
                [_, low, open, close, high] = day
                sellOk = false
                
                for i in [0 til stocks.length].reverse()
                    [_, _, prevOpen, _, _] = stocks[i]
                    rate = (open - prevOpen)*100 / prevOpen
                    if rate >= earnRate
                        tx.push [stocks[i], day]
                        stocks = stocks.slice(0, i) ++ stocks.slice(i+1, stocks.length)
                        sellOk = true
                
                if sellOk == false
                    stocks.push day
        
        txPrice = tx.map(([first])->first) |> Formula.Open
        avg = txPrice |> Formula.avg _
        sd = txPrice |> Formula.StandardDeviation avg, _
        
        min = max = 0
        if stocks.length > 0
            min = stocks |> Formula.Open |> Math.min.apply null, _ 
            max = stocks |> Formula.Open |> Math.max.apply null, _
     
        res.json [null, {
            style: style, 
            txRate: tx.length/(tx.length + stocks.length),
            earnRate: Math.pow(((earnRate/100) - 0.001425)+1, tx.length)
            check:{
                min: min
                max: max
                rate: if min != 0 then (max - min)/min else 0
            }
            price:{
                avg: avg
                sd: sd
            }
            stocks: stocks
            tx: tx
        }]
        
    app.listen 8080


startApp = ->
    (err) <- loadUserData
    if err
        return console.log err
    startExpress()
    console.log 'startApp'

startApp()
    