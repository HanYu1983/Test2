// Generated by LiveScript 1.6.0
(function(){
  var observer, cfg, apiKey, request, crypto, sendUrl, binanceSignedOption;
  observer = require('./observer');
  cfg = require('./config.json');
  apiKey = require('./private/binanceKey.json');
  request = require('request');
  crypto = require('crypto');
  sendUrl = curry$(function(opt, cb){
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
  binanceSignedOption = function(url, data, method){
    var query, signature, opt;
    data.timestamp = new Date().getTime();
    data.recvWindow = 5000;
    query = Object.keys(data).reduce(function(a, k){
      return a.concat([k + "=" + encodeURIComponent(data[k])]);
    }, []).join("&");
    signature = crypto.createHmac('sha256', apiKey.SecretKey).update(query).digest('hex');
    return opt = {
      url: url + '?' + query + '&signature=' + signature,
      qs: data,
      method: method,
      timeout: 5000,
      headers: {
        'Content-type': "application/x-www-form-urlencoded",
        'X-MBX-APIKEY': apiKey.ApiKey
      }
    };
  };
  observer.observe(cfg, 'xrpbtc', function(history){
    return sendUrl(binanceSignedOption("https://api.binance.com/api/v3/order/test", {
      symbol: "BTCUSDT",
      side: "SELL",
      type: "MARKET",
      quantity: 1
    }, "POST"), function(err, result){
      return console.log(err, result);
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
