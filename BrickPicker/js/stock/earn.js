// Generated by LiveScript 1.6.0
(function(){
  var checkSignal2, checkSignal, checkEarn, checkEarn2, out$ = typeof exports != 'undefined' && exports || this;
  checkSignal2 = function(line1, line2, data){
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
  out$.checkSignal = checkSignal = function(line, buyLine, sellLine, data){
    var orders, i$, ref$, len$, i, prevL, prevB, prevS, l, b, s, date, open, buyPrice;
    orders = [];
    for (i$ = 0, len$ = (ref$ = (fn$())).length; i$ < len$; ++i$) {
      i = ref$[i$];
      prevL = line[i - 1];
      prevB = buyLine[i - 1];
      prevS = sellLine[i - 1];
      l = line[i];
      b = buyLine[i];
      s = sellLine[i];
      if (prevL <= prevB && l > b && i < line.length - 1) {
        date = data[i][0];
        open = data[i][2];
        buyPrice = open;
        orders.push({
          action: "buy",
          price: buyPrice,
          date: date
        });
      }
      if (prevL >= prevS && l < s && i < line.length - 1) {
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
      for (i$ = 0, to$ = line.length; i$ < to$; ++i$) {
        results$.push(i$);
      }
      return results$;
    }
  };
  out$.checkEarn = checkEarn = function(orders){
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
  out$.checkEarn2 = checkEarn2 = function(orders){
    var storage, money, rate, gas, i$, len$, order, price, cost, j$, len1$, useMoney, earn, earnRate, earnRateAvg, transactionTime, useMoneyPerTranaction, totalEarn, totalEarnRate;
    storage = [];
    money = 0;
    rate = [];
    gas = 0.001425;
    for (i$ = 0, len$ = orders.length; i$ < len$; ++i$) {
      order = orders[i$];
      if (order.action === "buy") {
        price = order.price;
        cost = price + price * gas;
        money -= cost;
        storage.push(cost);
      }
      if (order.action === "sell") {
        if (storage.length === 0) {
          console.log("no storage");
        } else {
          for (j$ = 0, len1$ = storage.length; j$ < len1$; ++j$) {
            useMoney = storage[j$];
            price = order.price;
            earn = price - price * gas;
            money += earn;
            earnRate = (earn - useMoney) / useMoney;
            rate.push(earnRate);
          }
          storage = [];
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
      price: storage.reduce(curry$(function(x$, y$){
        return x$ + y$;
      }), 0),
      amount: storage.length,
      moneyFlow: money + storage.reduce(curry$(function(x$, y$){
        return x$ + y$;
      }), 0),
      ratePerTx: earnRateAvg,
      earn: totalEarn,
      earnRate: totalEarnRate,
      times: rate.length
    };
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
