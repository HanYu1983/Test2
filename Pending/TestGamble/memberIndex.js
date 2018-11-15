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

function fillfield(v){
    // inject script to iframe https://jsfiddle.net/onury/ALHGP/
    var script = document.createElement( "script" )
    script.setAttribute('type', 'text/javascript');
    script.innerHTML = '$(\'input[name="B1_1"]\').val('+v+')';
    $('iframe')[0].contentDocument.body.append(script)
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

(async ()=>{
    console.log("close popup")
    closePopup()
    
    await delay(5000)
    console.log("click tab")
    clickTab()
    
    await delay(5000)
    for(;;){
        var second = getSeconds()
        console.log(second)
        if(second > 20){
            console.log("fill field")
            fillfield(1)
        }
        var result = getResult()
        console.log(result)
        await delay(1000)
    }
    
    /*
    await delay(1000)
    console.log("click ok")
    var script = document.createElement( "script" )
    script.setAttribute('type', 'text/javascript');
    script.innerHTML = '$(\'input[value="确定"]\').click()';
    $('iframe')[0].contentDocument.body.append(script)
    //<input type="button" onclick="bet()" value="确定" class="button">
    */
})()



chrome.extension.onMessage.addListener(function(info) {
    console.log(info)
});