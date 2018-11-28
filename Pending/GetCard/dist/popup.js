// Generated by LiveScript 1.6.0
(function(){
  window.onload = function(){
    var console, delay;
    console = chrome.extension.getBackgroundPage().console;
    console.log("popup.js onload");
    delay = function(t){
      return new Promise(function(res, rej){
        return setTimeout(res, t);
      });
    };
    async function download(srcs){
      var cnt, num, i$, ref$, len$, i, s, e, j$, ref1$, len1$, src, fn$ = async function(){
        var i$, to$, results$ = [];
        for (i$ = 0, to$ = num; i$ <= to$; ++i$) {
          results$.push(i$);
        }
        return results$;
      }, results$ = [];
      console.log(srcs.length);
      cnt = 100;
      num = Math.floor(srcs.length / cnt);
      for (i$ = 0, len$ = (ref$ = (await (fn$()))).length; i$ < len$; ++i$) {
        i = ref$[i$];
        s = i * cnt;
        e = Math.min(s + cnt, srcs.length);
        console.log(s, e);
        for (j$ = 0, len1$ = (ref1$ = srcs.slice(s, e)).length; j$ < len1$; ++j$) {
          src = ref1$[j$];
          chrome.downloads.download({
            url: src
          }, fn1$);
        }
        results$.push((await delay(10000)));
      }
      return results$;
      function fn1$(downloadId){}
    }
    return $('#btnDownloadPics').click(function(){
      return chrome.tabs.query({
        active: true,
        currentWindow: true
      }, function(ts){
        return chrome.tabs.sendMessage(ts[0].id, {
          ask: "getImgs"
        }, function(msg){
          var ask, answer, srcs;
          if (!msg) {
            return;
          }
          ask = msg.ask, answer = msg.answer;
          if (ask === "getImgs") {
            srcs = answer.map(function(src){
              return "https://fftcg.square-enix-games.com".concat(src);
            });
            return download(srcs);
          }
        });
      });
    });
  };
}).call(this);
