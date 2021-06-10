
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
        chrome.tabs.create({ url: url });
      }
      break
    case "onCssLink":
      {
        const { downloadUrl, filename } = info
        if (openedUrlPool[downloadUrl]) {
          console.log("already open:", downloadUrl)
          return
        }
        openedUrlPool[downloadUrl] = true
        var downloadOptions = {
          url: downloadUrl,
          filename: filename, // no effect
          saveAs: false,
        };
        chrome.downloads.download(downloadOptions, function (downloadId) {
          console.log(downloadId);
        });
        // var a = document.createElement("a");
        // document.body.appendChild(a);
        // a.style = "display: none";
        // a.href = downloadUrl;
        // a.download = "xxx.css";
        // a.click();
        // window.URL.revokeObjectURL(downloadUrl);
        // a.remove()
      }
      break
  }
});
chrome.runtime.onInstalled.addListener(function () {
  return console.log('nothing to do');
});



