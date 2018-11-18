// Generated by LiveScript 1.6.0
(function(){
  var path, request, crypto, async, fs, fetch, fetchStockData, formatStockData, out$ = typeof exports != 'undefined' && exports || this;
  path = require('path');
  request = require('request');
  crypto = require('crypto');
  async = require('async');
  fs = require('fs');
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
  out$.fetch = fetch = curry$(function(url, cacheDir, cb){
    var urlKey, path, ws;
    console.log(url);
    urlKey = crypto.createHash('md5').update(url).digest('hex');
    path = cacheDir + "" + urlKey + ".html";
    if (cacheDir && fs.existsSync(path)) {
      return fs.readFile(path, 'utf8', cb);
    } else {
      ws = fs.createWriteStream(path).on('error', cb).on('finish', function(){
        if (fs.existsSync(path) === false) {
          return cb('save lost');
        } else {
          return fs.readFile(path, 'utf8', cb);
        }
      });
      return request.get(url).on('error', cb).pipe(ws);
    }
  });
  out$.fetchStockData = fetchStockData = curry$(function(stockId, years, months, cacheDir, cb){
    var now, fns, y, m;
    now = new Date;
    fns = (function(){
      var i$, ref$, len$, j$, ref1$, len1$, results$ = [];
      for (i$ = 0, len$ = (ref$ = years).length; i$ < len$; ++i$) {
        y = ref$[i$];
        for (j$ = 0, len1$ = (ref1$ = months).length; j$ < len1$; ++j$) {
          m = ref1$[j$];
          results$.push([y, m]);
        }
      }
      return results$;
    }()).map(function(arg$){
      var y, m;
      y = arg$[0], m = arg$[1];
      if (now.getMonth() + 1 === m) {
        return fetch("http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=" + y + (m + '').padStart(2, '0') + "01&stockNo=" + stockId, null);
      } else {
        return fetch("http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=" + y + (m + '').padStart(2, '0') + "01&stockNo=" + stockId, cacheDir);
      }
    });
    return async.series(fns, function(err, results){
      return cb(err, results);
    });
  });
  out$.formatStockData = formatStockData = function(data){
    var format;
    data = data.filter(function(r){
      return r.trim() !== "";
    }).map(function(v){
      var e;
      try {
        return JSON.parse(v);
      } catch (e$) {
        e = e$;
        console.log(e);
        return {
          stat: "error"
        };
      }
    }).filter(function(arg$){
      var stat;
      stat = arg$.stat;
      return stat === "OK";
    }).reduce(function(acc, arg$){
      var data;
      data = arg$.data;
      return acc.concat(data);
    }, []);
    format = function(arg$){
      var openTime, _, open, high, low, close, volumn;
      openTime = arg$[0], _ = arg$[1], _ = arg$[2], open = arg$[3], high = arg$[4], low = arg$[5], close = arg$[6], _ = arg$[7], volumn = arg$[8];
      return [openTime].concat([low, open, close, high, volumn].map(parseFloat));
    };
    return data.map(format);
  };
  function curry$(f, bound){
    var context,
    _curry = function(args) {
      return f.length > 1 ? function(){
        var params = args ? args.concat() : [];
        context = bound ? context || this : this;
        return params.push.apply(params, arguments) <
            f.length && arguments.length ?
          _curry.call(context, params) : f.apply(context, params);
      } : f;
    };
    return _curry();
  }
}).call(this);
