chrome.runtime.onInstalled.addListener(function() {
	console.log('oninstall')
	
	chrome.extension.sendMessage({cmd:'background', info:"abc"});
});