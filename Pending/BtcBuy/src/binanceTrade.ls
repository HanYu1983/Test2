console.log "trade"

dispatchKeyboardEvent = (target, initKeyboradEvent_args)->
    e = document.createEvent("KeyboardEvents")
    e.initKeyboardEvent.apply(e, Array.prototype.slice.call(arguments, 1))
    target.dispatchEvent(e)
    
delay = (t) ->
    new Promise (res, rej)->
        setTimeout res, t

async function main
    await delay 5000
        
    search = $('input[placeholder="查詢 …"]')
    console.log search

    # 假裝用戶焦點到欄位（非必要）
    search.focus()
    # 輸入欄位並產生假事件
    # 但假事件在這個範例中沒起到作用
    search.val("XRP").trigger("keypress")
    
    # 假事件第2種方法，一樣沒起到作用
    e = new KeyboardEvent do
        "keypress"
        bubbles : true
        cancelable : false
        char : "Q"
        key : "q"
        shiftKey : true
        keyCode : 81
    search[0].dispatchEvent(e)
    
    # 假事件第3種方法，一樣沒起到作用
    dispatchMouseEvent search[0], 'click', true, true
    
    
    # 同bianceTw.ls一樣，尋找可見文字
    nowP = $($('div:contains("市價")')[10]).find("li")[1]
    $(nowP).click()
    console.log nowP
    
    await delay 3000
    
    input = $('#FormRow-BUY-quantity')
    input.focus()
    # 填值後又被立刻清除（很像是填不了值），這個網頁很像有刻意防止機器人
    input.val("1").trigger("change")
    
    await delay 50
    buyB = $($('div:contains("買入")')[15]).find("button").first()
    # buyB.click()
    
main()

# 測試
chrome.runtime.onMessage.addListener (message, sender, sendResponse)->
    sendResponse({ content: "來自內容腳本的回覆" })
    