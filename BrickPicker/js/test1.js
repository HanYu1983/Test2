// Generated by LiveScript 1.6.0
(function(){
  var request, async, crypto, WebSocket, pako, signalR, zlib, ApiKey, getUrl, binanceApiOption, binanceSignedOption;
  request = require('request');
  async = require('async');
  crypto = require('crypto');
  WebSocket = require('ws');
  pako = require('pako');
  signalR = require('signalr-client');
  zlib = require('zlib');
  ApiKey = require('./private/apiKey');
  console.log(ApiKey);
  getUrl = curry$(function(opt, cb){
    var callback;
    callback = function(error, response, body){
      if (error) {
        return cb(error);
      } else {
        return cb(null, body);
      }
    };
    return request(opt, callback);
  });
  binanceApiOption = function(url, data, method){
    var query, opt;
    query = Object.keys(data).reduce(function(a, k){
      return a.concat([k + "=" + encodeURIComponent(data[k])]);
    }, []).join("&");
    return opt = {
      url: url + '?' + query,
      qs: data,
      method: method,
      timeout: 5000,
      headers: {
        'Content-type': "application/x-www-form-urlencoded",
        'X-MBX-APIKEY': ApiKey.binance.ApiKey
      }
    };
  };
  binanceSignedOption = function(url, data, method){
    var query, signature, opt;
    data.timestamp = new Date().getTime();
    data.recvWindow = 5000;
    query = Object.keys(data).reduce(function(a, k){
      return a.concat([k + "=" + encodeURIComponent(data[k])]);
    }, []).join("&");
    signature = crypto.createHmac('sha256', ApiKey.binance.SecretKey).update(query).digest('hex');
    return opt = {
      url: url + '?' + query + '&signature=' + signature,
      qs: data,
      method: method,
      timeout: 5000,
      headers: {
        'Content-type': "application/x-www-form-urlencoded",
        'X-MBX-APIKEY': ApiKey.binance.ApiKey
      }
    };
  };
  /*
  (err, {bids, asks}) <- getUrl 'https://api.binance.com/api/v1/depth?symbol=BTCUSDT'
  console.log(bids)
  sell1 = parseFloat(asks[0][0])
  
  
  (err, {tick:{bids, asks}}) <- getUrl 'https://api.huobipro.com/market/depth?symbol=btcusdt&type=step0'
  console.log(bids)
  buy1 = parseFloat(bids[0][0])
  
  console.log(buy1, sell1)
  
  
  sellPrice = buy1
  buyPrice = sell1
  
  if sellPrice > buyPrice
      console.log 'you can move!'
  else
      console.log 'can not move!'
  */
  /*
  (err, body) <- getUrl 'https://api.binance.com/api/v3/ticker/bookTicker?symbol=BTCUSDT'
  console.log(body)
  
  (err, body) <- getUrl 'https://api.huobipro.com/market/detail?symbol=btcusdt'
  console.log(body)
  
  (err, body) <- getUrl 'https://api.huobipro.com/market/depth?symbol=btcusdt&type=step1'
  console.log(body)
  */
  /*
  updatePrice = (cb) ->
      (err, results) <- async.parallel [
          getUrl 'https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT'
          getUrl 'https://api.binance.com/api/v3/ticker/bookTicker?symbol=BTCUSDT'
          getUrl 'https://api.huobipro.com/market/detail?symbol=btcusdt'
          getUrl 'https://api.huobipro.com/market/depth?symbol=btcusdt&type=step1'
      ]
      [binanceNow, binanceBid, huobiNow, huobiBid] = results
      console.log binanceNow.price
      console.log binanceBid.bidPrice
      console.log huobiNow.tick.close
      console.log huobiBid.tick.bids[0][0]
  */
  getUrl(binanceSignedOption("https://api.binance.com/api/v3/account", {}, "GET"), function(err, result){
    console.log(err, result);
    return getUrl(binanceApiOption("https://api.binance.com/api/v1/userDataStream", {}, "POST"), function(err, result){
      var listenKey, x$, binanceWs;
      listenKey = JSON.parse(
      result).listenKey;
      console.log(err, result);
      x$ = binanceWs = new WebSocket("wss://stream.binance.com:9443/ws/" + listenKey);
      x$.on('open', function(){
        return console.log('open');
      });
      x$.on('close', function(){
        return console.log('close');
      });
      x$.on('message', function(data){
        return console.log(JSON.parse(data));
      });
      x$.on('error', function(err){
        return console.log(err);
      });
      return x$;
    });
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
