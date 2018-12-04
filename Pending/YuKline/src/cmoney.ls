require! {
    'prelude-ls': P
}



delay = (t)->
    new Promise (res, rej)->
        setTimeout res, t

query = ->
    info1 = let
        doms = $('#InventoryDetailListInfo > tr')
        for dom in doms
            [idDom] = $(dom).find("td > span")
            [$(idDom).text()]

    info2 = let
        doms = $('#InventoryDetailListData > tr')
        for dom in doms
            [stateDom, avgDom]:datas = $(dom).find("td")
            [_, stockNum, txNum] = /(\d+) ?張\/ ?(\d+) ?張/g.exec($(stateDom).text())
            avg = $(avgDom).text() |> parseFloat _
            stockNum = parseFloat stockNum
            txNum = parseFloat txNum
            [stockNum, txNum, avg]
    
    info = P.zip-with (++), info1, info2

async function sell
    infos = query()
    for info in infos
        [id, _, txCnt, avg] = info
        # 填股票代號
        $('#textBoxCommkey').focus().val(id)
        # 切換focus觸發change事件
        $('#TextBoxQty').focus()
        # 等待原網頁的js處理
        await delay 500
    
        # 點選賣
        $('#Bs_S')[0].checked = true
        $('#Bs_S')[0].dispatchEvent(new Event("change"))
        # 等待原網頁的js處理
        await delay 500
    
        # 填價格
        $('#TextBoxPrice').focus().val(avg*1.01)
        # 填數量
        $('#TextBoxQty').focus().val(txCnt)
    
        await delay 1000
        $('#Orderbtn').click()

async function test
    await delay 2000
    $('a[href="#ID"]')[0].dispatchEvent(new Event("click"))
    await delay 2000
    await sell()

chrome.runtime.onMessage.addListener ({ask}:message, sender, sendResponse)->
    if ask == "sell all"
        sell()
        sendResponse(true)