# can not use require because this is frondend code

mapn = (f, ...args)->
    minLength = Math.min.apply(null, args.map (.length))
    for i in [0 til minLength]
        f.apply(null, args.map((ary)->ary[i]))

export drawer = ->
    
            
    Open = (data)->
        data.map(([_, _, open, _])->open)

    Close = (data)->
        data.map(([_, _, _, close])->close)

    Low = (data)->
        data.map(([_, low, _, _])->low)

    High = (data)->
        data.map(([_, _, _, _, high])->high)

    Mid = (data)->
        mapn ((a, b)->(a+b)/2), (High data), (Low data)

    Volume = (data)->
        data.map(([_, _, _, _, _, volume])->volume)
    
    max-v = ({type, line, kline})->
        switch type
            | "grid", "kline" =>
                Math.max.apply null, (High kline)
            | "line" =>
                Math.max.apply null, line
            | otherwise =>
                0
    
    min-v = ({type, line, kline})->
        switch type
            | "grid", "kline" =>
                Math.min.apply null, (Low kline)
            | "line" =>
                Math.min.apply null, line
            | otherwise =>
                0
    
    length = ({type, line, kline})->
        switch type
            | "grid"=>
                if line
                    return line.length
                kline.length
            | "line" =>
                line.length
            | "kline" =>
                kline.length
            | otherwise =>
                0
    
    graphic-base = (w, h, drawers)->
        max = Math.max.apply null, drawers.map(max-v)
        min = Math.min.apply null, drawers.map(min-v)
        len = Math.max.apply null, drawers.map(length)
        offset = max - min
        offset-x = w/ (len + 1)
        pos-y = (v)->
            (1 - (v - min)/ offset) * h
        [w, h, max, min, offset, offset-x, pos-y]
    
    draw-it = ({type, line, color, offset, hideY, style, kline, info, centerY}, base, ctx)->
        [w, h, max-v, min-v, offset-v, offset-x, pos-y] = base
        switch type
            | "grid" =>
                cnt = 10
                cntx = 5
                offset = (max-v - min-v)/ cnt
                ctx
                    ..strokeStyle = color
                    ..fillStyle = color
                    ..lineWidth = 1
                    ..beginPath()
                    
                if not hideY
                    for i in [0 til cnt]
                        v = -(centerY || min-v) - (i * offset)
                        y = pos-y(v)
                        ctx
                            ..fillText "#{v}", w*1/3, y
                            ..fillText "#{v}", w*2/3, y
                            ..moveTo 0, y
                            ..lineTo w, y
                        v = (centerY || min-v) + (i * offset)
                        y = pos-y(v)
                        ctx
                            ..fillText "#{v}", w*1/3, y
                            ..fillText "#{v}", w*2/3, y
                            ..moveTo 0, y
                            ..lineTo w, y
                if kline
                    for i in [0 til kline.length]
                        if i % cntx == 0
                            posx = offset-x/2 + i* offset-x
                            ctx
                                ..moveTo posx, 0
                                ..lineTo posx, h
                
                if line
                    for i in [0 til line.length]
                        if i % cntx == 0
                            posx = offset-x/2 + i* offset-x
                            ctx
                                ..moveTo posx, 0
                                ..lineTo posx, h
                ctx.stroke()
                
            | "line" =>
                offset = (offset || 0)
                style = (style || "")
                ctx
                    ..strokeStyle = color
                    ..lineWidth = 1
                switch style
                    | "dot" =>
                        ctx.fillStyle = color
                        for [idx, value] in mapn (...argv)->argv, [1 to line.length], line
                            ctx
                                ..beginPath()
                                ..arc (idx + offset) * offset-x, pos-y(value), offset-x / 4, 0, 6.28, false
                                ..closePath()
                                ..fill()
                                
                    | otherwise =>
                        ctx.beginPath()
                        for [idx, prev, curr] in mapn (...argv)->argv, [1 to line.length], line, line.slice(1, line.length)
                            ctx
                                ..moveTo (idx + offset) * offset-x, pos-y(prev)
                                ..lineTo (idx + offset + 1) * offset-x, pos-y(curr)
                        ctx.stroke()
            | "kline" =>
                ctx.fillStyle = "black"
                for [idx, [date, low, close, open, high, volume] info] in mapn (...args)->args, [1 to kline.length], kline, (info || kline)
                    ctx
                        ..strokeStyle = if close > open then "red" else "green"
                        ..lineWidth = 2
                        ..beginPath()
                        ..moveTo idx* offset-x, pos-y(low)
                        ..lineTo idx* offset-x, pos-y(high)
                        ..stroke()
                        
                        ..strokeStyle = if close > open then "red" else "green"
                        ..lineWidth = offset-x
                        ..beginPath()
                        ..moveTo idx* offset-x, pos-y(open)
                        ..lineTo idx* offset-x, pos-y(high) + 1
                        ..lineTo idx* offset-x, pos-y(close)
                        ..stroke()
    return
        graphic-base: graphic-base
        draw-it: draw-it