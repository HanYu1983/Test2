// Generated by LiveScript 1.6.0
(function(){
  var express, path, async, ref$, Close, Low, High, MA, EMA, RSV, KD, BBI, EBBI, MACDDIF, MACDDEM, Chaikin, UOS, Trix, YuClock, fetchStockData, formatStockData, checkSignal, checkEarn, checkEarn2, app;
  express = require('express');
  path = require('path');
  async = require('async');
  ref$ = require('./stock/formula'), Close = ref$.Close, Low = ref$.Low, High = ref$.High, MA = ref$.MA, EMA = ref$.EMA, RSV = ref$.RSV, KD = ref$.KD, BBI = ref$.BBI, EBBI = ref$.EBBI, MACDDIF = ref$.MACDDIF, MACDDEM = ref$.MACDDEM, Chaikin = ref$.Chaikin, UOS = ref$.UOS, Trix = ref$.Trix, YuClock = ref$.YuClock;
  ref$ = require('./stock/tool'), fetchStockData = ref$.fetchStockData, formatStockData = ref$.formatStockData;
  ref$ = require('./stock/earn'), checkSignal = ref$.checkSignal, checkEarn = ref$.checkEarn, checkEarn2 = ref$.checkEarn2;
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
  app = express();
  app.set('port', 8080);
  app.set('views', path.join(__dirname, '/views'));
  app.set('view engine', 'vash');
  app.get('/view/stock/:year/:cnt/:stockId', function(req, res){
    var stockId, year, cnt;
    stockId = req.params.stockId;
    year = req.params.year;
    cnt = parseInt(req.params.cnt);
    return fetchStockData(stockId, [year], (function(){
      var i$, to$, results$ = [];
      for (i$ = 1, to$ = cnt; i$ <= to$; ++i$) {
        results$.push(i$);
      }
      return results$;
    }()), function(err, data){
      var stockData, line25, res$, i$, to$, line50, line75, line0, close, ma5, ma10, ref$, kdK, kdD, ema5, ema10, dif, dem, bbi, chaikin, uos, trix, matrix, yuClock, checks, len$, name, l, l2, l3, e;
      if (err) {
        return res.json(err);
      }
      try {
        stockData = formatStockData(
        data);
        res$ = [];
        for (i$ = 0, to$ = stockData.length; i$ < to$; ++i$) {
          res$.push(25);
        }
        line25 = res$;
        res$ = [];
        for (i$ = 0, to$ = stockData.length; i$ < to$; ++i$) {
          res$.push(50);
        }
        line50 = res$;
        res$ = [];
        for (i$ = 0, to$ = stockData.length; i$ < to$; ++i$) {
          res$.push(75);
        }
        line75 = res$;
        res$ = [];
        for (i$ = 0, to$ = stockData.length; i$ < to$; ++i$) {
          res$.push(0);
        }
        line0 = res$;
        close = Close(
        stockData);
        ma5 = MA(5, close);
        ma10 = MA(10, close);
        ref$ = KD(RSV(9, stockData)), kdK = ref$[0], kdD = ref$[1];
        ema5 = EMA(2, close);
        ema10 = EMA(20, close);
        dif = MACDDIF(12, 26, close);
        dem = MACDDEM(9, dif);
        bbi = BBI(3, 6, 12, 24, close);
        chaikin = Chaikin(3, 10, stockData);
        uos = UOS(5, 10, 20, stockData);
        ref$ = Trix(12, 12, stockData), trix = ref$[0], matrix = ref$[1];
        yuClock = YuClock(20, 5, stockData);
        checks = [
          ["ma", ma5, ma10, ma10], ["ema2-20", ema5, ema10, ema10], ["kd", kdK, kdD, kdD], ["macd", dif, dem, dem], ["bbi", close, bbi, bbi], ["chaikin", chaikin, line0, line0], ["uos", uos, line50, line50], ["trix", trix, matrix, matrix], [
            "yuClock", yuClock, (function(){
              var i$, to$, results$ = [];
              for (i$ = 0, to$ = stockData.length; i$ < to$; ++i$) {
                results$.push(-0.5);
              }
              return results$;
            }()), (function(){
              var i$, to$, results$ = [];
              for (i$ = 0, to$ = stockData.length; i$ < to$; ++i$) {
                results$.push(0.5);
              }
              return results$;
            }())
          ]
        ];
        for (i$ = 0, len$ = checks.length; i$ < len$; ++i$) {
          ref$ = checks[i$], name = ref$[0], l = ref$[1], l2 = ref$[2], l3 = ref$[3];
          console.log(name);
          console.log(checkEarn2(
          checkSignal(l, l2, l3, stockData)));
        }
        return res.render("kline2", {
          data: JSON.stringify(stockData),
          close: JSON.stringify(close),
          ma5: JSON.stringify(ma5),
          ma10: JSON.stringify(ma10),
          kdK: JSON.stringify(kdK),
          kdD: JSON.stringify(kdD),
          ema5: JSON.stringify(ema5),
          ema10: JSON.stringify(ema10),
          macdDem: JSON.stringify(dem),
          macdDif: JSON.stringify(dif),
          bbi: JSON.stringify(bbi),
          chaikin: JSON.stringify(chaikin),
          uos: JSON.stringify(uos),
          trix: JSON.stringify(trix),
          matrix: JSON.stringify(matrix),
          yuClock: JSON.stringify(yuClock)
        });
      } catch (e$) {
        e = e$;
        console.log(e);
        return res.json('error');
      }
    });
  });
  app.get('/', function(req, res){
    return res.render('index', {
      title: "[['Mon', 20, 28, 38, 45]]",
      data: "[['Mon', 20, 28, 38, 45],['Tue', 31, 38, 55, 66],['Wed', 50, 55, 77, 80],['Thu', 77, 77, 66, 50],['Fri', 68, 66, 22, 15]]"
    });
  });
  app.listen(8080);
}).call(this);
