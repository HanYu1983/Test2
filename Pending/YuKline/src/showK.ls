<- $


rows = let
    table = $('#divK_ChartDetail > div > div > table')
    rows = table.find("tbody > tr")
    parse = 
        for row in rows
            [date, open, high, low, close, _, _, volume] = $(row).find("td")
            [date, low, open, close, high, volume].map (dom)->
                $(dom).find("nobr").text().replace(/,/, "")
    parse.reverse().map ([d, ...vs])-> [d] ++ vs.map(parseFloat)

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
    
