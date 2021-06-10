$("<div>Append By ChromeExtension</div>").appendTo("body");
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
        $(links[i]).attr("href", "https://www.mykomon.com" + $(links[i]).attr("href"))
    }
}

// 如是首頁
// 打開所有連結
if (window.location.toString() == 'https://www.mykomon.com/app/homeAo') {
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
        return true
    }).reduce((acc, c) => {
        acc[c] = true
        return acc
    }, {})
    console.log("urls:", urlSet)
    let i = 0;
    for (const url in urlSet) {
        if (++i == 5) {
            break
        }
        console.log("url:", url)
        chrome.extension.sendMessage({ cmd: 'onLink', info: window.location.origin + url })
    }
}

setTimeout(() => {
    const key = window.location.toString()
    const model = {}
    model[key] = document.documentElement.innerHTML
    chrome.storage.local.set(model, () => {
        console.log("fetched:" + window.location.toString())
    })
}, 3000)


