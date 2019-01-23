
/*
const translate = require('google-translate-api');
 
translate('I spea Dutch!', {from: 'en', to: 'nl'}).then(res => {
    console.log(res.text);
    //=> Ik spreek Nederlands!
    console.log(res.from.text.autoCorrected);
    //=> true
    console.log(res.from.text.value);
    //=> I [speak] Dutch!
    console.log(res.from.text.didYouMean);
    //=> false
}).catch(err => {
    console.error(err);
});

*/


/*
const translate = require('translate-api');

let transText = 'hello world!';
translate.getText(transText,{to: 'zh-CN'}).then(function(text){
	console.log(text)
}).catch(function(err){
	console.log(err)
})
*/

const request = require('request')
const fs = require('fs')
const { join } = require('path')
const YAML = require('yamljs');

const isDirectory = source => fs.lstatSync(source).isDirectory()
const getDirectories = source => fs.readdirSync(source)

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

function readFile(path){
	return new Promise((res,rej)=>{
		fs.readFile(path, 'utf8', function (err,data) {
			if (err) {
				rej(err)
				return
			}
			res(data)
		});
	})
}

function writeFile(path, data){
	return new Promise((res,rej)=>{
		fs.writeFile(path, data, function(err) {
			if(err) {
				rej(err)
				return
			}
			res()
		})
	})
}

function delay(t){
	return new Promise((res, rej)=>{
		setTimeout(res, t)
	})
}
// 被擋!!
function translate(msg){
	return new Promise(function(res, rej){
		// 將無法翻譯的部分抽換掉
		// abc左右必須空一格, 不然有道翻譯會略過
		var [f1, tags1] = format(/(§.+?§)/g, '(fgh)', msg)
		var [f2, tags2] = format(/(\[.+?\])/g, '(ijk)', f1)
		var [f3, tags3] = format(/(\$.+?\$)/g, '(lmn)', f2)
		var forstr = f3
		var sourceLang = 'en'
		var targetLang = 'zh-cn'
		var sourceText = forstr
		var url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + sourceLang + "&tl=" + targetLang + "&dt=t&q=" + encodeURI(forstr);
		console.log(url)
		request({
			url: url,
			method:'get'
		}, (err, _, body)=>{
			if(err){
				rej(err)
			} else {
				var ret = body
				var r1 = rebuild(tags1, '（FGH）', ret)
				var r2 = rebuild(tags2, '（IJK）', r1)
				var r3 = rebuild(tags3, '（LMN）', r2)
				try{
					var [[[restr]]] = JSON.parse(r3)
					res(restr)
				}catch(err){
					rej(err)
				}
			}
		})
	})
}


(async ()=>{
	var files = getDirectories('docs')
	console.log(files)
	for(var f in files){
		var filename = files[f]
		var path = join('docs', filename)
		var content = await readFile(path)
		var format = content.replace(/0 /g, " ")
		console.log(format)
		var json = YAML.parse(format)
		console.log(json)
		
		var root = json
		var ret = {}
		for(var k in root){
			if(root[k] == null){
				continue
			}
			await delay(5000)
			try{
				// 又被擋了!!
				var line = await translate(root[k])
				if(line.trim() == ""){
					line = root[k]
				}
			}catch(e){
				console.log(e)
				continue
			}
			ret[k] = line
		}
		console.log(ret)
			
		var rebuild = {
			"l_simp_chinese": ret
		}
		var txt = YAML.stringify(rebuild, 4);
		txt = txt.replace(/    /g, " ")
		console.log(txt)
		
		var outputPath = join('output', filename)
		console.log('write:'+outputPath)
		await writeFile(outputPath, txt)
	}
})()
