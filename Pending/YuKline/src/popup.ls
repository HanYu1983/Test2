window.onload = ->
    console = chrome.extension.getBackgroundPage()?.console
    
    vueModel = new Vue do
        el: '#app'
        data:
            input:
                addStock: "2330"
            stockInfo: {}
        methods:
            clickSellAll: ->
                (t) <- chrome.tabs.query {currentWindow: true, active: true}
                <- chrome.tabs.sendMessage t[0].id, { ask: 'sell all' }
                
            clickAddStock: (id)->
                (w) <- chrome.windows.getCurrent
                (t) <- chrome.tabs.create do
                    windowId: w.id
                    url: "https://goodinfo.tw/StockInfo/ShowK_Chart.asp?STOCK_ID=#{id}&CHT_CAT2=DATE"
                    
            clickUpdateStockAll: ->
                (w) <- chrome.windows.getCurrent
                for id, _ of vueModel.stockInfo
                    (t) <- chrome.tabs.create do
                        windowId: w.id
                        url: "https://goodinfo.tw/StockInfo/ShowK_Chart.asp?STOCK_ID=#{id}&CHT_CAT2=DATE"
            
            clickFind: (id)->
                earnRate = 0.01
                
                compute = (count)->
                    start = Math.max 0, vueModel.stockInfo[id].rows.length - count
                    rows = vueModel.stockInfo[id].rows.slice(start, vueModel.stockInfo[id].rows.length)
                    result = checkLowHighEarn earnRate, rows
                    [result.txRate, count, result]
                
                # 計算所有結果
                # 這裡先做排序不影響第二階段的計算
                rets = [1 to 20] |>
                    (.map (* 10)) |>
                    (.map compute) |>
                    (.sort ([txRate1], [txRate2])-> txRate2 - txRate1)
                
                # 計算所有結果的買均價和最後開盤價的碰觸次數
                # 越多次越好，代表有套利空間
                lastOpen = vueModel.stockInfo[id].rows[*-1][2]
                range = lastOpen * 0.01
                hitRets = rets.filter ([_, _, result])->
                    result.buyPrice.avg > (lastOpen - range) && result.buyPrice.avg < (lastOpen + range)
                
                # 先取最優結果
                selectInfo = rets[0]
                hitCount = hitRets.length
                
                # 若有碰觸到最後開盤價的結果優先取出
                if hitCount > 0
                    selectInfo = hitRets[0]
                
                # 記錄結果
                this.stockInfo[id].compute.earnRate = earnRate
                this.stockInfo[id].compute.count = selectInfo[1]
                this.stockInfo[id].compute.result = selectInfo[2]
                this.stockInfo[id].compute.hitCount = hitCount
                <- save
                    
                        
            clickCompute: (id, earnRate, count)->
                start = Math.max 0, vueModel.stockInfo[id].rows.length - count
                rows = vueModel.stockInfo[id].rows.slice(start, vueModel.stockInfo[id].rows.length)
                this.stockInfo[id].compute.earnRate = earnRate
                this.stockInfo[id].compute.count = count
                this.stockInfo[id].compute.result = checkLowHighEarn earnRate, rows
                <- save
    
    save = (cb)->
        model = JSON.parse(JSON.stringify(vueModel.$data))
        # 不存入計算結果，節省空間
        # 每次讀取時會重新計算
        for k, _ of model.stockInfo
            if model.stockInfo[k].compute
                delete model.stockInfo[k].compute.result
        chrome.storage.local.set {"stockInfo":model.stockInfo}, cb
    
    load = (cb)->
        (obj) <- chrome.storage.local.get null
        for k, v of obj.stockInfo
            if not v.compute
                # 加入result = null讓vue知道這個欄位必須監聽
                v.compute = {earnRate: 0.01, count: 20, result: null, hitCount: 0}
            start = Math.max 0, v.rows.length - v.compute.count
            rows = v.rows.slice(start, v.rows.length)
            v.compute.result = checkLowHighEarn v.compute.earnRate, rows
        vueModel.stockInfo = obj.stockInfo
        
    load()
        
    chrome.extension.onMessage.addListener ({cmd, info}:obj)->
        switch cmd
            | "update" => load()
            | otherwise =>