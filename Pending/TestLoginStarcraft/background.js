chrome.runtime.onInstalled.addListener(function() {
	console.log('oninstall')
	chrome.extension.sendMessage({cmd:'background', info:"abc"});
});

chrome.tabs.onCreated.addListener(function(t){
    console.log("onCreated")
    console.log(t)
    /*
    chrome.tabs.executeScript(t.id, { file: 'jquery.min.js' }, function() {
        console.log('inner')
        chrome.tabs.executeScript(t.id, { file: "inner.js" });
    });
    */
});

chrome.tabs.onUpdated.addListener(function(t, {status}) {
    console.log("onUpdated")
    console.log(t)
    console.log(status)
    
    if(status == "complete"){
        /*
        chrome.tabs.executeScript(t.id, { file: 'jquery.min.js' }, function() {
            console.log('inner')
            chrome.tabs.executeScript(t.id, { file: "inner.js" });
        });
        */
        console.log("send onComplete")
        chrome.extension.sendMessage({cmd:'onComplete', info:t.id});
    }
});