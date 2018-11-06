
require! {
    express
    path
    async
    "./stock/formula": {Close, Low, High, MA, EMA, RSV, KD, BBI, EBBI, MACD-DIF, MACD-DEM, Chaikin, UOS, Trix, YuClock}
    "./stock/tool": {fetchStockData, formatStockData}
    "./stock/earn": {checkSignal, checkEarn, checkEarn2}
}

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
        [trix, matrix] = stockData |> Trix 12, 12, _
        yuClock = stockData |> YuClock 20, 5, _ 
        
        checks = [
            ["ma", ma5, ma10, ma10]
            ["ema2-20", ema5, ema10, ema10]
            ["kd", kdK, kdD, kdD]
            ["macd", dif, dem, dem]
            ["bbi", close, bbi, bbi]
            ["chaikin", chaikin, line0, line0]
            ["uos", uos, line50, line50]
            ["trix", trix, matrix, matrix]
            ["yuClock", yuClock, [-0.5 for til stockData.length], [0.5 for til stockData.length]]
        ]
        
        for [name, l, l2, l3] in checks
            console.log name
            checkSignal l, l2, l3, stockData |>
            checkEarn2 |>
            console.log  _
            
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
            trix: JSON.stringify trix
            matrix: JSON.stringify matrix
            yuClock: JSON.stringify yuClock
    catch e
        console.log e
        res.json 'error'
            
            
app.get '/', (req, res)->
    res.render do
        'index'
        title: "[['Mon', 20, 28, 38, 45]]"
        data: "[['Mon', 20, 28, 38, 45],['Tue', 31, 38, 55, 66],['Wed', 50, 55, 77, 80],['Thu', 77, 77, 66, 50],['Fri', 68, 66, 22, 15]]"

app.listen 8080