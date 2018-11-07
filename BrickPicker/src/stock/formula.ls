export Open = (data)->
    data.map(([_, _, open, _])->open)
    
export Close = (data)->
    data.map(([_, _, _, close])->close)

export Low = (data)->
    data.map(([_, low, _, _])->low)

export High = (data)->
    data.map(([_, _, _, _, high])->high)
    
export Mid = (data)->
    mapn ((a, b)->(a+b)/2), (High data), (Low data)

export Volume = (data)->
    data.map(([_, _, _, _, _, volume])->volume)
    
export avg = (data)->
    data.reduce((+), 0)/data.length
        
export MA = (cnt, data)->
    ret = 
        for i in [cnt-1 til data.length]
            avg = data.slice(i-(cnt-1), i+1)
                .reduce(((acc, curr)->acc+curr), 0)/cnt
    [ret[0] for til (cnt-1)].concat ret

export RSV = (cnt, data)->
    ret = 
        for i in [cnt-1 til data.length]
            [openTime, low, open, close, high] = data[i]
            before9k = data.slice i-(cnt-1), i+1
            min9k = Math.min.apply null, before9k.map(([_, low])->low)
            max9k = Math.max.apply null, before9k.map(([_, _, _, _, high])->high)
            rsv = (close - min9k)*100/(max9k - min9k)
    [ret[0] for til (cnt-1)].concat ret

export KD = (data)->
    kline = []
    dline = []
    for i in [0 til data.length]
        rsv = data[i]
        prevK = if i > 0 then kline[i-1] else 50
        prevD = if i > 0 then dline[i-1] else 50
        k = prevK * (2/3) + rsv/3
        d = prevD * (2/3) + k/3
        kline.push(k)
        dline.push(d)
    [kline, dline]

export reductions = (f, i, seq)->
    seq.reduce do
        (acc, v)->
            prev = acc[*-1]
            curr = f(prev, v)
            acc ++ [curr]
        [i]

export mapn = (f, ...args)->
    maxLength = Math.min.apply(null, args.map (.length))
    for i in [0 til maxLength]
        f.apply(null, args.map((ary)->ary[i]))
            
export YuMA = (n, data)->
    if data.length >= n
        fv = data.slice(0, n).reduce((+), 0)/n
        ret = reductions do
            (ma, v)->
                ma*((n-1)/n) + v/n
            fv
            data.slice(n, data.length)
        [ret[0] for til (n - 1)].concat ret

export EMA = (n, data)->
    if data.length >= n
        fv = data.slice(0, n).reduce((+), 0)/n
        alpha = 2/(n+1)
        ret = reductions do
            (ema, v)->
                (v - ema)*alpha + ema
            fv
            data.slice(n, data.length)
        [ret[0] for til (n - 1)].concat ret

export MACD-DIF = (n, m, data)->
    mapn (-), EMA(n, data), EMA(m, data)

export MACD-DEM = EMA


export BBI = (n, m, o, p, data)->
    l1 = MA n, data
    l2 = MA m, data
    l3 = MA o, data
    l4 = MA p, data
    mapn do
        (...args)->
            args.reduce((+), 0)/args.length
        l1, l2, l3, l4

export EBBI = (n, m, o, p, data)->
    l1 = EMA n, data
    l2 = EMA m, data
    l3 = EMA o, data
    l4 = EMA p, data
    mapn do
        (...args)->
            args.reduce((+), 0)/args.length
        l1, l2, l3, l4
        
export AccDist = (data)->
    ret = reductions do
        (+)
        0
        data.map ([_, low, open, close, high, volume])->
            if high == low
                0
            else
                ((close - low) - (high - close)) * volume / (high - low)
    # 去掉reductions中產生的初始值0
    ret.slice(1, ret.length)

export Chaikin = (n, m, data)->
    acc = AccDist data
    mapn (-), EMA(n, acc), EMA(m, acc)

export TrueLow = (data)->
    ret = mapn do
        Math.min
        Close data
        Low data.slice(1, data.length)
    (Low [data[0]]).concat ret 

export TrueWave = (data)->
    ret = mapn do
        (close, high, low)->
            Math.max high - low, Math.abs(high - close), Math.abs(low - close)
        Close data
        High(data).slice(1, data.length)
        Low(data).slice(1, data.length)
    (High [data[0]]).concat ret
        
export UOS = (m, n, o, data)->
    tl = TrueLow data
    bp = mapn (-), Close(data), tl
    tr = TrueWave data
    ruo = mapn do
        (b1, b2, b3, t1, t2, t3)->
            (b1 / t1)*4 + (b2 / t2)*2 + b3 / t3
        MA(m, bp).map (* m)
        MA(n, bp).map (* n)
        MA(o, bp).map (* o)
        MA(m, tr).map (* m)
        MA(n, tr).map (* n)
        MA(o, tr).map (* o)
    uos = ruo.map (n)-> n*(100/7)

export Volatility = (data)->
    ret = mapn do
        (prev, curr)->
            (curr - prev)/prev
        data
        data.slice(1, data.length)
    [ret[0]].concat ret

export Trix = (n, m, data)->
    tr = Close data |> EMA n, _ |> EMA n, _ |> EMA n, _
    trix = Volatility tr
    matrix = trix |> EMA m, _
    [trix, matrix]

export StandardDeviation = (avg, data)->
    v1 = data.map((v)-> Math.pow(v - avg, 2)).reduce((+), 0)
    v2 = v1 / (data.length - 1)
    v3 = Math.sqrt v2

export ZScore = (avg, sd, data)->
    data.map (v)->(v - avg)/sd

export YuClock = (startN, n, data)->
    ps = data |> Close _ |> MA n, _
    ps-avg = avg ps.slice(0, startN)
    ps-sd = StandardDeviation ps-avg, ps.slice(0, startN)
    ps-z = ZScore ps-avg, ps-sd, ps
    
    vs = data |> Volume _ |> MA n, _ 
    vs-avg = avg vs.slice(0, startN)
    vs-sd = StandardDeviation vs-avg, vs.slice(0, startN)
    vs-z = ZScore vs-avg, vs-sd, vs
    
    ps = mapn (...args)->args, vs-z, ps-z
    
    axis = [0.707, 0.707]
    
    dot = (l1, l2)-> mapn((*), l1, l2).reduce((+), 0)
    
    length = (line)-> dot(line, line) |> Math.sqrt _
    
    normalize = (line)-> 
        len = length line
        if len == 0
            line
        else
            line.map (/ len)
    
    projs = mapn do
        (prev, curr)->
            dir = mapn (-), curr, prev
            dir |> normalize _ |> dot _, axis
        ps
        ps.slice(1, ps.length)
    
    [projs[0]].concat projs