var Main = artifacts.require("./YAMDMain.sol");

contract('1顆鑽石價格與購買', function(accounts) {
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
    
    it("鑽石價格", function(){
        return Main.deployed().then(function(ins){
            return main.getKeyPrice(1*fixPointFactor)
        }).then(function(ret){
            var price = ret.toNumber()
            console.log("1鑽石價格:"+price)
            oneKeyPrice = price;
        })
    })
    
    it("han買1個鑽石", function(){
        var useMoney = oneKeyPrice;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("han投入"+useMoney)
            return main.buy({from: han, value: useMoney})
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
            var key = ret[0].toNumber()
            assert.equal((key/fixPointFactor) >= 1, true, "買的數量不符")
            return main.getKeyPrice(1*fixPointFactor)
        }).then(function(ret){
            var price = ret.toNumber()
            console.log("1鑽石價格:"+price)
            oneKeyPrice = price
        })
    })
    
    it("han買1個鑽石", function(){
        var useMoney = oneKeyPrice;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("han投入"+useMoney)
            return main.buy({from: han, value: useMoney})
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
            var key = ret[0].toNumber()
            assert.equal((key/fixPointFactor) >= 2, true, "買的數量不符")
            return main.getKeyPrice(1*fixPointFactor)
        }).then(function(ret){
            var price = ret.toNumber()
            console.log("1鑽石價格:"+price)
            oneKeyPrice = price
        })
    })
    
    it("han買1個鑽石", function(){
        var useMoney = oneKeyPrice;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("han投入"+useMoney)
            return main.buy({from: han, value: useMoney})
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
            var key = ret[0].toNumber()
            assert.equal((key/fixPointFactor) >= 3, true, "買的數量不符")
            return main.getKeyPrice(1*fixPointFactor)
        }).then(function(ret){
            var price = ret.toNumber()
            console.log("1鑽石價格:"+price)
            oneKeyPrice = price
        })
    })
    
    it("han買1個鑽石", function(){
        var useMoney = oneKeyPrice;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("han投入"+useMoney)
            return main.buy({from: han, value: useMoney})
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
            var key = ret[0].toNumber()
            assert.equal((key/fixPointFactor) >= 4, true, "買的數量不符")
            return main.getKeyPrice(1*fixPointFactor)
        }).then(function(ret){
            var price = ret.toNumber()
            console.log("1鑽石價格:"+price)
            oneKeyPrice = price
        })
    })
    
    it("han買1個鑽石", function(){
        var useMoney = oneKeyPrice;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("han投入"+useMoney)
            return main.buy({from: han, value: useMoney})
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
            var key = ret[0].toNumber()
            assert.equal((key/fixPointFactor) >= 5, true, "買的數量不符")
            return main.getKeyPrice(1*fixPointFactor)
        }).then(function(ret){
            var price = ret.toNumber()
            console.log("1鑽石價格:"+price)
            oneKeyPrice = price
        })
    })
    
    
    function logPlayerInfo(tag, ret){
        var key = ret[0].toNumber()
        var win = ret[1].toNumber()
        var gen = ret[2].toNumber()
        var fri = ret[3].toNumber()
        var par = ret[4].toNumber()
        var friendLink = ret[5]
        console.log(tag+"鑽石:", key/fixPointFactor)
        console.log(tag+"勝利:", win/oneEther/fixPointFactor)
        console.log(tag+"分紅:", gen/oneEther/fixPointFactor)
        console.log(tag+"推薦:", fri/oneEther/fixPointFactor)
        console.log(tag+"合夥:", par/oneEther/fixPointFactor)
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