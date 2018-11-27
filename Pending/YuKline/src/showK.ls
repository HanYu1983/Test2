delay = (t)->
    new Promise (res, rej)->
        setTimeout res, t

async function main
    # 等待資料讀取
    await delay 3000

    # 修改範圍
    $('#selK_ChartPeriod').val("365")

    # 發送修改事件觸發網頁的js
    # 不能透過jquery來trigger，因為這是在sendbox，必須直接操做dom
    # https://stackoverflow.com/questions/17819344/triggering-a-click-event-from-content-script-chrome-extension
    $('#selK_ChartPeriod')[0].dispatchEvent(new Event("change"))
    
    # 等待資料讀取
    await delay 5000

    # 取得資料
    rows = let
        table = $('#divK_ChartDetail > div > div > table')
        rows = table.find("tbody > tr")
        parse = 
            for row in rows
                [date, open, high, low, close, _, _, volume] = $(row).find("td")
                [date, low, open, close, high, volume].map (dom)->
                    $(dom).find("nobr").text().replace(/,/, "")
        parse.reverse().map ([d, ...vs])-> [d] ++ vs.map(parseFloat)
    
    console.log rows

    [id, name] = let 
        origin = $("body > table:nth-child(6) > tbody > tr > td:nth-child(3) > table:nth-child(1) > tbody > tr:nth-child(1) > td:nth-child(1) > table > tbody > tr:nth-child(1) > td > table > tbody > tr > td:nth-child(1) > nobr > a")
        if origin.length == 0
            origin = $("body > table:nth-child(5) > tbody > tr > td:nth-child(3) > table:nth-child(1) > tbody > tr:nth-child(1) > td:nth-child(1) > table > tbody > tr:nth-child(1) > td > table > tbody > tr > td:nth-child(1) > nobr > a")
        origin.text().replace(/\s/, " ").split(" ")
    
    chrome.storage.local.get null, (model)->
        if not model.stockInfo
            model.stockInfo = {}
        
        model.stockInfo[id] = {name: name, rows: rows}
        chrome.storage.local.set {"stockInfo": model.stockInfo}, ->
            chrome.extension.sendMessage {cmd:'update'}

main()