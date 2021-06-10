
{

  const taskPool = [];
  (async () => {
    while (true) {
      console.log("waiting...", taskPool.length)
      try {
        if (taskPool.length) {
          const task = taskPool.pop()
          await task()
        }
      } catch (e) {
        console.log("====")
        console.log(e)
      }
      await new Promise(res => {
        setTimeout(res, 2000)
      })
    }
  })()
  function addTask(task) {
    taskPool.push(task)
  }

  let currentTab = null
  chrome.tabs.query({ currentWindow: true, active: true }, function (tab) {
    if (tab == null) {
      return
    }
    currentTab = tab
  });

  const openedUrlPool = {}
  chrome.extension.onMessage.addListener(function (obj) {
    if (currentTab == null) {
      console.log("currentTab not init")
      return
    }
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
        {
          const url = info
          if (openedUrlPool[url]) {
            console.log("already open:", url)
            return
          }
          openedUrlPool[url] = true
          addTask(() => {
            return new Promise(res => {
              console.log("change tab:", url)
              chrome.tabs.update(currentTab.id, { url: url });
              res()
            })
          })
        }
        break
      case "onScriptSrc":
      case "onCssLink":
        {
          const { url } = info
          if (openedUrlPool[url]) {
            console.log("already open:", url)
            return
          }
          openedUrlPool[url] = true
          addTask(() => {
            return new Promise(res => {
              console.log("change tab:", url)
              const p = url.indexOf("?")
              let formatUrl = url
              if (p != -1) {
                formatUrl = formatUrl.substr(0, p)
              }
              chrome.tabs.update(currentTab.id, { url: formatUrl });
              res()
            })
          })
        }
        break
    }
  });
}

chrome.runtime.onInstalled.addListener(function () {
  return console.log('nothing to do');
});



