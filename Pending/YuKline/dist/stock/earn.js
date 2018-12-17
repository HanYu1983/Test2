// Generated by LiveScript 1.6.0
(function(){
  var checkSignal, checkEarn, checkEarn2, checkStyle, checkLowHighEarn, out$ = typeof exports != 'undefined' && exports || this;
  out$.checkSignal = checkSignal = function(line, buyLine, sellLine, data){
    var orders, i$, ref$, len$, i, prevL, prevB, prevS, l, b, s, ref1$, date, _, open, close, buyPrice;
    orders = [];
    for (i$ = 0, len$ = (ref$ = (fn$())).length; i$ < len$; ++i$) {
      i = ref$[i$];
      prevL = line[i - 1];
      prevB = buyLine[i - 1];
      prevS = sellLine[i - 1];
      l = line[i];
      b = buyLine[i];
      s = sellLine[i];
      if (prevL <= prevB && l > b && i < line.length) {
        ref1$ = data[i], date = ref1$[0], _ = ref1$[1], open = ref1$[2], close = ref1$[3], _ = ref1$[4];
        buyPrice = open;
        orders.push({
          action: "buy",
          price: buyPrice,
          date: date,
          idx: i
        });
      }
      if (prevL >= prevS && l < s && i < line.length) {
        ref1$ = data[i], date = ref1$[0], _ = ref1$[1], open = ref1$[2], close = ref1$[3], _ = ref1$[4];
        buyPrice = (open + close) / 2;
        orders.push({
          action: "sell",
          price: buyPrice,
          date: date,
          idx: i
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
  out$.checkEarn = checkEarn = function(data, orders){
    var storage, money, useMoney, rate, gas, i$, len$, order, ref$, date, low, open, close, high, price, cost, earn, earnRate, earnRateAvg, transactionTime, useMoneyPerTranaction, totalEarn, totalEarnRate;
    storage = 0;
    money = 0;
    useMoney = 0;
    rate = [];
    gas = 0.001425;
    for (i$ = 0, len$ = orders.length; i$ < len$; ++i$) {
      order = orders[i$];
      if (order.idx >= data.length - 1) {
        break;
      }
      if (order.action === "buy") {
        if (storage !== 0) {
          console.log("has storage");
        } else {
          ref$ = data[order.idx + 1], date = ref$[0], low = ref$[1], open = ref$[2], close = ref$[3], high = ref$[4];
          /*
          if order.price < low || order.price > high
              continue
          price = order.price
          */
          price = open;
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
          ref$ = data[order.idx + 1], date = ref$[0], low = ref$[1], open = ref$[2], close = ref$[3], high = ref$[4];
          price = open;
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
  out$.checkStyle = checkStyle = function(origin){
    var total, i$, ref$, len$, i, prev, curr, rate;
    if (origin.length < 2) {
      return 0;
    }
    total = 0;
    for (i$ = 0, len$ = (ref$ = (fn$())).length; i$ < len$; ++i$) {
      i = ref$[i$];
      prev = origin[i - 1][3];
      curr = origin[i][3];
      if (prev === 0) {
        continue;
      }
      rate = (curr - prev) / prev;
      total += rate;
    }
    return total;
    function fn$(){
      var i$, to$, results$ = [];
      for (i$ = 1, to$ = origin.length; i$ < to$; ++i$) {
        results$.push(i$);
      }
      return results$;
    }
  };
  out$.checkLowHighEarn = checkLowHighEarn = function(earnRate, stockData){
    var stocks, tx, i$, len$, day, _, low, open, close, high, sellOk, j$, ref$, len1$, i, ref1$, prevOpen, rate, buyPrice, buyAvg, buySd, buyZ, sellPrice, sellAvg, sellSd, sellZ, lastKey, lastOpen, lastZ1, lastZ2, min, max, txRate, txFee, txDurAvg, pos, ret;
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
          rate = (open - prevOpen) / prevOpen;
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
    buyPrice = Open(
    tx.map(function(arg$){
      var first;
      first = arg$[0];
      return first;
    }));
    buyAvg = avg(buyPrice);
    buySd = StandardDeviation(buyAvg, buyPrice);
    buyZ = ZScore(buyAvg, buySd, buyPrice);
    sellPrice = Open(
    tx.map(function(arg$){
      var _, second;
      _ = arg$[0], second = arg$[1];
      return second;
    }));
    sellAvg = avg(sellPrice);
    sellSd = StandardDeviation(sellAvg, sellPrice);
    sellZ = ZScore(sellAvg, sellSd, sellPrice);
    ref$ = stockData[stockData.length - 1], lastKey = ref$[0], _ = ref$[1], lastOpen = ref$[2];
    lastZ1 = (lastOpen - buyAvg) / buySd;
    lastZ2 = (lastOpen - sellAvg) / sellSd;
    min = max = 0;
    if (stocks.length > 0) {
      min = Math.min.apply(null, Open(
      stocks));
      max = Math.max.apply(null, Open(
      stocks));
    }
    txRate = tx.length / (tx.length + stocks.length);
    txFee = 0.00142748091;
    txDurAvg = (function(){
      var difs;
      difs = tx.map(function(arg$){
        var t1, t2, d1, d2, day;
        t1 = arg$[0][0], t2 = arg$[1][0];
        d1 = new Date(t1);
        d2 = new Date(t2);
        return day = (d2.getTime() - d1.getTime()) / (1000 * 60 * 60 * 24);
      });
      return difs.reduce(curry$(function(x$, y$){
        return x$ + y$;
      }), 0) / tx.length;
    }.call(this));
    pos = (lastOpen - buyAvg) / (sellAvg - buyAvg);
    return ret = {
      txRate: txRate,
      earnRate: Math.pow((earnRate - txFee) + 1, tx.length * txRate),
      maxEarnRate: Math.pow((earnRate - txFee) + 1, tx.length),
      check: {
        min: min,
        max: max,
        rate: min !== 0 ? (max - min) / min : 0
      },
      buyPrice: {
        avg: buyAvg,
        sd: buySd,
        z: buyZ
      },
      sellPrice: {
        avg: sellAvg,
        sd: sellSd,
        z: sellZ
      },
      last: {
        key: lastKey,
        open: lastOpen,
        buyZ: lastZ1,
        sellZ: lastZ2
      },
      txDurAvg: txDurAvg,
      pos: pos,
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
