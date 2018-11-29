window.onload = ->
    # console = chrome.extension.getBackgroundPage().console
    vueModel = new Vue do
        el: '#app'
        data:
            input:
                addStock: "2330"
            stockInfo: {}
        methods:
            clickAddStock: (id)->
                (w) <- chrome.windows.getCurrent
                (t) <- chrome.tabs.create do
                    windowId: w.id
                    url: "https://goodinfo.tw/StockInfo/ShowK_Chart.asp?STOCK_ID=#{id}&CHT_CAT2=DATE"
                    
            clickCompute: (id, earnRate, count)->
                start = Math.max 0, vueModel.stockInfo[id].rows.length - count
                rows = vueModel.stockInfo[id].rows.slice(start, vueModel.stockInfo[id].rows.length)
                this.stockInfo[id].compute.earnRate = earnRate
                this.stockInfo[id].compute.count = count
                this.stockInfo[id].compute.result = checkLowHighEarn earnRate, rows
                <- save
    
    save = (cb)->
        model = JSON.parse(JSON.stringify(vueModel.$data))
        chrome.storage.local.set {"stockInfo":model.stockInfo}, cb
    
    load = (cb)->
        (obj) <- chrome.storage.local.get null
        for k, v of obj.stockInfo
            if not v.compute
                # 加入result = null讓vue知道這個欄位必須監聽
                v.compute = {earnRate: 0.01, count: 20, result: null}
            start = Math.max 0, v.rows.length - v.compute.count
            rows = v.rows.slice(start, v.rows.length)
            v.result = checkLowHighEarn v.compute.earnRate, rows
        vueModel.stockInfo = obj.stockInfo
        
    load()
        
    chrome.extension.onMessage.addListener ({cmd, info}:obj)->
        switch cmd
            | "update" => load()
            | otherwise =>