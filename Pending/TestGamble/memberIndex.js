var dispatchMouseEvent = function(target, var_args) {
    var e = document.createEvent("MouseEvents");
    e.initEvent.apply(e, Array.prototype.slice.call(arguments, 1));
    target.dispatchEvent(e);
};
var dispatchKeyboardEvent = function(target, initKeyboradEvent_args) {
    var e = document.createEvent("KeyboardEvents");
    e.initKeyboardEvent.apply(e, Array.prototype.slice.call(arguments, 1));
    target.dispatchEvent(e);
};
var dispatchTextEvent = function(target, initTextEvent_args) {
    var e = document.createEvent("TextEvent");
    e.initTextEvent.apply(e, Array.prototype.slice.call(arguments, 1));
    target.dispatchEvent(e);
};
var dispatchSimpleEvent = function(target, type, canBubble, cancelable) {
    var e = document.createEvent("Event");
    e.initEvent.apply(e, Array.prototype.slice.call(arguments, 1));
    target.dispatchEvent(e);
};

function delay(v){
    return new Promise((res, rej)=>{
        setTimeout(()=>res(true), v)
    })
}

function closePopup(){
    for(var i=1; i<=20;++i) {
        $('#notice_button'+i).click()
    }
}

function clickTab(){
    dispatchMouseEvent($(document).find('a[href="load?lottery=PK10JSC&page=110"]')[0], 'click', true, true);
}

function inputBet(idx, v){
    // inject script to iframe https://jsfiddle.net/onury/ALHGP/
    var script = document.createElement( "script" )
    script.setAttribute('type', 'text/javascript');
    script.innerHTML = '$(\'input[name="B1_'+idx+'"]\').val('+v+')';
    $('iframe')[0].contentDocument.body.append(script)
}

function clickBet(){
    //<input type="button" onclick="bet()" value="确定" class="button">
    var script = document.createElement( "script" )
    script.setAttribute('type', 'text/javascript');
    script.innerHTML = '$(\'input[value="确定"]\').click()';
    $('iframe')[0].contentDocument.body.append(script)
}


function clickConfirm(){
    //<button type="button" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button"><span class="ui-button-text">确定</span></button>
    // 以下5和2請用chrome檢查在console中下$('button[type="button"]')來看是哪一個
    // 按掉錯誤提示
    $('button[type="button"]')[5].click()
    // 按下確認
    $('button[type="button"]')[2].click()
}

function getDrawNum(){
    // <span id="drawNumber">30859915</span>
    var ret = $($('iframe')[0].contentDocument.body).find('#drawNumber').html()
    return parseInt(ret)
}

function getSeconds(){
    // <span id="cdDraw">00:11</span>
    var time = $($('iframe')[0].contentDocument.body).find('#cdDraw').html()
    var [minute, second] = time.split(':')
    minute = parseInt(minute)
    second = parseInt(second)
    return second + minute*60
}

function getResult(){
    // <span id="cdDraw">00:11</span>
    var ret = $($('iframe')[0].contentDocument.body).find('#bresult').html()
    return parseInt(ret)
}

function getRank(){
    var [before, after] = $("#result_balls").find("span").text().split("10")
    var rank = []
    for(var i=0; i<before.length; ++i){
        rank.push(parseInt(before[i]))
    }
    rank.push(10)
    for(var i=0; i<after.length; ++i){
        rank.push(parseInt(after[i]))
    }
    return rank
}

(async ()=>{
    
    console.log("close popup")
    closePopup()
    
    await delay(5000)
    console.log("click tab")
    clickTab()
    
    await delay(5000)
    
    var startBet = 1
    var maxLevel = 40
    var record = {
        level: 0,
        state: "waitBuy",
        drawNumber: getDrawNum(),
        bet: startBet,
        pos: -1,
        loseTime: 0,
        totalLoseTime: 0
    }
    
    for(;;){
        console.log(record)
        chrome.extension.sendMessage({cmd:'loop', info:record});
        
        switch(record.state){
        case "waitBuy":
            {
                var second = getSeconds()
                if(second > 20 && second < 60){
                    // 排名和下注位置都是從1開始, 請略過0
                    var pos = getRank()[0]
                    if(record.pos > -1){
                        var isWin = pos == record.pos
                        if(isWin){
                            record.loseTime = 0
                        } else {
                            record.loseTime += 1
                            record.totalLoseTime += 1
                        }
                    }
                    record.bet = (Math.floor(record.loseTime / 5) + 1) * startBet
                    record.pos = pos
                    inputBet(record.pos, record.bet)
                    // 若是最後一注，讓迴圈結束也沒關係。不必運行waitResult
                    record.level += 1
                    
                    chrome.extension.sendMessage({cmd:'loop', info:record});
                    await delay(5000)
                    /*
                    clickBet()
                    await delay(1000)
                    clickConfirm()
                    */
                    record.state = "waitResult"
                }
            }
            break
        case "waitResult":
            {
                var currDrawNum = getDrawNum()
                var enterNextDraw = currDrawNum > record.drawNumber
                if(enterNextDraw == false){
                    break
                }
                record.drawNumber = currDrawNum
                
                chrome.extension.sendMessage({cmd:'loop', info:record});
                await delay(5000)
                if(record.level >= maxLevel){
                    record.state = "finished"
                } else {
                    record.state = "waitBuy"
                }
            }
            break
        case "finished":
            {
                // ignore
                // 不跳出迴圈的用意是要讓它持續更新UI(popup.html)
                // 不然使用者體驗不好
            }
            break
        }
        await delay(1000)
    }
})()