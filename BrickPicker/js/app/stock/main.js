// Generated by LiveScript 1.6.0
(function(){
  var express, path, async, fs, Formula, Tool, Earn, Config, bodyParser, writeFile, readFile, mapn, formulaKey, saveUserData_, loadUserData_, startExpress, loadConfig, startApp;
  express = require('express');
  path = require('path');
  async = require('async');
  fs = require('fs');
  Formula = require('../../stock/formula');
  Tool = require('../../stock/tool');
  Earn = require('../../stock/earn');
  Config = require('init-config');
  bodyParser = require('body-parser');
  writeFile = function(fileName, content, cb){
    return fs.writeFile(fileName, content, cb);
  };
  readFile = function(fileName, cb){
    return fs.readFile(fileName, 'utf8', cb);
  };
  mapn = function(f){
    var args, res$, i$, to$, maxLength, ref$, len$, i, results$ = [];
    res$ = [];
    for (i$ = 1, to$ = arguments.length; i$ < to$; ++i$) {
      res$.push(arguments[i$]);
    }
    args = res$;
    maxLength = Math.min.apply(null, args.map(function(it){
      return it.length;
    }));
    for (i$ = 0, len$ = (ref$ = (fn$())).length; i$ < len$; ++i$) {
      i = ref$[i$];
      results$.push(f.apply(null, args.map(fn1$)));
    }
    return results$;
    function fn$(){
      var i$, to$, results$ = [];
      for (i$ = 0, to$ = maxLength; i$ < to$; ++i$) {
        results$.push(i$);
      }
      return results$;
    }
    function fn1$(ary){
      return ary[i];
    }
  };
  formulaKey = function(formula){
    return formula.map(function(it){
      return it.toString();
    }).reduce(curry$(function(x$, y$){
      return x$ + y$;
    }), "");
  };
  saveUserData_ = curry$(function(cfg, userdata, cb){
    var saveDir, fileName;
    saveDir = cfg.saveDir;
    fileName = saveDir + "/userdata.json";
    return writeFile(fileName, JSON.stringify(userdata), cb);
  });
  loadUserData_ = curry$(function(cfg, cb){
    var saveDir, fileName;
    saveDir = cfg.saveDir;
    fileName = saveDir + "/userdata.json";
    if (fs.existsSync(fileName) === false) {
      return saveUserData(cb, {
        stockIds: [],
        formulas: [],
        earnRateSettings: []
      });
    }
    return readFile(fileName, function(err, data){
      var loadData, e;
      if (err) {
        return cb(err);
      }
      try {
        loadData = JSON.parse(data);
        return cb(null, loadData);
      } catch (e$) {
        e = e$;
        return cb(e);
      }
    });
  });
  startExpress = function(cfg){
    var saveUserData, loadUserData, app;
    saveUserData = saveUserData_(cfg);
    loadUserData = loadUserData_(cfg);
    app = express();
    app.set('port', 8080);
    app.set('views', path.join(__dirname, '/view'));
    app.set('view engine', 'vash');
    app.use(bodyParser.urlencoded({
      extended: true
    }));
    app.use('/', express['static'](path.join(__dirname, '/www')));
    app.get('/fn/userdata', function(req, res){
      return loadUserData(function(err, userdata){
        if (err) {
          return res.json([err]);
        }
        return res.render("edit", {
          data: userdata
        });
      });
    });
    app.get('/fn/addFormula/kd/:arg1', function(req, res){
      var arg1;
      arg1 = parseInt(req.params.arg1);
      return loadUserData(function(err, userdata){
        if (err) {
          return res.json([err]);
        }
        userdata.formulas.push(['kd', arg1]);
        return saveUserData(userdata, function(err){
          return res.json([err, userdata]);
        });
      });
    });
    app.get('/fn/addFormula/ma/:arg1/:arg2', function(req, res){
      var arg1, arg2;
      arg1 = parseInt(req.params.arg1);
      arg2 = parseInt(req.params.arg2);
      return loadUserData(function(err, userdata){
        if (err) {
          return res.json([err]);
        }
        userdata.formulas.push(['ma', arg1, arg2]);
        return saveUserData(userdata, function(err){
          return res.json([err, userdata]);
        });
      });
    });
    app.get('/fn/addFormula/bbi/:arg1/:arg2/:arg3/:arg4', function(req, res){
      var arg1, arg2, arg3, arg4;
      arg1 = parseInt(req.params.arg1);
      arg2 = parseInt(req.params.arg2);
      arg3 = parseInt(req.params.arg3);
      arg4 = parseInt(req.params.arg4);
      return loadUserData(function(err, userdata){
        if (err) {
          return res.json([err]);
        }
        userdata.formulas.push(['bbi', arg1, arg2, arg3, arg4]);
        return saveUserData(userdata, function(err){
          return res.json([err, userdata]);
        });
      });
    });
    app.get('/fn/removeFormula/:name', function(req, res){
      var name;
      name = req.params.name;
      return loadUserData(function(err, userdata){
        if (err) {
          return res.json([err]);
        }
        userdata.formulas = userdata.formulas.filter(function(f){
          return formulaKey(f) !== name;
        });
        return saveUserData(userdata, function(err){
          return res.json([err, userdata]);
        });
      });
    });
    app.post('/fn/addStockId', function(req, res){
      var stockId;
      stockId = req.body.stockId;
      return loadUserData(function(err, userdata){
        var isExist;
        if (err) {
          return res.json([err]);
        }
        isExist = (function(it){
          return it > 0;
        })(
        function(it){
          return it.length;
        }(
        userdata.stockIds.filter(function(id){
          return id === stockId;
        })));
        if (stockId.trim() === "") {
          res.json("no stockId");
          return;
        }
        if (!isExist) {
          userdata.stockIds.push(stockId);
        }
        return saveUserData(userdata, function(err){
          return res.redirect("/fn/userdata");
        });
      });
    });
    app.post('/fn/removeStockId', function(req, res){
      var stockId;
      stockId = req.body.stockId;
      return loadUserData(function(err, userdata){
        if (err) {
          return res.json([err]);
        }
        userdata.stockIds = userdata.stockIds.filter(function(id){
          return id !== stockId;
        });
        return saveUserData(userdata, function(err){
          return res.redirect("/fn/userdata");
        });
      });
    });
    app.get('/fn/dashboard', function(req, res){
      return loadUserData(function(err, userdata){
        var year, fns;
        if (err) {
          return res.json([err]);
        }
        year = new Date().getFullYear();
        fns = userdata.stockIds.map(function(stockId){
          return Tool.fetchStockData(stockId, [year], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], cfg.cacheDir);
        });
        return async.parallel(fns, function(err, results){
          var res$, i$, ref$, len$, ref1$, stockId, data, stockData, styles, e;
          if (err) {
            console.log(err);
            return res.json([err]);
          }
          res$ = [];
          for (i$ = 0, len$ = (ref$ = mapn(fn$, userdata.stockIds, results)).length; i$ < len$; ++i$) {
            ref1$ = ref$[i$], stockId = ref1$[0], data = ref1$[1];
            try {
              stockData = Tool.formatStockData(
              data);
              styles = [120, 60, 20, 10, 5].map(fn1$);
              res$.push({
                stock: stockId,
                styles: styles,
                price: stockData[stockData.length - 1]
              });
            } catch (e$) {
              e = e$;
              console.log(e);
              res$.push({
                stock: stockId,
                msg: 'some thing wrong'
              });
            }
          }
          results = res$;
          results.sort(function(arg$, arg1$){
            var a, b;
            a = arg$.styles;
            b = arg1$.styles;
            return b[b.length - 1] - a[a.length - 1];
          });
          return res.render("dashboard", {
            data: results
          });
          function fn$(){
            var args, res$, i$, to$;
            res$ = [];
            for (i$ = 0, to$ = arguments.length; i$ < to$; ++i$) {
              res$.push(arguments[i$]);
            }
            args = res$;
            return args;
          }
          function fn1$(cnt){
            var minCnt;
            minCnt = Math.min(cnt, stockData.length);
            return Earn.checkStyle(stockData.slice(stockData.length - minCnt, stockData.length));
          }
        });
      });
    });
    app.get('/fn/earnRates', function(req, res){
      return loadUserData(function(err, userdata){
        var fns;
        if (err) {
          return res.json([err]);
        }
        fns = userdata.earnRateSettings.map(function(setting){
          var stockId, year, count, earnRate;
          stockId = setting.stockId, year = setting.year, count = setting.count, earnRate = setting.earnRate;
          return function(cb){
            return async.waterfall([
              Tool.fetchStockData(stockId, [year], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], cfg.cacheDir), function(data, cb){
                var stockData, cnt, earnInfo;
                stockData = Tool.formatStockData(
                data);
                cnt = Math.min(count, stockData.length);
                stockData = stockData.slice(stockData.length - cnt, stockData.length);
                earnInfo = Earn.checkLowHighEarn(earnRate, stockData);
                earnInfo.style = Earn.checkStyle(stockData);
                earnInfo.setting = setting;
                return cb(null, earnInfo);
              }
            ], cb);
          };
        });
        return async.series(fns, function(err, results){
          if (err) {
            return res.json([err]);
          }
          return res.render("earnRate", {
            userdata: userdata,
            result: results
          });
        });
      });
    });
    app.post('/fn/earnRates/add', function(req, res){
      var stockId, year, count, earnRate;
      stockId = req.body.stockId;
      year = parseInt(req.body.year);
      count = parseInt(req.body.count);
      earnRate = parseFloat(req.body.earnRate);
      return loadUserData(function(err, userdata){
        if (err) {
          return res.json([err]);
        }
        userdata.earnRateSettings.push({
          stockId: stockId,
          year: year,
          count: count,
          earnRate: earnRate
        });
        return saveUserData(userdata, function(err){
          if (err) {
            return res.json([err]);
          }
          return res.redirect("/fn/earnRates");
        });
      });
    });
    app.post('/fn/earnRates/remove', function(req, res){
      var stockId, year, count, earnRate;
      stockId = req.body.stockId;
      year = parseInt(req.body.year);
      count = parseInt(req.body.count);
      earnRate = parseFloat(req.body.earnRate);
      return loadUserData(function(err, userdata){
        if (err) {
          return res.json([err]);
        }
        userdata.earnRateSettings = userdata.earnRateSettings.filter(function(info){
          if (!info) {
            return false;
          }
          return info.stockId !== stockId || info.year !== year || info.count !== count || info.earnRate !== earnRate;
        });
        return saveUserData(userdata, function(err){
          if (err) {
            return res.json([err]);
          }
          return res.redirect("/fn/earnRates");
        });
      });
    });
    app.get('/fn/compute/:year', function(req, res){
      var year, computeOne;
      year = req.params.year;
      computeOne = curry$(function(userdata, stockId, cb){
        return Tool.fetchStockData(stockId, [year], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], cfg.cacheDir, function(err, data){
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
              kline: stockData,
              results: results
            });
          } catch (e$) {
            e = e$;
            console.log(e);
            return cb(e.error);
          }
        });
      });
      return loadUserData(function(err, userdata){
        var fns;
        if (err) {
          console.log(err);
          return res.json([err]);
        }
        console.log(userdata);
        fns = userdata.stockIds.map(function(stockId){
          return computeOne(userdata, stockId);
        });
        return async.series(fns, function(err, data){
          if (err) {
            console.log(err);
            return res.json([err]);
          }
          return res.render("stock", {
            data: JSON.stringify(data)
          });
        });
      });
    });
    app.get('/fn/test/:stockId/:year/:count/:earnRate', function(req, res){
      var stockId, year, count, earnRate;
      stockId = req.params.stockId;
      year = req.params.year;
      count = parseInt(req.params.count);
      earnRate = parseInt(req.params.earnRate);
      return Tool.fetchStockData(stockId, [year], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], cfg.cacheDir, function(err, data){
        var stockData, cnt, earnInfo;
        if (err) {
          return res.json([err]);
        }
        stockData = Tool.formatStockData(
        data);
        cnt = Math.min(count, stockData.length);
        stockData = stockData.slice(stockData.length - cnt, stockData.length);
        earnInfo = Earn.checkLowHighEarn(earnRate, stockData);
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
      return Tool.fetch(("https://api.binance.com/api/v1/klines?interval=" + range + "&limit=" + count + "&symbol=") + mb.toUpperCase() + ma.toUpperCase(), cfg.cacheDir, function(err, data){
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
        earnInfo = Earn.checkLowHighEarn(earnRate, stockData);
        earnInfo.style = Earn.checkStyle(stockData);
        return res.json([null, earnInfo]);
      });
    });
    return app.listen(8080);
  };
  loadConfig = function(){
    var defaults, config1, config2, e;
    defaults = Config.Defaults({
      configPath: Config.Value('./config.json', Config.CLI(['configPath', 'cp'], 'path to load config'))
    });
    config1 = Config.init(defaults);
    try {
      config2 = Config.init(config1.configPath);
      return [null, config2];
    } catch (e$) {
      e = e$;
      console.log(defaults.toTerminal());
      return [e];
    }
  };
  startApp = function(){
    var ref$, err, config;
    ref$ = loadConfig(), err = ref$[0], config = ref$[1];
    if (err) {
      return console.log(err);
    }
    console.log(config);
    startExpress(config);
    return console.log('startApp');
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
