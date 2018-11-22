require! {
    request
}



getStockInfo = (stockId, cb)->
    option = 
        url: "https://goodinfo.tw/StockInfo/ShowK_Chart.asp?STOCK_ID=#{stockId}&CHT_CAT2=DATE"
        method: "GET"
    request option, (err, res, body)-> cb(err, body)

(err, res) <- getStockInfo 2331
console.log err
console.log res