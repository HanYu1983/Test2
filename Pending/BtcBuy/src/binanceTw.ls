console.log "tw"

# 測試用
# 確定可以注入websocket的程式碼
ws = new WebSocket 'wss://stream.binance.com:9443/ws/xrpbtc@trade'
    ..onopen = (evt) ->
        console.log evt

    ..onclose = (evt) ->
        console.log evt
    
    ..onmessage = ({data}) ->
        {p} = JSON.parse(data)
        console.log p
        
    ..onerror = (evt) ->
        console.log evt

delay = (t) ->
    new Promise (res, rej)->
        setTimeout res, t

click = (elem)->
    $(elem).click()

scroll = (elem, pos)->
    $(elem).scrollTop pos
    
async function main
    await delay 1000
    
    # 用來查是哪一個物件在scroll，有scroll才會印出東西
    # 這個範例中確定是document在scroll
    scrollTarget = $(document).first()
    scrollTarget.scroll ->
        console.log scrollTarget.scrollTop()
    
    # 用contains來尋找顯示在瀏灠器中（使用者見到的）的文字(text)
    # 並在console中確定是第2個
    # 要用cnotains的原因是這個網頁的很多用來尋找的標記(ex. class, id)不是欠缺就是會亂數產生，從而讓尋找失效
    # 所以才使用顯示上必定不會更動的文字來找
    click $('#tabContainer').find('div:contains("BTC")')[2]
    
    await delay 100
    
    # 滑動視窗
    scroll document, 4619
    
    await delay 500
    
    rows = $('div[role="row"]')
    infos = for row in rows.slice(1, rows.length) # jquery出來的array用map沒有效果
        grids = $(row).find('div[role="gridcell"]')
        console.log grids[0]
        [_, marketDom, _, priceDom, _, _, volume] = grids
        $(marketDom).text()
        #price = $(priceDom).find('div > span').first().text() |> parseFloat _
    console.log infos

main()