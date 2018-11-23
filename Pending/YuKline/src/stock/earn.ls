
export checkSignal = (line, buyLine, sellLine, data)->
    orders = []
    for i in [0 til line.length]
        prevL = line[i-1]
        prevB = buyLine[i-1]
        prevS = sellLine[i-1]
        
        l = line[i]
        b = buyLine[i]
        s = sellLine[i]
        
        if prevL <= prevB && l > b && i<line.length
            [date, _, open, close, _] = data[i]
            buyPrice = open #(open + close)/2
            orders.push do
                action:"buy"
                price:buyPrice
                date:date
                idx:i
                
        if prevL >= prevS && l < s && i<line.length
            [date, _, open, close, _] = data[i]
            buyPrice = (open + close)/2
            orders.push do
                action:"sell"
                price:buyPrice
                date:date
                idx:i
    orders

export checkEarn = (data, orders)->
    storage = 0
    money = 0
    useMoney = 0
    rate = []
    gas = 0.001425
    for order in orders
        if order.idx >= data.length - 1
            break
            
        if order.action == "buy"    
            if storage != 0
                console.log "has storage"
            else
                [date, low, open, close, high] = data[order.idx+1]
                /*
                if order.price < low || order.price > high
                    continue
                price = order.price
                */
                price = open
                cost = price + price * gas
                money -= cost
                useMoney = cost
                storage = price
                
        if order.action == "sell"
            if storage == 0
                console.log "no storage"
            else
                [date, low, open, close, high] = data[order.idx+1]
                price = open
                #price = order.price
                earn = price - price * gas
                money += earn
                earnRate = ((earn - useMoney)/useMoney)
                storage = 0
                rate.push earnRate
                
    earnRateAvg = rate.reduce(((a,b)->a+b), 0)/rate.length
    transactionTime = rate.length
    useMoneyPerTranaction = 100000
    totalEarn = (useMoneyPerTranaction*earnRateAvg)*transactionTime
    totalEarnRate = (totalEarn + useMoneyPerTranaction)/useMoneyPerTranaction
    
    return
        price: storage
        amount: if storage != 0 then useMoneyPerTranaction/storage else 0
        moneyFlow: money + storage
        ratePerTx: earnRateAvg
        earn: totalEarn
        earnRate: totalEarnRate
        times: rate.length
        

export checkEarn2 = (orders)->
    storage = []
    money = 0
    rate = []
    gas = 0.001425
    for order in orders
        if order.action == "buy"
            price = order.price
            cost = price + price * gas
            money -= cost
            storage.push cost
        
        if order.action == "sell"
            if storage.length == 0
                console.log "no storage"
            else
                for useMoney in storage
                    price = order.price
                    earn = price - price * gas
                    money += earn
                    earnRate = ((earn - useMoney)/useMoney)
                    rate.push earnRate
                storage = []
        
    earnRateAvg = rate.reduce(((a,b)->a+b), 0)/rate.length
    transactionTime = rate.length
    useMoneyPerTranaction = 100000
    totalEarn = (useMoneyPerTranaction*earnRateAvg)*transactionTime
    totalEarnRate = (totalEarn + useMoneyPerTranaction)/useMoneyPerTranaction

    return
        price: storage.reduce((+), 0)
        amount: storage.length
        moneyFlow: money + storage.reduce((+), 0)
        ratePerTx: earnRateAvg
        earn: totalEarn
        earnRate: totalEarnRate
        times: rate.length
        

export checkStyle = (origin)->
    if origin.length < 2
        return 0
    total = 0
    for i in [1 til origin.length]
        prev = origin[i - 1][3]
        curr = origin[i][3]
        if prev == 0
            continue
        rate = (curr - prev)/prev
        total += rate
    total


export checkLowHighEarn = (earnRate, stockData)->
    stocks = []
    tx = []
    for day in stockData
        if stocks.length == 0
            stocks.push day
        else
            [_, low, open, close, high] = day
            sellOk = false
            
            for i in [0 til stocks.length].reverse()
                [_, _, prevOpen, _, _] = stocks[i]
                rate = (open - prevOpen) / prevOpen
                if rate >= earnRate
                    tx.push [stocks[i], day]
                    stocks = stocks.slice(0, i) ++ stocks.slice(i+1, stocks.length)
                    sellOk = true
            
            if sellOk == false
                stocks.push day
    
    buyPrice = tx.map(([first])->first) |> Open
    buyAvg = buyPrice |> avg _
    buySd = buyPrice |> StandardDeviation buyAvg, _
    buyZ = buyPrice |> ZScore buyAvg, buySd, _
    
    sellPrice = tx.map(([_, second])->second) |> Open
    sellAvg = sellPrice |> avg _
    sellSd = sellPrice |> StandardDeviation sellAvg, _
    sellZ = sellPrice |> ZScore sellAvg, sellSd, _
    
    [lastKey, _, lastOpen] = stockData[*-1]
    lastZ1 = (lastOpen - buyAvg) / buySd
    lastZ2 = (lastOpen - sellAvg) / sellSd
    
    min = max = 0
    if stocks.length > 0
        min = stocks |> Open |> Math.min.apply null, _ 
        max = stocks |> Open |> Math.max.apply null, _
 
    txRate = tx.length/(tx.length + stocks.length)
    
    ret =
        txRate: txRate
        earnRate: Math.pow(((earnRate) - 0.001425)+1, tx.length* txRate)
        maxEarnRate: Math.pow(((earnRate) - 0.001425)+1, tx.length)
        check:{
            min: min
            max: max
            rate: if min != 0 then (max - min)/min else 0
        }
        buyPrice:{
            avg: buyAvg
            sd: buySd
            z: buyZ
        }
        sellPrice:{
            avg: sellAvg
            sd: sellSd
            z: sellZ
        }
        last:{
            key: lastKey
            open: lastOpen
            buyZ: lastZ1
            sellZ : lastZ2
        }
        stocks: stocks
        tx: tx