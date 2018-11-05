// Generated by LiveScript 1.6.0
(function(){
  var path, request, async, crypto, fs, file, fetch, testFetch, fetchStockData, formatStockData, Close, MA, RSV, KD, checkSignal, checkEarn;
  path = require('path');
  request = require('request');
  async = require('async');
  crypto = require('crypto');
  fs = require('fs');
  file = "./test.db";
  /*
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
  fetch = curry$(function(url, dontUseCache, cb){
    var urlKey, path, ws;
    urlKey = crypto.createHash('md5').update(url).digest('hex');
    path = "cache/" + urlKey + ".html";
    if (!!dontUseCache === false && fs.existsSync(path)) {
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
  testFetch = function(stockId, cnt){
    return async.series((function(){
      var i$, to$, results$ = [];
      for (i$ = 1, to$ = cnt; i$ <= to$; ++i$) {
        results$.push(i$);
      }
      return results$;
    }()).map(function(m){
      return fetch("http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=2017" + (m + '').padStart(2, '0') + "01&stockNo=" + stockId, false);
    }), function(err, results){
      if (err) {
        return console.log(err);
      } else {
        return console.log(results);
      }
    });
  };
  fetchStockData = function(stockId, years, months, cb){
    var urls, y, m;
    urls = Array.prototype.map.call((function(){
      var i$, ref$, len$, j$, ref1$, len1$, results$ = [];
      for (i$ = 0, len$ = (ref$ = years).length; i$ < len$; ++i$) {
        y = ref$[i$];
        for (j$ = 0, len1$ = (ref1$ = months).length; j$ < len1$; ++j$) {
          m = ref1$[j$];
          results$.push([y, m]);
        }
      }
      return results$;
    }()), function(arg$){
      var y, m;
      y = arg$[0], m = arg$[1];
      return "http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date=" + y + (m + '').padStart(2, '0') + "01&stockNo=" + stockId;
    });
    return async.series(urls.map(function(url){
      return fetch(url, false);
    }), function(err, results){
      return cb(err, results);
    });
  };
  formatStockData = function(data){
    var format;
    data = data.filter(function(r){
      return r.trim() !== "";
    }).map(JSON.parse).filter(function(arg$){
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
      return [new Date(openTime).toString()].concat([low, open, close, high, volumn].map(parseFloat));
    };
    return data.map(format);
  };
  Close = function(data){
    return data.map(function(arg$){
      var _, close;
      _ = arg$[0], _ = arg$[1], _ = arg$[2], close = arg$[3];
      return close;
    });
  };
  MA = function(cnt, data){
    var ret, res$, i$, ref$, len$, i, avg;
    res$ = [];
    for (i$ = 0, len$ = (ref$ = (fn$())).length; i$ < len$; ++i$) {
      i = ref$[i$];
      res$.push(avg = data.slice(i - (cnt - 1), i + 1).reduce(fn1$, 0) / cnt);
    }
    ret = res$;
    return (function(){
      var i$, to$, results$ = [];
      for (i$ = 0, to$ = cnt - 1; i$ < to$; ++i$) {
        results$.push(0);
      }
      return results$;
    }()).concat(ret);
    function fn$(){
      var i$, to$, results$ = [];
      for (i$ = cnt - 1, to$ = data.length; i$ < to$; ++i$) {
        results$.push(i$);
      }
      return results$;
    }
    function fn1$(acc, curr){
      return acc + curr;
    }
  };
  RSV = function(cnt, data){
    var ret, res$, i$, ref$, len$, i, ref1$, openTime, low, open, close, high, before9k, min9k, max9k, rsv;
    res$ = [];
    for (i$ = 0, len$ = (ref$ = (fn$())).length; i$ < len$; ++i$) {
      i = ref$[i$];
      ref1$ = data[i], openTime = ref1$[0], low = ref1$[1], open = ref1$[2], close = ref1$[3], high = ref1$[4];
      before9k = data.slice(i - (cnt - 1), i + 1);
      min9k = Math.min.apply(null, before9k.map(fn1$));
      max9k = Math.max.apply(null, before9k.map(fn2$));
      res$.push(rsv = (close - min9k) * 100 / (max9k - min9k));
    }
    ret = res$;
    return (function(){
      var i$, to$, results$ = [];
      for (i$ = 0, to$ = cnt - 1; i$ < to$; ++i$) {
        results$.push(0);
      }
      return results$;
    }()).concat(ret);
    function fn$(){
      var i$, to$, results$ = [];
      for (i$ = cnt - 1, to$ = data.length; i$ < to$; ++i$) {
        results$.push(i$);
      }
      return results$;
    }
    function fn1$(arg$){
      var _, low;
      _ = arg$[0], low = arg$[1];
      return low;
    }
    function fn2$(arg$){
      var _, high;
      _ = arg$[0], _ = arg$[1], _ = arg$[2], _ = arg$[3], high = arg$[4];
      return high;
    }
  };
  KD = function(data){
    var kline, dline, i$, ref$, len$, i, rsv, prevK, prevD, k, d;
    kline = [];
    dline = [];
    for (i$ = 0, len$ = (ref$ = (fn$())).length; i$ < len$; ++i$) {
      i = ref$[i$];
      rsv = data[i];
      prevK = i > 0 ? kline[i - 1] : 50;
      prevD = i > 0 ? dline[i - 1] : 50;
      k = prevK * (2 / 3) + rsv / 3;
      d = prevD * (2 / 3) + k / 3;
      kline.push(k);
      dline.push(d);
    }
    return [kline, dline];
    function fn$(){
      var i$, to$, results$ = [];
      for (i$ = 0, to$ = data.length; i$ < to$; ++i$) {
        results$.push(i$);
      }
      return results$;
    }
  };
  checkSignal = function(line1, line2, data){
    var orders, i$, ref$, len$, i, prevK, prevD, k, d, date, open, buyPrice;
    orders = [];
    for (i$ = 0, len$ = (ref$ = (fn$())).length; i$ < len$; ++i$) {
      i = ref$[i$];
      prevK = line1[i - 1];
      prevD = line2[i - 1];
      k = line1[i];
      d = line2[i];
      if (prevK <= prevD && k > d && i < line1.length - 1) {
        date = data[i][0];
        open = data[i][2];
        buyPrice = open;
        orders.push({
          action: "buy",
          price: buyPrice,
          date: date
        });
      }
      if (prevK >= prevD && k < d && i < line1.length - 1) {
        date = data[i][0];
        open = data[i][2];
        buyPrice = open;
        orders.push({
          action: "sell",
          price: buyPrice,
          date: date
        });
      }
    }
    return orders;
    function fn$(){
      var i$, to$, results$ = [];
      for (i$ = 0, to$ = line1.length; i$ < to$; ++i$) {
        results$.push(i$);
      }
      return results$;
    }
  };
  checkEarn = function(orders){
    var storage, money, useMoney, rate, gas, i$, len$, order, price, cost, earn, earnRate, earnRateAvg, transactionTime, useMoneyPerTranaction, totalEarn, totalEarnRate;
    storage = 0;
    money = 0;
    useMoney = 0;
    rate = [];
    gas = 0.001425;
    for (i$ = 0, len$ = orders.length; i$ < len$; ++i$) {
      order = orders[i$];
      if (order.action === "buy") {
        if (storage !== 0) {
          console.log("has storage");
        } else {
          price = order.price;
          cost = price + price * gas;
          money -= cost;
          useMoney = cost;
          storage = price;
        }
      }
      if (order.action === "sell") {
        if (storage === 0) {
          console.log("no storage");
        } else {
          price = order.price;
          earn = price - price * gas;
          money += earn;
          earnRate = (earn - useMoney) / useMoney;
          storage = 0;
          rate.push(earnRate);
        }
      }
    }
    earnRateAvg = rate.reduce(function(a, b){
      return a + b;
    }, 0) / rate.length;
    transactionTime = rate.length;
    useMoneyPerTranaction = 100000;
    totalEarn = (useMoneyPerTranaction * earnRateAvg) * transactionTime;
    totalEarnRate = (totalEarn + useMoneyPerTranaction) / useMoneyPerTranaction;
    return {
      price: storage,
      amount: storage !== 0 ? useMoneyPerTranaction / storage : 0,
      moneyFlow: money + storage,
      ratePerTx: earnRateAvg,
      earn: totalEarn,
      earnRate: totalEarnRate,
      times: rate.length
    };
  };
  fetchStockData(2475, [2017], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], function(err, data){
    var stockData, close, kd, orders, result;
    if (err) {
      return console.log(err);
    }
    stockData = formatStockData(
    data);
    close = Close(
    stockData);
    kd = KD(
    RSV(9, stockData));
    orders = checkSignal(kd[0], kd[1], stockData);
    result = checkEarn(
    orders);
    console.log(result);
    orders = checkSignal(MA(5, close), MA(10, close), stockData);
    result = checkEarn(
    orders);
    console.log(result);
    orders = checkSignal(MA(2, close), MA(20, close), stockData);
    result = checkEarn(
    orders);
    return console.log(result);
  });
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
