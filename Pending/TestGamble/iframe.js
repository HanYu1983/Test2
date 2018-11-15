//alert("iframe")

var data = 'iframe test';
chrome.runtime.sendMessage({sendBack:true, data:data});

/*
if (!window.isTop) {
    var data = 'iframe test';
    chrome.runtime.sendMessage({sendBack:true, data:data});
}
*/