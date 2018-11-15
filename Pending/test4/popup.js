
window.onload = function() {
	var console = chrome.extension.getBackgroundPage().console

	let btn = document.getElementById('btn');
	btn.onclick = ()=>{
		/*chrome.tabs.query({}, (tabs)=>{
			for(var i in tabs){
				var t = tabs[i]
				console.log(t)
				chrome.tabs.executeScript(t.id, {file: 'parse.js'});
			}
		})*/
		
		chrome.windows.getCurrent(function (currentWindow) {
			chrome.tabs.query({active: true, windowId: currentWindow.id},function(activeTabs) {
				chrome.tabs.executeScript(activeTabs[0].id, {file: 'inner.js', allFrames: true});
			});
		});
	}
	
	document.getElementById('btn_win').addEventListener('click',()=>{
		chrome.windows.create({url:'http://tcg.sanguosha.com/card/51/detail'}, (w)=>{
			console.log('create window')
			console.log(w)
			chrome.tabs.create({url:'http://tcg.sanguosha.com/card/51/detail', windowId: w.id}, (t)=>{
				
			})
		})
	})
};

function openDetailWindow(links){
	var console = chrome.extension.getBackgroundPage().console
	
	if(links.length <= 0){
		return
	}
	var first = links[0]
	chrome.windows.create({url:first}, (w)=>{
		for(var i=1; i<links.length; ++i){
			chrome.tabs.create({url:links[i], windowId: w.id}, (t)=>{
				chrome.tabs.executeScript(t.id, {file: 'parse.js'});
			})
		}
	})
}

var infos = []
chrome.extension.onMessage.addListener(function(info) {
	var console = chrome.extension.getBackgroundPage().console
	
	console.log(info)
	
	if(info.cmd == 'detailLinks'){
		openDetailWindow(info.info)
	}
	
	/*
	infos.push(info)
	console.log(info)
	**/
});

