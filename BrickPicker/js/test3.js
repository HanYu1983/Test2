// Generated by LiveScript 1.6.0
(function(){
  var request, async, getUrl, updatePrice, checkBuy;
  request = require('request');
  async = require('async');
  getUrl = curry$(function(url, cb){
    var options, callback;
    options = {
      url: url,
      method: 'GET',
      headers: {
        'User-Agent': 'request'
      }
    };
    callback = function(error, response, body){
      if (error) {
        return cb(error);
      } else {
        return cb(null, JSON.parse(body));
      }
    };
    return request(options, callback);
  });
  updatePrice = function(cb){
    var storage;
    storage = {};
    return async.parallel([getUrl('https://api.binance.com/api/v1/depth?symbol=BTCUSDT'), getUrl('https://api.huobipro.com/market/depth?symbol=btcusdt&type=step0'), getUrl('https://bittrex.com/api/v1.1/public/getorderbook?market=USDT-BTC&type=both'), getUrl('https://poloniex.com/public?command=returnOrderBook&currencyPair=USDT_BTC&depth=10')], function(err, results){
      var binanceDepth, huobiDepth, bittrexDepth, poloniexDepth, bids, asks, success, message, result, buy, sell;
      if (err) {
        return cb(err);
      }
      binanceDepth = results[0], huobiDepth = results[1], bittrexDepth = results[2], poloniexDepth = results[3];
      bids = binanceDepth.bids, asks = binanceDepth.asks;
      bids = bids.map(function(ary){
        ary.pop();
        return ary.map(parseFloat);
      });
      asks = asks.map(function(ary){
        ary.pop();
        return ary.map(parseFloat);
      });
      storage.binance = {
        bids: bids,
        asks: asks
      };
      storage.huobi = huobiDepth.tick;
      success = bittrexDepth.success, message = bittrexDepth.message, result = bittrexDepth.result, buy = result.buy, sell = result.sell;
      if (success) {
        storage.bittrex = {
          bids: buy.map(function(arg$){
            var Quantity, Rate;
            Quantity = arg$.Quantity, Rate = arg$.Rate;
            return [Rate, Quantity];
          }),
          asks: sell.map(function(arg$){
            var Quantity, Rate;
            Quantity = arg$.Quantity, Rate = arg$.Rate;
            return [Rate, Quantity];
          })
        };
      } else {
        console.log(message);
      }
      bids = poloniexDepth.bids, asks = poloniexDepth.asks;
      bids = bids.map(function(ary){
        return ary.map(parseFloat);
      });
      asks = asks.map(function(ary){
        return ary.map(parseFloat);
      });
      storage.poloniex = {
        bids: bids,
        asks: asks
      };
      return cb(null, storage);
    });
  };
  checkBuy = function(storage, ma, mb, dir){
    var sell1a, buy1a, sell1b, buy1b, order, buyCost, sellEarn, space, x$, volumn, guess;
    dir == null && (dir = '>');
    sell1a = storage[ma].asks[0];
    buy1a = storage[ma].bids[0];
    sell1b = storage[mb].asks[0];
    buy1b = storage[mb].bids[0];
    order = [sell1a, buy1a, sell1b, buy1b];
    buyCost = dir === '>' ? sell1b : sell1a;
    sellEarn = dir === '>' ? buy1a : buy1b;
    space = sellEarn[0] - buyCost[0];
    x$ = volumn = [sellEarn[1], buyCost[1]];
    x$.sort();
    guess = [space * volumn[0], space * volumn[0] * 30];
    return {
      dir: ma + " " + dir + " " + mb,
      buyCost: buyCost[0],
      sellEarn: sellEarn[0],
      space: space,
      volumn: volumn,
      guess: guess
    };
  };
  updatePrice(function(err, storage){
    var info;
    info = checkBuy(storage, 'binance', 'huobi');
    console.log(info);
    info = checkBuy(storage, 'binance', 'huobi', '<');
    console.log(info);
    info = checkBuy(storage, 'binance', 'bittrex');
    console.log(info);
    info = checkBuy(storage, 'binance', 'bittrex', '<');
    console.log(info);
    info = checkBuy(storage, 'binance', 'poloniex');
    console.log(info);
    info = checkBuy(storage, 'binance', 'poloniex', '<');
    console.log(info);
    info = checkBuy(storage, 'huobi', 'bittrex');
    console.log(info);
    info = checkBuy(storage, 'huobi', 'bittrex', '<');
    console.log(info);
    info = checkBuy(storage, 'huobi', 'poloniex');
    console.log(info);
    info = checkBuy(storage, 'huobi', 'poloniex', '<');
    console.log(info);
    info = checkBuy(storage, 'bittrex', 'poloniex');
    console.log(info);
    info = checkBuy(storage, 'bittrex', 'poloniex', '<');
    return console.log(info);
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
