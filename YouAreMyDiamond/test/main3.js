var Main = artifacts.require("./YAMDMain.sol");

contract('輪數與輪狀態', function(accounts) {
    var oneEther = 1000000000000000000;
    var fixPointFactor = 1000000000;
    var han = accounts[0];
    var marry = accounts[1];
    var john = accounts[2];
    
    var main;
    it("開盤", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.nextPhase()
        })
    })
    
    it("輪數必須是0", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
            var rnd = ret[0].toNumber()
            assert.equal(rnd, 0, "輪數必須是0")
        });
    })
    
    it("狀態必須是0", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var state = ret[7].toNumber()
            assert.equal(state, 0, "起始狀態必須是0")
        })
    })
    
    it("買個鑽石", function(){
        var useMoney = oneEther;
        return main.buy({from: han, value: useMoney})
    })
    
    it("狀態必須是1", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var state = ret[7].toNumber()
            assert.equal(state, 1, "起始狀態必須是1")
        })
    })
    
    it("結束", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.endRound()
        })
    })
    
    it("輪數必須是1", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
            var rnd = ret[0].toNumber()
            assert.equal(rnd, 1, "輪數必須是1")
        });
    })
    
    
    
    it("狀態必須是0", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var state = ret[7].toNumber()
            assert.equal(state, 0, "起始狀態必須是0")
        })
    })
    
    it("買個鑽石", function(){
        var useMoney = oneEther;
        return main.buy({from: han, value: useMoney})
    })
    
    it("狀態必須是1", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var state = ret[7].toNumber()
            assert.equal(state, 1, "起始狀態必須是1")
        })
    })
    
    it("結束", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.endRound()
        })
    })
    
    it("輪數必須是2", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
            var rnd = ret[0].toNumber()
            assert.equal(rnd, 2, "輪數必須是2")
        });
    })
    
    
    
    it("狀態必須是0", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var state = ret[7].toNumber()
            assert.equal(state, 0, "起始狀態必須是0")
        })
    })
    
    it("買個鑽石", function(){
        var useMoney = oneEther;
        return main.buy({from: han, value: useMoney})
    })
    
    it("狀態必須是1", function(){
        return main.getRoundInfo().then(function(ret){
            logRoundInfo(ret)
            var state = ret[7].toNumber()
            assert.equal(state, 1, "起始狀態必須是1")
        })
    })
    
    it("結束", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.endRound()
        })
    })
    
    it("輪數必須是3", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
            var rnd = ret[0].toNumber()
            assert.equal(rnd, 3, "輪數必須是3")
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