var Main = artifacts.require("./YAMDMain.sol");

contract('領先玩家', function(accounts) {
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
    
    it("han買1個鑽石", function(){
        var useMoney = oneEther;
        return main.buy({from: han, value: useMoney})
    })
    
    it("取得輪資料", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var lastPlyrId = ret[8].toNumber()
            assert.equal(lastPlyrId, 1, "領先玩家必須是1")
        });
    })
    
    it("marry買1個鑽石", function(){
        var useMoney = oneEther;
        return main.buy({from: marry, value: useMoney})
    })
    
    it("取得輪資料", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var lastPlyrId = ret[8].toNumber()
            assert.equal(lastPlyrId, 2, "領先玩家必須是2")
        });
    })
    
    it("john買1個鑽石", function(){
        var useMoney = oneEther;
        return main.buy({from: john, value: useMoney})
    })
    
    it("取得輪資料", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var lastPlyrId = ret[8].toNumber()
            assert.equal(lastPlyrId, 3, "領先玩家必須是3")
        });
    })
    
    it("han買1個鑽石", function(){
        var useMoney = oneEther;
        return main.buy({from: han, value: useMoney})
    })
    
    it("取得輪資料", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var lastPlyrId = ret[8].toNumber()
            assert.equal(lastPlyrId, 1, "領先玩家必須是1")
        });
    })
    
    it("marry買1個鑽石", function(){
        var useMoney = oneEther;
        return main.buy({from: marry, value: useMoney})
    })
    
    it("取得輪資料", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var lastPlyrId = ret[8].toNumber()
            assert.equal(lastPlyrId, 2, "領先玩家必須是2")
        });
    })
    
    it("john買1個鑽石", function(){
        var useMoney = oneEther;
        return main.buy({from: john, value: useMoney})
    })
    
    it("取得輪資料", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var lastPlyrId = ret[8].toNumber()
            assert.equal(lastPlyrId, 3, "領先玩家必須是3")
        });
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