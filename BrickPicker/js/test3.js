// Generated by LiveScript 1.6.0
(function(){
  var request, async, getUrl, updatePrice, checkBuy, x$, markets, total, slice$ = [].slice;
  request = require('request');
  async = require('async');
  getUrl = curry$(function(url, cb){
    var options, callback;
    console.log(url);
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
  updatePrice = function(a, b, cb){
    var storage;
    storage = {};
    return async.parallel([getUrl("https://api.binance.com/api/v1/depth?symbol=" + b.toUpperCase() + a.toUpperCase()), getUrl("https://api.huobipro.com/market/depth?symbol=" + b + a + "&type=step0"), getUrl("https://bittrex.com/api/v1.1/public/getorderbook?type=both&market=" + a.toUpperCase() + "-" + b.toUpperCase()), getUrl("https://poloniex.com/public?command=returnOrderBook&depth=10&currencyPair=" + a.toUpperCase() + "_" + b.toUpperCase())], function(err, results){
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
    guess = space * volumn[0];
    return {
      dir: ma + " " + dir + " " + mb,
      buyCost: buyCost[0],
      sellEarn: sellEarn[0],
      space: space,
      spaceR: space / buyCost[0],
      volumn: volumn,
      guess: guess,
      realCost: buyCost[0] * volumn[0],
      earnP: guess / (buyCost[0] * volumn[0])
    };
  };
  /*
  (err, storage) <- updatePrice
  if err
      return console.log err
  
  info = checkBuy storage, 'binance', 'huobi'
  console.log info
  
  info = checkBuy storage, 'binance', 'huobi', '<'
  console.log info
  
  info = checkBuy storage, 'binance', 'bittrex'
  console.log info
  
  info = checkBuy storage, 'binance', 'bittrex', '<'
  console.log info
  
  info = checkBuy storage, 'binance', 'poloniex'
  console.log info
  
  info = checkBuy storage, 'binance', 'poloniex', '<'
  console.log info
  
  info = checkBuy storage, 'huobi', 'bittrex'
  console.log info
  
  info = checkBuy storage, 'huobi', 'bittrex', '<'
  console.log info
  
  info = checkBuy storage, 'huobi', 'poloniex'
  console.log info
  
  info = checkBuy storage, 'huobi', 'poloniex', '<'
  console.log info
  
  info = checkBuy storage, 'bittrex', 'poloniex'
  console.log info
  
  info = checkBuy storage, 'bittrex', 'poloniex', '<'
  console.log info
  
  */
  x$ = markets = ['binance', 'huobi', 'bittrex', 'poloniex'];
  x$.sort();
  total = [0, 0];
  setInterval(function(){
    return updatePrice('btc', 'xrp', function(err, storage){
      var orders, ma, mb, earn;
      if (err) {
        return console.log(err);
      }
      orders = partialize$.apply(Array.prototype.filter, [
        Array.prototype.filter.call, [
          void 8, function(info){
            return info.spaceR > 0;
          }
        ], [0]
      ])(
      partialize$.apply(Array.prototype.map, [
        Array.prototype.map.call, [
          void 8, function(args){
            return checkBuy.apply(null, [storage].concat(args));
          }
        ], [0]
      ])(
      partialize$.apply(Array.prototype.map, [
        Array.prototype.map.call, [
          void 8, function(arg$){
            var ma, mb;
            ma = arg$[0], mb = arg$[1];
            return [ma, mb, ma > mb ? '>' : '<'];
          }
        ], [0]
      ])(
      (function(){
        var i$, ref$, len$, j$, ref1$, len1$, results$ = [];
        for (i$ = 0, len$ = (ref$ = markets).length; i$ < len$; ++i$) {
          ma = ref$[i$];
          for (j$ = 0, len1$ = (ref1$ = markets).length; j$ < len1$; ++j$) {
            mb = ref1$[j$];
            if (ma !== mb) {
              results$.push([ma, mb]);
            }
          }
        }
        return results$;
      }()))));
      if (orders.length === 0) {
        return console.log('wait for next');
      }
      earn = partialize$.apply(Array.prototype.reduce, [
        Array.prototype.reduce.call, [
          void 8, function(arg$, info){
            var guess, cost;
            guess = arg$[0], cost = arg$[1];
            return [guess + info.guess, cost + info.buyCost * info.volumn[0]];
          }, [0, 0]
        ], [0]
      ])(
      orders);
      console.log('curr', earn, earn[0] / earn[1]);
      total[0] += earn[0];
      total[1] += earn[1];
      return console.log('total:', total, total[0] / total[1]);
    });
  }, 1000);
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
  function partialize$(f, args, where){
    var context = this;
    return function(){
      var params = slice$.call(arguments), i,
          len = params.length, wlen = where.length,
          ta = args ? args.concat() : [], tw = where ? where.concat() : [];
      for(i = 0; i < len; ++i) { ta[tw[0]] = params[i]; tw.shift(); }
      return len < wlen && len ?
        partialize$.apply(context, [f, ta, tw]) : f.apply(context, ta);
    };
  }
}).call(this);
