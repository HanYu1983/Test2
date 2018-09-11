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
                  useFriend: isUseLink()
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
                  friendLink: "link"
              }
          },
          filters:{
              formatTime: (str)=>{
                  return window.formatTime(parseInt(str))
              }
          },
          methods:{
              buy: (keyAmount)=>{
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
                          alert("eth not enougth. your vault is "+ Math.round(totalEth))
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
                  vueModel.temp.keyAmount += parseFloat(keyAmount)
                  vueModel.onKeyAmountChange()
              },
              registerPartner: (level)=>{
                  (async function(){
                      var contract = model.getContract()
                      try{
                          var fee = await contract.getPartnerProjectFee(level)
                          await contract.registerPartner(level, {value: fee})
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
                  var urlstr = url('protocol') + "://" +url('hostname') +":"+ url("port") + url("path") + "?link=" + vueModel.playerInfo.friendLink
                  copyToClipboard(urlstr)
                  alert("成功copy到剪貼簿");
              }
          }
        })
    
        var updateKeyPrice = async (keyAmount)=>{
            keyAmount = keyAmount + 0.0001;
            var contract = model.getContract()
            var price = await contract.getKeyPrice(keyAmount * model.fixPointFactor)

            vueModel.temp.keyPriceWei = price.toNumber()
            vueModel.temp.keyPriceEth = vueModel.temp.keyPriceWei / oneEther
        }
    
        var loadData = async ()=>{
            var contract = model.getContract()
            var roundInfo = await model.loadRoundInfo()
            console.log(roundInfo)
        
            var playerInfo = await model.loadPlayerInfo()
            console.log(playerInfo)
        
            vueModel.roundInfo = roundInfo
            vueModel.playerInfo = playerInfo
        }
    
    
        var start = async ()=>{
            await model.loadContract()
            await loadData()
            await updateKeyPrice(vueModel.temp.keyAmount)
        
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
    module.startApp = startApp
})(controller)