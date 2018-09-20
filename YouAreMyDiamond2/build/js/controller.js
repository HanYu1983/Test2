// https://ropsten.etherscan.io/tx/0xfb3fd9245ff3461b19301fbfb7016b78f2cfa6e193bb7b2ecbe11c7cc4fd0acd

var controller = controller || {};
(function(module){
    
    function setLink(link){
        $.cookie('link', link, { expires: 30, path: '/' });
    }
    
    function getLink(){
        return $.cookie('link');
    }
    
    function isUseLink(){
        return !!getLink()
    }
    
    function formatLinkForUserCopy(link){
        // === 支援本機測試 start === //
        var port = url("port")
        if(port == "8080"){
            port = ":"+port
        } else {
            port = ""
        }
        // === 支援本機測試 end === //
        return url('protocol') +"://"+ url('hostname') + port + url("path") + "?link="+link
    }
    
    function startApp(){
        var keyAmountChangeRunner = 0
    
        var vueModel = new Vue({
          el: '#app',
          data: {
              temp:{
                  keyAmount: 1,
                  keyPriceWei:0,
                  keyPriceEth:0.0,
                  remainTime:0,
                  usedFriendLink: getLink(),
                  useFriend: isUseLink(),
                  lastPlayerIdFormat: "",
                  friendLinkForUserCopy: ""
              },
              roundInfo: {
                "rnd": 0,
                "startTime": 0,
                "endTime": 0, 
                "remainTime": 0,
                "com": 0,
                "pot": 0,
                "pub": 0,
                "state": 0,
                "lastPlyrId": 0
              },
              playerInfo: {
                  key: 0,
                  win: 0,
                  gen: 0,
                  fri: 0,
                  par: 0,
                  friendLink: "link",
                  eth: 0,
                  id: "未有領先玩家",
                  alreadyShareFromKey: 0
              },
              info: {
                  phase: 0,
                  openTime: 0
              }
          },
          filters:{
              formatTime: (str)=>{
                  return window.formatTime(parseInt(str))
              },
              fixFloat: (num)=>{
                  return new Number(num).toFixed(8)
              }
          },
          methods:{
              buy: (keyAmount)=>{
                  if(vueModel.info.phase == 0){
                      alert(txt_alert_notopen)
                      return
                  }
                  
                  (async function(){
                      await updateKeyPrice(keyAmount)
                      var contract = model.getContract()
                      try{
                      
                          if(vueModel.temp.useFriend){
                              var link = vueModel.temp.usedFriendLink
                              await contract.buyWithFriendLink(link, {value: vueModel.temp.keyPriceWei, gas: 2100000})
                          }else{
                              await contract.buy({value: vueModel.temp.keyPriceWei, gas: 2100000})
                          }
                          await loadData()
                      }catch(e){
                          console.log(e)
                      }
                  })()
              },
              vaultBuy: (keyAmount)=>{
                  if(vueModel.info.phase == 0){
                      alert(txt_alert_notopen)
                      return
                  }
                  
                  (async function(){
                      await updateKeyPrice(vueModel.temp.keyAmount)
                      
                      var totalEth = Math.round(
                          (vueModel.playerInfo.win +
                          vueModel.playerInfo.gen +
                          vueModel.playerInfo.fri +
                          vueModel.playerInfo.par) * oneEther
                      );
                      
                      var useEth = vueModel.temp.keyPriceWei;
                      if(useEth > totalEth){
                          alert(txt_alert_ethnotenougth + Math.round(totalEth))
                          return
                      }
                  
                      var contract = model.getContract()
                      try{
                          if(vueModel.temp.useFriend){
                              var link = vueModel.temp.usedFriendLink
                              await contract.vaultBuyWithFriendLink(useEth, link, {gas: 2100000})
                          }else{
                              await contract.vaultBuy(useEth, {gas: 2100000})
                          }
                          await loadData()
                      }catch(e){
                          console.log(e)
                      }
                  })()
              },
              getKeyPrice: (keyAmount)=>{
                  (async function(){
                      var contract = model.getContract()
                      var price = await contract.getKeyPrice(keyAmount * model.fixPointFactor)
                      vueModel.temp.keyPriceWei = price.toNumber()
                      vueModel.temp.keyPriceEth = vueModel.temp.keyPriceWei / oneEther
                  })()
              },
              withdraw: ()=>{
                  if(vueModel.info.phase == 0){
                      alert(txt_alert_notopen)
                      return
                  }
                  
                  (async function(){
                      var contract = model.getContract()
                      try{
                          await contract.withdraw()
                          await loadData()
                      }catch(e){
                          console.log(e)
                      }
                  })()
              },
              addKeyAmount:(keyAmount)=>{
                  vueModel.temp.keyAmount = parseFloat(vueModel.temp.keyAmount) + parseFloat(keyAmount)
                  vueModel.onKeyAmountChange()
              },
              registerPartner: (level)=>{
                  (async function(){
                      var contract = model.getContract()
                      try{
                          var canRegister = await contract.isCanRegisterPartner()
                          if(canRegister == false){
                              alert(txt_alert_youcannotregister)
                              return
                          }
                          var fee = await contract.getPartnerProjectFee(level)
                          await contract.registerPartner(level, {value: fee})
                          await loadData()
                      }catch(e){
                          console.log(e)
                      }
                  })()
              },
              onKeyAmountChange: ()=>{
                  if(keyAmountChangeRunner != 0){
                      clearInterval(keyAmountChangeRunner)
                      keyAmountChangeRunner = 0
                  }
              
                  keyAmountChangeRunner = setTimeout(()=>{
                      (async function(){
                          await updateKeyPrice(vueModel.temp.keyAmount)
                      })()
                  }, 500)
              },
              copyLink: ()=>{
                  if(vueModel.info.phase == 0){
                      alert(txt_alert_notopen)
                      return
                  }
                  
                  var urlstr = url('protocol') + "://" +url('hostname') +":"+ url("port") + url("path") + "?link=" + vueModel.playerInfo.friendLink
                  copyToClipboard(urlstr)
                  alert(txt_alert_copylinksuccess);
              }
          }
        })
    
        var updateKeyPrice = async (keyAmount)=>{
            keyAmount = keyAmount;// + 0.0001;
            var contract = model.getContract()
            var price = await contract.getKeyPrice(keyAmount * model.fixPointFactor)

            vueModel.temp.keyPriceWei = price.toNumber()
            vueModel.temp.keyPriceEth = vueModel.temp.keyPriceWei / oneEther
        }
    
        var loadData = async ()=>{
            var roundInfo = await model.loadRoundInfo()
            console.log(roundInfo)
            vueModel.roundInfo = roundInfo
        
            var playerInfo = await model.loadPlayerInfo()
            console.log(playerInfo)
            vueModel.playerInfo = playerInfo
            
            await updateKeyPrice(vueModel.temp.keyAmount)
            
            vueModel.temp.lastPlayerIdFormat = 
                roundInfo.lastPlyrId == 0 ? "尚未有人领先" :
                roundInfo.lastPlyrId == playerInfo.id ? "我" :
                "玩家" + roundInfo.lastPlyrId
            
            vueModel.temp.friendLinkForUserCopy = formatLinkForUserCopy(playerInfo.friendLink)
        }
    
        var start = async ()=>{
            await model.loadContract()
            var info = await model.getContract().getInfo()
            var [phase, openTime] = info
            vueModel.info.phase = phase
            vueModel.info.openTime = openTime.toNumber()
            
            if(vueModel.info.phase == 0){
                alert(txt_alert_notopen)
            }
            
            await loadData()
        
            model.addListener({
                onMsg: function(msg){
                    console.log(msg)
                    if(msg.indexOf("buy") != -1){
                        async()=>{
                            // message alert
                        }
                    }
                }
            })
        
            var update = ()=>{
                var remainSecond = --vueModel.roundInfo.remainTime
                if(remainSecond < 0){
                    remainSecond = 0
                }
                vueModel.temp.remainTime = remainSecond
            }
            setInterval(update, 1000)
        }
        start()
    }
    
    module.setLink = setLink
    module.getLink = getLink
    module.formatLinkForUserCopy = formatLinkForUserCopy
    module.startApp = startApp
})(controller)