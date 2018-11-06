
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
                            earn = Earn.checkEarn2 signals
                            [formula, earn, signals]
                        | "bbi" =>
                            bbi = close |> Formula.BBI arg1, arg2, arg3, arg4, _
                            signals = Earn.checkSignal close, bbi, bbi, stockData
                            earn = Earn.checkEarn2 signals
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
    
    app.listen 8080


startApp = ->
    (err) <- loadUserData
    if err
        return console.log err
    startExpress()
    console.log 'startApp'

startApp()
    