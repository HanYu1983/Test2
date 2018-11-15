// 每開一個新頁，這個extension都要重按一次來打開，以下的腳本才能執行
// 所以以下的onMessage必須在新頁打開時立刻在那頁的按下這個extension後，才能聽到事件

window.onload = function() {
    var console = chrome.extension.getBackgroundPage().console
    console.log("start app")
    /*
    var btn = $('#btnStart')[0];
    btn.onclick = ()=>{
        chrome.windows.getCurrent(function (currentWindow) {
            chrome.tabs.query({active: true, windowId: currentWindow.id},function(activeTabs) {
                chrome.tabs.executeScript(activeTabs[0].id, { file: 'jquery.min.js' }, function() {
                    chrome.tabs.executeScript(activeTabs[0].id, { file: 'memberIndex.js' });
                });
            });
        });
    };
    */
    
    function convertState(state){
        var mapping = {
            "waitBuy": "等待下注",
            "waitResult": "等待結果"
        }
        return mapping[state]
    }
    
    var infos = []
    chrome.extension.onMessage.addListener(function(info) {
        var console = chrome.extension.getBackgroundPage().console
        console.log(info)
        
        if(info.cmd == "loop"){
            $("#level").html(info.info.level)
            $("#state").html(convertState(info.info.state))
            $("#drawNumber").html(info.info.drawNumber)
            $("#bet").html(info.info.bet)
            $("#pos").html(info.info.pos)
            $("#loseTime").html(info.info.loseTime)
        }
    });
    
};


