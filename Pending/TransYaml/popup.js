// 每開一個新頁，這個extension都要重按一次來打開，以下的腳本才能執行
// 所以以下的onMessage必須在新頁打開時立刻在那頁的按下這個extension後，才能聽到事件

window.onload = function() {
	var console = chrome.extension.getBackgroundPage().console
	var url = chrome.runtime.getURL("docs/newfrontiers_l_simp_chinese.yml")
	
	$.ajax({
		url: url,
		dataType: 'text',
		success: function(res){
			var format = res.replace(/0 /g, " ")
			var json = YAML.parse(format)
			var root = json["l_simp_chinese"]
			chrome.tabs.query({active: true, currentWindow: true}, (ts)=>{
				chrome.tabs.sendMessage(ts[0].id, {ask: "trans", info: root}, (msg)=>{
					var rebuild = {
						"l_simp_chinese": msg.answer
					}
					var txt = YAML.stringify(rebuild, 4);
					txt = txt.replace(/    /g, " ")
					console.log(txt)
					$('textarea').text(txt)
				})
			})
		},
		error: function(xhr, res, err){
			console.log(err)
		}
	})
}


