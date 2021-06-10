window.onload = function () {
  const console = chrome.extension.getBackgroundPage().console
  console.log("start app")

  document.getElementById('fetch').addEventListener('click', function () {
    // 資料由home.js中存到storage.local
    // 取得所有網頁的html
    // 並塞入log中
    chrome.storage.local.get(null, model => {
      const logDom = $("#log")
      for (const url in model) {
        // 每一頁
        const htmlContent = model[url]
        logDom.append($(`<h1>${url}</h1>`))
        const pageNode = $("<div/>").html(htmlContent)
        logDom.append(pageNode)
      }
    })
  });

  document.getElementById('download').addEventListener('click', function () {
    const links = $(document).find("link")
    for (let i = 0; i < links.length; ++i) {
      const href = $(links[i]).attr("href")
      const filename = href.substring(href.lastIndexOf('/') + 1);
      $(links[i]).attr("href", filename)
    }

    const html = document.documentElement.innerHTML
    var blob = new Blob([html], { type: "text/html" });
    var url = URL.createObjectURL(blob);
    // =========== 以下的方法無法命名檔名 ========= // 
    // var downloadOptions = {
    //   url: url,
    //   filename: "test.html",
    //   saveAs: false,
    // };
    // chrome.downloads.download(downloadOptions, function (downloadId) {
    //   console.log(downloadId);
    // });

    // ========== 使用這個方法命名檔名 =========== //
    var a = document.createElement("a");
    document.body.appendChild(a);
    a.style = "display: none";
    a.href = url;
    a.download = `${window.location.origin}.html`;
    a.click();
    window.URL.revokeObjectURL(url);
    a.remove()
  })

  document.getElementById('clear').addEventListener('click', function () {
    chrome.storage.local.clear(function () {
      console.log("cleared");
    });
  })

  chrome.extension.onMessage.addListener(function (obj) {
    console.log(obj)
  });
};
