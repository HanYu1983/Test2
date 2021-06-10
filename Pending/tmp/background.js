
const openedUrlPool = {}
chrome.extension.onMessage.addListener(function (obj) {
  console.log(obj)
  const { cmd, info } = obj
  switch (cmd) {
    case "onLink":
      {
        const url = info
        if (openedUrlPool[url]) {
          console.log("already open:", url)
          return
        }
        openedUrlPool[url] = true
        console.log(openedUrlPool)
        chrome.tabs.create({ url: url });
      }
  }
});
chrome.runtime.onInstalled.addListener(function () {
  return console.log('nothing to do');
});



