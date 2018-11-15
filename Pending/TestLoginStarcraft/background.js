chrome.runtime.onInstalled.addListener(function() {
	console.log('oninstall')
});

chrome.tabs.onCreated.addListener(function(t){
    console.log("onCreated")
    console.log(t)
});

chrome.tabs.onUpdated.addListener(function(t, {status}) {
    console.log("onUpdated")
    console.log(t)
    console.log(status)
    if(status == "complete"){
        console.log("send onComplete")
        chrome.extension.sendMessage({cmd:'onComplete', info:t.id});
    }
});