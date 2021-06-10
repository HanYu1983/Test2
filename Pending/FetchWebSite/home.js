// get all css
{
    const links = $(document).find("link")
    for (let i = 0; i < links.length; ++i) {
        const href = $(links[i]).attr("href")
        chrome.extension.sendMessage({ cmd: 'onCssLink', info: { url: window.location.origin + href } })
    }
}

// get all js
{
    const links = $(document).find("script")
    for (let i = 0; i < links.length; ++i) {
        const href = $(links[i]).attr("src")
        if (href) {
            //chrome.extension.sendMessage({ cmd: 'onScriptSrc', info: { url: window.location.origin + href } })
        }
    }
}
// get all link
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
        if (url.indexOf("http") != -1) {
            return false
        }
        if (url.indexOf("viewDaily") != -1) {
            return false
        }
        if (url.indexOf("daily.do") != -1) {
            return false
        }
        if (url.indexOf("www.mykomon.comviewitem") != -1) {
            return false
        }
        return true
    }).reduce((acc, c) => {
        acc[c] = true
        return acc
    }, {})
    let i = 0;
    for (const url in urlSet) {
        if (++i == 5) {
            break
        }
        //chrome.extension.sendMessage({ cmd: 'onLink', info: { url: window.location.origin + url } })
    }
}

// get this page
setTimeout(() => {
    chrome.extension.sendMessage({
        cmd: 'onFetch', info: {
            url: window.location.toString(),
            content: document.documentElement.innerHTML
        }
    })
}, 500)


