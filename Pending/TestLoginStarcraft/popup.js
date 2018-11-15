// 每開一個新頁，這個extension都要重按一次來打開，以下的腳本才能執行
// 所以以下的onMessage必須在新頁打開時立刻在那頁的按下這個extension後，才能聽到事件

window.onload = function() {
    var console = chrome.extension.getBackgroundPage().console
    console.log("popupjs onload")

    let btn = document.getElementById('btn');
    btn.onclick = ()=>{
        chrome.windows.getCurrent(function (currentWindow) {
            chrome.tabs.create({url: 'https://starcraft2.com/zh-tw/', windowId: currentWindow.id}, (t)=>{
            })
        });
    };
    
    /*
    document.getElementById('btn_win').addEventListener('click',()=>{
        chrome.windows.getCurrent((currentWindow)=>{
            chrome.tabs.query({active: true, windowId: currentWindow.id},function(activeTabs) {
                chrome.tabs.executeScript(activeTabs[0].id, { file: 'jquery.min.js' }, function() {
                    chrome.tabs.executeScript(activeTabs[0].id, { file: "inner.js" });
                });
            });
        });
    });
    */
};

var infos = []
chrome.extension.onMessage.addListener(function(info) {
    var console = chrome.extension.getBackgroundPage().console
    console.log(info)
});


