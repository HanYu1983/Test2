
require! {
    path
    request
    crypto
    async
    fs
}

export fetch = (url, dontUseCache, cb) -->
  urlKey = crypto.createHash('md5').update(url).digest('hex')
  path = "cache/"+urlKey+".html"
  
  if (!!dontUseCache) == false && fs.existsSync path
      fs.readFile path, 'utf8', cb
  else
      ws = fs.createWriteStream path
          .on('error', cb)
          .on('finish', ->
              if (fs.existsSync path) == false
                  cb 'save lost'
              else
                  fs.readFile path, 'utf8', cb
          )
      request
          .get(url)
          .on('error', cb)
          .pipe ws

export fetchStockData = (stockId, years, months, cb)-->
    urls = [[y, m] for y in years for m in months] |>
        Array.prototype.map.call _, ([y, m])->"http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=#{y}#{(m+'').padStart(2, '0')}01&stockNo=#{stockId}"

    (err, results) <- async.series do
        urls.map (url)-> fetch url, false

    cb err, results

export formatStockData = (data) ->
    data = data
        .filter (r)->r.trim() != ""
        .map JSON.parse
        .filter ({stat})-> stat=="OK"
        .reduce ((acc, {data})->acc ++ data), []

    format = ([openTime, _, _, open, high, low, close, _, volumn])->
        [openTime] ++ ([low, open, close, high, volumn].map parseFloat)
        /*
        tmp = new Date(openTime)
        y = tmp.getFullYear() + 1911
        m = tmp.getMonth() + 1
        d = tmp.getDay() + 1 
        [new Date("#{y}/#{m}/#{d}").getTime()] ++ ([low, open, close, high, volumn].map parseFloat)
        */

    return data.map format