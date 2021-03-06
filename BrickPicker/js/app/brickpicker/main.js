// Generated by LiveScript 1.6.0
(function(){
  var express, path, async, fs, Formula, Tool, Earn, WebSocket, guess;
  express = require('express');
  path = require('path');
  async = require('async');
  fs = require('fs');
  Formula = require('../../stock/formula');
  Tool = require('../../stock/tool');
  Earn = require('../../stock/earn');
  WebSocket = require('ws');
  guess = function(cb){
    var count, ma, mb, range, earnRate;
    count = 240;
    ma = "usdt";
    mb = "btc";
    range = "1h";
    earnRate = 100;
    return Tool.fetch(("https://api.binance.com/api/v1/klines?interval=" + range + "&limit=" + count + "&symbol=") + mb.toUpperCase() + ma.toUpperCase(), true, function(err, data){
      var format, stockData, earnInfo;
      if (err) {
        return cb(err.error);
      }
      format = function(arg$){
        var openTime, open, high, low, close;
        openTime = arg$[0], open = arg$[1], high = arg$[2], low = arg$[3], close = arg$[4];
        return [new Date(openTime).toString()].concat([low, open, close, high].map(parseFloat));
      };
      stockData = Array.prototype.map.call(JSON.parse(data), format);
      earnInfo = Earn.checkLowHighEarn(earnRate, stockData);
      earnInfo.style = Earn.checkStyle(stockData);
      return cb(null, earnInfo);
    });
  };
  guess(function(err, earnInfo){
    var txRate, price, avg, sd, tx, style, check, root, x$, tradeStream;
    if (err) {
      return err;
    }
    txRate = earnInfo.txRate, price = earnInfo.price, avg = price.avg, sd = price.sd, tx = earnInfo.tx, style = earnInfo.style, check = earnInfo.check;
    console.log("style", style);
    console.log(check);
    if (tx.length === 0) {
      console.log('no tx');
      return;
    }
    console.log(txRate, avg, sd);
    if (txRate < 0.7) {
      console.log("txRate to low: " + txRate);
      return;
    }
    /*
    if txRate < 0.7
        return console.log "txRate < 0.7"
    */
    root = {
      storage: [],
      money: 0,
      lastOrderTime: 0,
      price: 999999
    };
    x$ = tradeStream = new WebSocket('wss://stream.binance.com:9443/ws/btcusdt@aggTrade');
    x$.on('open', function(){
      return console.log('open');
    });
    x$.on('close', function(){
      return console.log('close');
    });
    x$.on('message', function(data){
      var p, total, currZ, sellOk, i$, ref$, len$, i, sp;
      data = JSON.parse(data);
      p = data.p;
      p = parseFloat(p);
      root.price = p;
      total = root.money + root.storage.reduce(curry$(function(x$, y$){
        return x$ + y$;
      }), 0);
      currZ = (root.price - avg) / sd;
      console.log("totalMoney: " + total + ", currPrice: " + root.price + ", z: " + currZ + " len: " + root.storage.length + " wait...");
      sellOk = false;
      if (root.storage.length > 0) {
        for (i$ = 0, len$ = (ref$ = (fn$()).reverse()).length; i$ < len$; ++i$) {
          i = ref$[i$];
          sp = root.storage[i];
          if ((root.price - sp) / sp > 0.0001) {
            root.money = root.money + root.price;
            sellOk = true;
            root.storage = root.storage.slice(0, i).concat(root.storage.slice(i + 1, root.storage.length));
            console.log("*******");
            console.log("sell " + sp);
            console.log("money: " + root.money);
            console.log("storage.len " + root.storage.length);
          }
        }
      }
      if (sellOk === false) {
        if (new Date().getTime() - root.lastOrderTime < 60000) {
          return console.log("wait until 1 minute");
        } else if (root.price > avg - sd && root.price <= avg + sd) {
          root.storage.push(root.price);
          root.money = root.money - root.price;
          root.lastOrderTime = new Date().getTime();
          console.log("buy", root.price);
          console.log("money: " + root.money);
          return console.log("storage.len " + root.storage.length);
        }
      }
      function fn$(){
        var i$, to$, results$ = [];
        for (i$ = 0, to$ = root.storage.length; i$ < to$; ++i$) {
          results$.push(i$);
        }
        return results$;
      }
    });
    x$.on('error', function(err){
      return console.log(err);
    });
    return x$;
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
