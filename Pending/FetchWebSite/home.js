// 將路徑改為相對路徑
// {
//     const as = $("a")
//     for (let i = 0; i < as.length; ++i) {
//         $(as[i]).attr("href", "." + $(as[i]).attr("href"))
//     }
// }
// 
{
    const links = $(document).find("link")
    for (let i = 0; i < links.length; ++i) {
        const href = $(links[i]).attr("href")
        const filename = href.substring(href.lastIndexOf('/') + 1);
        chrome.extension.sendMessage({ cmd: 'onCssLink', info: { downloadUrl: window.location.origin + href, filename: filename } })
        $(links[i]).attr("href", window.location.origin + href)
    }
}

// 如是首頁
// 打開所有連結
// if (window.location.toString() == 'https://www.mykomon.com/app/homeAo') 
{
    const aLinks = $("a")
    const urls = []
    for (let i = 0; i < aLinks.length; ++i) {
        const url = $(aLinks[i]).attr("href")
        urls.push(url)
    }
    const urlSet = urls.filter(url => {
        if (url == null) {
            return false
        }
        if (url == "") {
            return false
        }
        if (url == '/app/homeAo') {
            return false
        }
        if (url == '/app/home') {
            return false
        }
        if (url.startsWith("javascrip")) {
            return false
        }
        if (url.startsWith("#")) {
            return false
        }
        if (url.startsWith("\"")) {
            return false
        }
        if (url.startsWith("http")) {
            return false
        }
        if (url.indexOf("viewNews") != -1) {
            return false
        }
        if (url.indexOf("viewTokushu") != -1) {
            return false
        }
        if (url.indexOf("listBook") != -1) {
            return false
        }
        if (url.indexOf("logout") != -1) {
            return false
        }
        return true
    }).reduce((acc, c) => {
        acc[c] = true
        return acc
    }, {})
    console.log("urls:", urlSet);

    (async () => {
        let i = 0;
        for (const url in urlSet) {
            if (++i == 10) {
                break
            }
            await new Promise((res) => {
                setTimeout(() => {
                    chrome.extension.sendMessage({ cmd: 'onLink', info: window.location.origin + url })
                    res()
                }, 3000)
            })
        }
    })()
}

setTimeout(() => {
    const key = window.location.toString()
    const model = {}
    model[key] = document.documentElement.innerHTML
    chrome.storage.local.set(model, () => {
        console.log("fetched:" + window.location.toString())
    })
}, 3000)


