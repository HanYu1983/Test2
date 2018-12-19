
function format(reg, tag, str){
	var gets = []
	var row = 0
	while(row = reg.exec(str)){
		var [_, get] = row
		gets.push(get)
	}
	var ret = str.replace(reg, tag)
	return [ret, gets]
}

function rebuild(reserve, tag, str){
	var ret = str
	for(var i in reserve){
		ret = ret.replace(tag, reserve[i])
	}
	return ret
}

// 被擋!!
function translate(msg){
	return new Promise(function(res, rej){
		// 將無法翻譯的部分抽換掉
		// abc左右必須空一格, 不然有道翻譯會略過
		var [f1, tags1] = format(/(§.+?§)/g, '( fgh )', msg)
		var [f2, tags2] = format(/(\[.+?\])/g, '( ijk )', f1)
		var forstr = f2
		$('#inputOriginal').text(forstr)
		$('#transMachine')[0].dispatchEvent(new Event('click'))
		setTimeout(function(){
			var ret = $('#transTarget > p >span').text()
			var r1 = rebuild(tags1, '(abc)', ret)
			var r2 = rebuild(tags2, '(cde)', r1)
			var restr = r2
			res(restr)
		}, 1000)
	})
}

// 被擋!!
function translateWoai(msg){
	var retSelector = '#jieguo_show_0_html > span'
	return new Promise(function(res, rej){
		// 將無法翻譯的部分抽換掉
		// abc左右必須空一格, 不然有道翻譯會略過
		var [f1, tags1] = format(/(§.+?§)/g, '( fgh )', msg)
		var [f2, tags2] = format(/(\[.+?\])/g, '( ijk )', f1)
		var forstr = f2
		$('#fy_source').text(forstr)
		$('#fy_anniu')[0].dispatchEvent(new Event('click'))
		setTimeout(function(){
			var ret = $(retSelector).text()
			var r1 = rebuild(tags1, '(abc)', ret)
			var r2 = rebuild(tags2, '(cde)', r1)
			var restr = r2
			res(restr)
		}, 10000)
	})
}

function delay(t){
	return new Promise(function(res, rej){
		setTimeout(res, t)
	})
}

(async ()=>{
	var t1 = await translateWoai('[Root.GetName]: go to eat food')
	console.log(t1)
	
	var t2 = await translateWoai('[Root.GetName]: i go to home, gan')
	console.log(t2)
})()

chrome.runtime.onMessage.addListener(function(message, sender, sendResponse){
	console.log(message)
	var {ask, info} = message
	switch(ask){
		case "trans":
			(async ()=>{
				var ret = {}
				for(var k in info){
					var line = await translateWoai(info[k])
					if(line.trim() == ""){
						line = info[k]
					}
					ret[k] = line
				}
				console.log(ret)
				sendResponse({ask: ask, answer: ret})
			})()
			return true //return true for async response
		default:
			sendResponse({ask: ask, answer: ""})
	}
	return false
})