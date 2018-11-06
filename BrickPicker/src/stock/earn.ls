
checkSignal2 = (line1, line2, data)->
    orders = []
    for i in [0 til line1.length]
        prevK = line1[i-1]
        prevD = line2[i-1]
        k = line1[i]
        d = line2[i]
        if prevK <= prevD && k > d && i<line1.length-1
            date = data[i][0]
            open = data[i][2]
            buyPrice = open
            orders.push do
                action:"buy",
                price:buyPrice,
                date:date
        if prevK >= prevD && k < d && i<line1.length-1
            date = data[i][0]
            open = data[i][2]
            buyPrice = open
            orders.push do
                action:"sell",
                price:buyPrice,
                date:date
    orders

export checkSignal = (line, buyLine, sellLine, data)->
    orders = []
    for i in [0 til line.length]
        prevL = line[i-1]
        prevB = buyLine[i-1]
        prevS = sellLine[i-1]
        
        l = line[i]
        b = buyLine[i]
        s = sellLine[i]
        
        if prevL <= prevB && l > b && i<line.length-1
            date = data[i][0]
            open = data[i][2]
            buyPrice = open
            orders.push do
                action:"buy",
                price:buyPrice,
                date:date
                
        if prevL >= prevS && l < s && i<line.length-1
            date = data[i][0]
            open = data[i][2]
            buyPrice = open
            orders.push do
                action:"sell",
                price:buyPrice,
                date:date
    orders

export checkEarn = (orders)->
    storage = 0
    money = 0
    useMoney = 0
    rate = []
    gas = 0.001425
    for order in orders
        if order.action == "buy"
            if storage != 0
                console.log "has storage"
            else
                price = order.price
                cost = price + price * gas
                money -= cost
                useMoney = cost
                storage = price
                
        if order.action == "sell"
            if storage == 0
                console.log "no storage"
            else
                price = order.price
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