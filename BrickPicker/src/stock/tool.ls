
require! {
    path
    request
    crypto
    async
    fs
}
/*
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
*/
export fetch = (url, cacheDir, cb) -->
    console.log url
    if cacheDir
        urlKey = crypto.createHash('md5').update(url).digest('hex')
        path = "#{cacheDir}#{urlKey}.html"
        if fs.existsSync path
            fs.readFile path, 'utf8', cb
        else
            ws = fs.createWriteStream path
                .on('error', cb)
                .on('finish', ->
                    if not fs.existsSync path
                        cb 'save lost'
                    else
                        fs.readFile path, 'utf8', cb
                )
            request
                .get(url)
                .on('error', cb)
                .pipe ws
    else
        request
            .get(url, (err, res, body)->
                cb(err, body)
            )
    /*
    urlKey = crypto.createHash('md5').update(url).digest('hex')
    path = "#{cacheDir}#{urlKey}.html"
    if cacheDir && fs.existsSync path
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
    */
        
export fetchStockData = (stockId, years, months, cacheDir, cb)-->
    now = new Date
    fns = [[y, m] for y in years for m in months]
        .map ([y, m])->
            if now.getMonth()+1 == m 
                # 當月不用快取
                fetch "http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=#{y}#{(m+'').padStart(2, '0')}01&stockNo=#{stockId}", null
            else
                fetch "http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=#{y}#{(m+'').padStart(2, '0')}01&stockNo=#{stockId}", cacheDir
    (err, results) <- async.series fns
    cb err, results

export formatStockData = (data) ->
    data = data
        .filter (r)->r.trim() != ""
        .map (v)->
            try
                JSON.parse v
            catch e
                console.log e
                {stat: "error"}
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