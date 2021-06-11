
function delay(t) {
  return new Promise(res => {
    setTimeout(res, t)
  })
}

{
  const taskPool = [];
  (async () => {
    while (true) {
      console.log("waiting...", taskPool.length)
      try {
        if (taskPool.length) {
          const task = taskPool.pop()
          // 這裡不能用異步，因為大量的呼叫會讓網站負荷不了
          await task()
        }
      } catch (e) {
        console.log("====")
        console.log(e)
      }
      await delay(100)
    }
  })()
  function addTask(task) {
    taskPool.push(task)
  }

  const openedUrlPool = {}
  chrome.extension.onMessage.addListener(function (obj) {
    const { cmd, info } = obj
    console.log(cmd)
    switch (cmd) {
      case "onFetch":
        {
          const { url, content } = info
          addTask(() => {
            return new Promise((res) => {
              console.log("download:", url)
              let postfix = ".html"
              let mine = "text/html"
              if (url.indexOf("css") != -1) {
                postfix = ""
                mine = "text/css"
              } else if (url.indexOf("js") != -1) {
                postfix = ""
                mine = "text/javascript"
              }
              const blob = new Blob([content], { type: mine });
              const objectUrl = URL.createObjectURL(blob);
              const a = document.createElement("a");
              document.body.appendChild(a);
              a.style = "display: none";
              a.href = objectUrl;
              // 要加上download才會下載
              // `${encodeURIComponent(url)}.html`可以自動建立資料夾, 必須有副檔名
              // 必須使用blob名稱才會生效
              a.download = `${encodeURIComponent(url)}${postfix}`
              a.click();
              window.URL.revokeObjectURL(objectUrl);
              a.remove()
              res()
            })
          })
        }
        break
      case "onLink":
      case "onScriptSrc":
      case "onCssLink":
        {
          const { url } = info
          if (openedUrlPool[url]) {
            console.log("already open:", url)
            return
          }
          openedUrlPool[url] = true
          addTask(async () => {
            console.log("change tab:", url)
            let formatUrl = url
            // 只有css,js要做
            if (cmd != "onLink") {
              const p = url.indexOf("?")
              if (p != -1) {
                formatUrl = formatUrl.substr(0, p)
              }
            }
            const tab = await new Promise(res => {
              // 必須使用callback的形式才能取到tab
              chrome.tabs.create({ url: formatUrl }, tab => {
                res(tab)
              });
            })
            await delay(5000)
            await chrome.tabs.remove(tab.id)
          })
        }
        break
    }
  });
}

chrome.runtime.onInstalled.addListener(function () {
  return console.log('nothing to do');
});



