// https://ropsten.etherscan.io/tx/0xfb3fd9245ff3461b19301fbfb7016b78f2cfa6e193bb7b2ecbe11c7cc4fd0acd

var controller = controller || {};
(function(module){
    function init(){
        var keyAmountChangeRunner = 0
    
        var vueModel = new Vue({
          el: '#app',
          data: {
              temp:{
                  keyAmount: 1,
                  keyPriceWei:0,
                  keyPriceEth:0.0,
                  remainTimeFormat:"",
                  partnerLink:"",
                  usedPartnerLink: "",
                  usedFriendLink: "",
                  useFriend: false
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
                  
                      var contract = model.getContract()
                      try{
                          if(vueModel.temp.useFriend){
                              var link = vueModel.temp.usedFriendLink
                              await contract.vaultBuyWithFriendLink(vueModel.temp.keyPriceWei, link, {value: vueModel.temp.keyPriceWei, gas: 2100000})
                          }else{
                              await contract.vaultBuy(vueModel.temp.keyPriceWei, {gas: 2100000})
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
              copyLink: (arg)=>{
                  copyToClipboard(vueModel.playerInfo.friendLink)
                  alert("你的連結為：" + vueModel.playerInfo.friendLink);
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
                var date = new Date(remainSecond*1000)
                vueModel.temp.remainTimeFormat = date.getUTCHours()+":"+date.getUTCMinutes()+":"+date.getUTCSeconds()
            }
            setInterval(update, 1000)
        }
        start()
    }
    module.init = init
})(controller)