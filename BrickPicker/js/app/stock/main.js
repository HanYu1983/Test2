// Generated by LiveScript 1.6.0
(function(){
  var express, path, async, fs, Formula, Tool, Earn, userdata, writeFile, readFile, formulaKey, saveUserData, loadUserData, startExpress, startApp;
  express = require('express');
  path = require('path');
  async = require('async');
  fs = require('fs');
  Formula = require('../../stock/formula');
  Tool = require('../../stock/tool');
  Earn = require('../../stock/earn');
  userdata = {
    stockIds: {
      "2331": 0,
      "2332": 0
    },
    formulas: [["kd", 9], ["bbi", 3, 6, 12, 24]]
  };
  writeFile = function(fileName, content, cb){
    return fs.writeFile(fileName, content, cb);
  };
  readFile = function(fileName, cb){
    return fs.readFile(fileName, 'utf8', cb);
  };
  formulaKey = function(formula){
    return formula.map(function(it){
      return it.toString();
    }).reduce(curry$(function(x$, y$){
      return x$ + y$;
    }), "");
  };
  saveUserData = function(cb){
    var fileName;
    fileName = 'save/stock/userdata.json';
    return writeFile(fileName, JSON.stringify(userdata), cb);
  };
  loadUserData = function(cb){
    var fileName;
    fileName = 'save/stock/userdata.json';
    if (fs.existsSync(fileName) === false) {
      return saveUserData(cb);
    }
    return readFile(fileName, function(err, data){
      var loadData, e;
      if (err) {
        return cb(err);
      }
      try {
        loadData = JSON.parse(data);
        userdata.stockIds = loadData.stockIds;
        userdata.formulas = loadData.formulas;
        return cb();
      } catch (e$) {
        e = e$;
        return cb(e);
      }
    });
  };
  startExpress = function(){
    var app, checkLowHighEarn;
    app = express();
    app.set('port', 8080);
    app.set('views', path.join(__dirname, '/views'));
    app.set('view engine', 'vash');
    app.get('/fn/userdata', function(req, res){
      return res.json([null, userdata]);
    });
    app.get('/fn/addFormula/kd/:arg1', function(req, res){
      var arg1;
      arg1 = parseInt(req.params.arg1);
      userdata.formulas.push(['kd', arg1]);
      return saveUserData(function(err){
        return res.json([err, userdata]);
      });
    });
    app.get('/fn/addFormula/ma/:arg1/:arg2', function(req, res){
      var arg1, arg2;
      arg1 = parseInt(req.params.arg1);
      arg2 = parseInt(req.params.arg2);
      userdata.formulas.push(['ma', arg1, arg2]);
      return saveUserData(function(err){
        return res.json([err, userdata]);
      });
    });
    app.get('/fn/addFormula/bbi/:arg1/:arg2/:arg3/:arg4', function(req, res){
      var arg1, arg2, arg3, arg4;
      arg1 = parseInt(req.params.arg1);
      arg2 = parseInt(req.params.arg2);
      arg3 = parseInt(req.params.arg3);
      arg4 = parseInt(req.params.arg4);
      userdata.formulas.push(['bbi', arg1, arg2, arg3, arg4]);
      return saveUserData(function(err){
        return res.json([err, userdata]);
      });
    });
    app.get('/fn/removeFormula/:name', function(req, res){
      var name;
      name = req.params.name;
      userdata.formulas = userdata.formulas.filter(function(f){
        return formulaKey(f) !== name;
      });
      return saveUserData(function(err){
        return res.json([err, userdata]);
      });
    });
    app.get('/fn/addStockId/:stockId', function(req, res){
      var stockId;
      stockId = req.params.stockId;
      userdata.stockIds[stockId] = 0;
      return saveUserData(function(err){
        return res.json([err, userdata]);
      });
    });
    app.get('/fn/removeStockId/:stockId', function(req, res){
      var stockId;
      stockId = req.params.stockId;
      delete userdata.stockIds[stockId];
      return saveUserData(function(err){
        return res.json([err, userdata]);
      });
    });
    app.get('/fn/compute/:year', function(req, res){
      var year, computeOne, fns, res$, stockId, ref$, _;
      year = req.params.year;
      computeOne = curry$(function(stockId, cb){
        return Tool.fetchStockData(stockId, [year], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], function(err, data){
          var stockData, close, style, results, e;
          if (err) {
            return cb(err);
          }
          try {
            stockData = Tool.formatStockData(
            data);
            close = Formula.Close(
            stockData);
            style = Earn.checkStyle(stockData);
            results = userdata.formulas.map(function(formula){
              var name, arg1, arg2, arg3, arg4, ref$, kdK, kdD, signals, earn, bbi, ma1, ma2;
              name = formula[0], arg1 = formula[1], arg2 = formula[2], arg3 = formula[3], arg4 = formula[4];
              switch (name) {
              case "kd":
                ref$ = Formula.KD(Formula.RSV(arg1, stockData)), kdK = ref$[0], kdD = ref$[1];
                signals = Earn.checkSignal(kdK, kdD, kdD, stockData);
                earn = Earn.checkEarn(stockData, signals);
                return [formula, earn, signals];
              case "bbi":
                bbi = Formula.BBI(arg1, arg2, arg3, arg4, close);
                signals = Earn.checkSignal(close, bbi, bbi, stockData);
                earn = Earn.checkEarn(stockData, signals);
                return [formula, earn, signals];
              case "ma":
                ma1 = Formula.MA(arg1, close);
                ma2 = Formula.MA(arg2, close);
                signals = Earn.checkSignal(ma1, ma2, ma2, stockData);
                earn = Earn.checkEarn(stockData, signals);
                return [formula, earn, signals];
              default:

              }
            });
            return cb(null, {
              stockId: stockId,
              year: year,
              style: style,
              results: results
            });
          } catch (e$) {
            e = e$;
            console.log(e);
            return cb(e.error);
          }
        });
      });
      res$ = [];
      for (stockId in ref$ = userdata.stockIds) {
        _ = ref$[stockId];
        res$.push(computeOne(stockId));
      }
      fns = res$;
      return async.series(fns, function(err, data){
        if (err) {
          console.log(err);
          return res.json([err]);
        }
        return res.json([null, data]);
      });
    });
    checkLowHighEarn = function(earnRate, stockData){
      var stocks, tx, i$, len$, day, _, low, open, close, high, sellOk, j$, ref$, len1$, i, ref1$, prevOpen, rate, txPrice, avg, sd, z, min, max, txRate, ret;
      stocks = [];
      tx = [];
      for (i$ = 0, len$ = stockData.length; i$ < len$; ++i$) {
        day = stockData[i$];
        if (stocks.length === 0) {
          stocks.push(day);
        } else {
          _ = day[0], low = day[1], open = day[2], close = day[3], high = day[4];
          sellOk = false;
          for (j$ = 0, len1$ = (ref$ = (fn$()).reverse()).length; j$ < len1$; ++j$) {
            i = ref$[j$];
            ref1$ = stocks[i], _ = ref1$[0], _ = ref1$[1], prevOpen = ref1$[2], _ = ref1$[3], _ = ref1$[4];
            rate = (open - prevOpen) * 1000 / prevOpen;
            if (rate >= earnRate) {
              tx.push([stocks[i], day]);
              stocks = stocks.slice(0, i).concat(stocks.slice(i + 1, stocks.length));
              sellOk = true;
            }
          }
          if (sellOk === false) {
            stocks.push(day);
          }
        }
      }
      txPrice = Formula.Open(
      tx.map(function(arg$){
        var first;
        first = arg$[0];
        return first;
      }));
      avg = Formula.avg(txPrice);
      sd = Formula.StandardDeviation(avg, txPrice);
      z = Formula.ZScore(avg, sd, txPrice);
      min = max = 0;
      if (stocks.length > 0) {
        min = Math.min.apply(null, Formula.Open(
        stocks));
        max = Math.max.apply(null, Formula.Open(
        stocks));
      }
      txRate = tx.length / (tx.length + stocks.length);
      return ret = {
        txRate: txRate,
        earnRate: Math.pow((earnRate / 1000 - 0.001425) + 1, tx.length * txRate),
        maxEarnRate: Math.pow((earnRate / 1000 - 0.001425) + 1, tx.length),
        check: {
          min: min,
          max: max,
          rate: min !== 0 ? (max - min) / min : 0
        },
        price: {
          avg: avg,
          sd: sd,
          z: z
        },
        stocks: stocks,
        tx: tx
      };
      function fn$(){
        var i$, to$, results$ = [];
        for (i$ = 0, to$ = stocks.length; i$ < to$; ++i$) {
          results$.push(i$);
        }
        return results$;
      }
    };
    app.get('/fn/test/:stockId/:year/:count/:earnRate', function(req, res){
      var stockId, year, count, earnRate;
      stockId = req.params.stockId;
      year = req.params.year;
      count = parseInt(req.params.count);
      earnRate = parseInt(req.params.earnRate);
      return Tool.fetchStockData(stockId, [year], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], function(err, data){
        var stockData, cnt, earnInfo;
        if (err) {
          return res.json([err]);
        }
        stockData = Tool.formatStockData(
        data);
        cnt = Math.min(count, stockData.length);
        stockData = stockData.slice(stockData.length - cnt, stockData.length);
        earnInfo = checkLowHighEarn(earnRate, stockData);
        earnInfo.style = Earn.checkStyle(stockData);
        return res.json([null, earnInfo]);
      });
    });
    app.get('/fn/block/:ma/:mb/:range/:count/:earnRate', function(req, res){
      var count, ma, mb, range, earnRate;
      count = req.params.count;
      ma = req.params.ma;
      mb = req.params.mb;
      range = req.params.range;
      earnRate = parseInt(req.params.earnRate);
      return Tool.fetch(("https://api.binance.com/api/v1/klines?interval=" + range + "&limit=" + count + "&symbol=") + mb.toUpperCase() + ma.toUpperCase(), false, function(err, data){
        var format, stockData, earnInfo;
        if (err) {
          return res.json([err.error]);
        }
        format = function(arg$){
          var openTime, open, high, low, close;
          openTime = arg$[0], open = arg$[1], high = arg$[2], low = arg$[3], close = arg$[4];
          return [new Date(openTime).toString()].concat([low, open, close, high].map(parseFloat));
        };
        stockData = Array.prototype.map.call(JSON.parse(data), format);
        earnInfo = checkLowHighEarn(earnRate, stockData);
        earnInfo.style = Earn.checkStyle(stockData);
        return res.json([null, earnInfo]);
      });
    });
    return app.listen(8080);
  };
  startApp = function(){
    return loadUserData(function(err){
      if (err) {
        return console.log(err);
      }
      startExpress();
      return console.log('startApp');
    });
  };
  startApp();
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
