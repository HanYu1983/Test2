var Main = artifacts.require("./YAMDMain.sol");

contract('使用遊戲錢包', function(accounts) {
    var oneEther = 1000000000000000000;
    var fixPointFactor = 1000000000;
    var han = accounts[0];
    var marry = accounts[1];
    var john = accounts[2];
    
    var oneKeyPrice;
    var main;
    it("開盤", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.nextPhase()
        })
    })
    
    it("使用1以太幣買", function(){
        var useMoney = oneEther;
        return main.buy({from: han, value: useMoney})
    })
    
    it("結束", function(){
        return main.endRound();
    })
    
    it("取得玩家資料", function(){
        return main.getPlayerInfo({from: han}).then(function(ret){
            logPlayerInfo("han:", ret);
        })
    })
    
    it("使用錢包買", function(){
        var useMoney = oneEther*0.01;
        console.log("使用:"+useMoney);
        return main.vaultBuy(useMoney, {from: han})
    })
    
    it("取得玩家資料", function(){
        return main.getPlayerInfo({from: han}).then(function(ret){
            logPlayerInfo("han:", ret);
            var gen = ret[2].toNumber()/fixPointFactor;
            var shildRemain = 90000000000000000;
            assert.equal(gen, shildRemain, "必須剩餘"+shildRemain)
        })
    })
    
    it("使用錢包買", function(){
        var useMoney = oneEther*0.01;
        console.log("使用:"+useMoney);
        return main.vaultBuy(useMoney, {from: han})
    })
    
    it("取得玩家資料", function(){
        return main.getPlayerInfo({from: han}).then(function(ret){
            logPlayerInfo("han:", ret);
            var gen = ret[2].toNumber()/fixPointFactor;
            var shildRemain = 80000000000000000;
            assert.equal(gen, shildRemain, "必須剩餘"+shildRemain)
        })
    })
    
    it("使用錢包買", function(){
        var useMoney = oneEther*0.01;
        console.log("使用:"+useMoney);
        return main.vaultBuy(useMoney, {from: han})
    })
    
    it("取得玩家資料", function(){
        return main.getPlayerInfo({from: han}).then(function(ret){
            logPlayerInfo("han:", ret);
            var gen = ret[2].toNumber()/fixPointFactor;
            var shildRemain = 70000000000000000;
            assert.equal(gen, shildRemain, "必須剩餘"+shildRemain)
        })
    })
    
    it("使用錢包買", function(){
        var useMoney = oneEther*0.01;
        console.log("使用:"+useMoney);
        return main.vaultBuy(useMoney, {from: han})
    })
    
    it("取得玩家資料", function(){
        return main.getPlayerInfo({from: han}).then(function(ret){
            logPlayerInfo("han:", ret);
            var gen = ret[2].toNumber()/fixPointFactor;
            var shildRemain = 60000000000000000;
            assert.equal(gen, shildRemain, "必須剩餘"+shildRemain)
        })
    })
    
    it("使用錢包買", function(){
        var useMoney = oneEther*0.01;
        console.log("使用:"+useMoney);
        return main.vaultBuy(useMoney, {from: han})
    })
    
    it("取得玩家資料", function(){
        return main.getPlayerInfo({from: han}).then(function(ret){
            logPlayerInfo("han:", ret);
            var gen = ret[2].toNumber()/fixPointFactor;
            var shildRemain = 50000000000000000;
            assert.equal(gen, shildRemain, "必須剩餘"+shildRemain)
        })
    })
    
    it("使用錢包買", function(){
        var useMoney = oneEther*999;
        console.log("使用:"+useMoney);
        return main.vaultBuy(useMoney, {from: han})
    })
    
    var lastKey;
    
    it("取得玩家資料", function(){
        return main.getPlayerInfo({from: han}).then(function(ret){
            logPlayerInfo("han:", ret);
            var key = ret[0].toNumber()
            var gen = ret[2].toNumber()/fixPointFactor;
            var shildRemain = 0;
            assert.equal(gen, shildRemain, "必須剩餘"+shildRemain)
            
            lastKey = key
        })
    })
    
    function logPlayerInfo(tag, ret){
        var key = ret[0].toNumber()
        var win = ret[1].toFormat()
        var gen = ret[2].toFormat()
        var fri = ret[3].toFormat()
        var par = ret[4].toFormat()
        var friendLink = ret[5]
        console.log(tag+"鑽石:", key/fixPointFactor)
        console.log(tag+"勝利:", win)
        console.log(tag+"分紅:", gen)
        console.log(tag+"推薦:", fri)
        console.log(tag+"合夥:", par)
        console.log(tag+"推薦人連結:"+friendLink)
    }
    
    function logRoundInfo(ret){
        var rnd = ret[0].toNumber()
        var startTime = ret[1].toNumber()
        var endTime = ret[2].toNumber()
        var remainTime = ret[3].toNumber()
        var com = ret[4].toNumber()
        var pot = ret[5].toNumber()
        var pub = ret[6].toNumber()
        var state = ret[7].toNumber()
        var lastPlyrId = ret[8].toNumber()
        var keyAmount = ret[9].toNumber()
        //console.log(rnd, startTime, endTime, remainTime, pot, state)
        console.log("輪數:", rnd)
        console.log("時間:", remainTime)
        console.log("公司:", (com/oneEther/fixPointFactor))
        console.log("彩池:", (pot/oneEther/fixPointFactor))
        console.log("公益:", (pub/oneEther/fixPointFactor))
        console.log("狀態:", state)
        console.log("最後玩家:", lastPlyrId)
        console.log("鑽石總數:", keyAmount/fixPointFactor)
    }
});