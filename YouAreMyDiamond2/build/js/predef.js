const copyToClipboard = str => {
  const el = document.createElement('textarea');  // Create a <textarea> element
  el.value = str;                                 // Set its value to the string that you want copied
  el.setAttribute('readonly', '');                // Make it readonly to be tamper-proof
  el.style.position = 'absolute';                 
  el.style.left = '-9999px';                      // Move outside the screen to make it invisible
  document.body.appendChild(el);                  // Append the <textarea> element to the HTML document
  const selected =            
    document.getSelection().rangeCount > 0        // Check if there is any content selected previously
      ? document.getSelection().getRangeAt(0)     // Store selection if found
      : false;                                    // Mark as false to know no selection existed before
  el.select();                                    // Select the <textarea> content
  document.execCommand('copy');                   // Copy - only works as a result of a user action (e.g. click events)
  document.body.removeChild(el);                  // Remove the <textarea> element
  if (selected) {                                 // If a selection existed before copying
    document.getSelection().removeAllRanges();    // Unselect everything on the HTML document
    document.getSelection().addRange(selected);   // Restore the original selection
  }
};

function byteStr2str(byteStr){
  var msg = byteStr
  var [_, parsed] = msg.split('x')

  var ret = ""
  for(var i=0; i<parsed.length; i+=2){
    var token = parsed[i] + parsed[i+1]
    var tokenI = parseInt(token, 16)
    if(tokenI != 0){
      var c = String.fromCharCode(tokenI)
      ret += c
    }
  }
  return ret
}

function formatTime(seconds){
  var date = new Date(seconds*1000)
  return date.getUTCHours()+":"+date.getUTCMinutes()+":"+date.getUTCSeconds() 
}

var oneEther = 1000000000000000000;
var fixPointFactor = oneEther;
var txt_alert_notopen = "尚未开盘";
var txt_alert_youcannotregister = "无法注册";
var txt_alert_ethnotenougth = "馀额不足，你的钱包剩";
var txt_alert_copylinksuccess = "成功copy到剪贴簿";
