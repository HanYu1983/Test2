console.log('parse')
document.body.style.backgroundColor = "#ffaa00"


var nameElem = document.getElementsByClassName("name")
var name = nameElem[0].innerText
console.log(nameElem[0].innerText)

chrome.extension.sendMessage({
	name: name
});