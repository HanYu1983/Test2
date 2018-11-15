function delay(v){
    return new Promise((res, rej)=>{
        setTimeout(()=>res(true), v)
    })
}

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

function closePopup(){
    for(var i=1; i<=10;++i) {
        $('#notice_button'+i).click()
    }
}

function clickTab(){
    dispatchMouseEvent($(document).find('a[href="load?lottery=PK10JSC&page=110"]')[0], 'click', true, true);
}

function fillfield(idx, v){
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
    var record = {
        state: "waitBuy",
        result: getResult(),
        drawNumber: getDrawNum(),
        bet: startBet
    }
    
    for(;;){
        var second = getSeconds()
        console.log(record)
        switch(record.state){
        case "waitBuy":
            {
                if(second > 20 && second < 60){
                    console.log("fill field")
                    var pos = getRank()[0]
                    fillfield(pos, record.bet)
                    await delay(5000)
                    /*
                    console.log("Bet!!")
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
                await delay(5000)
                
                var result = getResult()
                var isWin = result > record.result
                if(isWin){
                    record.bet = startBet
                } else {
                    record.bet = record.bet*2
                }
                record.result = result
                record.state = "waitBuy"
            }
            break
        }
        await delay(1000)
    }
})()



chrome.extension.onMessage.addListener(function(info) {
    console.log(info)
});