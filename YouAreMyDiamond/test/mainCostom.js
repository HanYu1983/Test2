var Main = artifacts.require("./YAMDMain.sol");

contract('大略測試與輸出觀察', function(accounts) {
    var oneEther = 1000000000000000000;
    var fixPointFactor = 1000000000;
    var han = accounts[0];
    var marry = accounts[1];
    var john = accounts[2];
    
    var startRemainTime = 0
    var hanPartnerLink;
    
    it("開盤前", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("han註冊合夥人")
            return main.reigsterPartnerTwo({from: han, value: oneEther})
        }).then(function(){
            console.log("han註冊成功")
            return main.getPartnerLink({from: han})
        }).then(function(link){
            console.log("han的合夥人連結:", link)
            hanPartnerLink = link
        }).then(function(){
            console.log("開盤")
            return main.nextPhase()
        }).then(function(){
            console.log("開盤成功")
        })
    })
    
    var oneKeyPrice;
    
    it("輪起動前", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getRoundInfo({from: han})
        }).then(function(ret){
            var rnd = ret[0].toNumber()
            var remainTime = ret[3].toNumber()
            var state = ret[7].toNumber()
            //console.log(rnd, startTime, endTime, remainTime, pot, state)
            
            startRemainTime = remainTime
            assert.equal(rnd, 0, "輪數必須是0")
            assert.equal(state, 0, "起始狀態必須是0")
            
            return main.getKeyPrice(0.5*fixPointFactor)
        }).then(function(ret){
            var price = ret.toNumber()
            console.log("0.5鑽石價格:"+(price/oneEther))
            return main.getKeyPrice(1*fixPointFactor)
        }).then(function(ret){
            var price = ret.toNumber()
            console.log("1鑽石價格:"+(price/oneEther))
            oneKeyPrice = price;
        })
    })
    
    var hanFriendLink;
    it("han買1個鑽石", function(){
        var useMoney = oneKeyPrice;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("han投入"+(useMoney/oneEther))
            return main.buy({from: han, value: useMoney})
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
            var key = ret[0].toNumber()
            var friendLink = ret[5]
            var shouldBuyKey = useMoney/oneKeyPrice
            assert.equal(Math.floor(key/fixPointFactor), shouldBuyKey, "買的數量不符")
            hanFriendLink = friendLink
            return main.getKeyPrice(1*fixPointFactor)
        }).then(function(ret){
            var price = ret.toNumber()
            console.log("1鑽石價格:"+(price/oneEther))
        })
    })
    
    it("買鑽石後", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
            var rnd = ret[0].toNumber()
            var remainTime = ret[3].toNumber()
            var state = ret[7].toNumber()
            
            var addedTime = remainTime - startRemainTime
            assert.equal(rnd, 0, "輪數必須是0")
            //assert.equal(addedTime, 60, "剩餘時間必須增加60秒")
            assert.equal(state, 1, "有人買鑽石後狀態必須是1")
        })
    })
    
    it("路續買1個鑽石", function(){
        var useMoney = oneKeyPrice;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("marry投入"+(useMoney/oneEther))
            return main.buy({from: marry, value: useMoney})
        }).then(function(){
            console.log("john投入"+(useMoney/oneEther))
            return main.buy({from: john, value: useMoney})
        }).then(function(){
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
        }).then(function(){
            return main.getPlayerInfo({from: marry})
        }).then(function(ret){
            logPlayerInfo("marry:", ret)
        }).then(function(){
            return main.getPlayerInfo({from: john})
        }).then(function(ret){
            logPlayerInfo("john:", ret)
        })
    })
    
    it("路續買1個鑽石", function(){
        var useMoney = 1000000000000000000;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("marry用合夥人連結投入"+(useMoney/oneEther))
            return main.buyWithPartnerLink(hanPartnerLink, {from: marry, value: useMoney})
        }).then(function(){
            console.log("john用合夥人連結投入"+(useMoney/oneEther))
            return main.buyWithPartnerLink(hanPartnerLink, {from: john, value: useMoney})
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
        })
    })
    
    it("路續買1個鑽石", function(){
        var useMoney = 1000000000000000000;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("marry用推薦人連結投入"+(useMoney/oneEther))
            return main.buyWithFriendLink(hanFriendLink, {from: marry, value: useMoney})
        }).then(function(){
            console.log("john用推薦人連結投入"+(useMoney/oneEther))
            return main.buyWithFriendLink(hanFriendLink, {from: john, value: useMoney})
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
        })
    })
    
    it("結束這一輪", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("結束前")
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
            console.log("結束")
            return main.endRound();
        }).then(function(){
            console.log("結束後")
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
            var rnd = ret[0].toNumber()
            var remainTime = ret[3].toNumber()
            var state = ret[7].toNumber()
            assert.equal(rnd, 1, "輪數必須是1")
            //assert.equal(addedTime, 60, "剩餘時間必須增加60秒")
            assert.equal(state, 0, "結束後狀態必須是0")
            return main.getPlayerInfo({from: john})
        }).then(function(ret){
            logPlayerInfo("john:", ret)
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
            return main.getPlayerInfo({from: marry})
        }).then(function(ret){
            logPlayerInfo("marry:", ret)
        })
    })
    
    it("路續買1個鑽石", function(){
        var useMoney = 1000000000000000000;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.buy({from: marry, value: useMoney})
        }).then(function(){
            return main.getRoundInfo()
        }).then(function(ret){
            var state = ret[7].toNumber()
            logRoundInfo(ret)
            
            assert.equal(state, 1, "起始狀態必須是1")
            return main.buy({from: john, value: useMoney})
        }).then(function(){
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
        })
    })
    
    it("結束這一輪", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("結束前")
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
            console.log("結束")
            return main.endRound();
        }).then(function(){
            console.log("結束後")
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
            var rnd = ret[0].toNumber()
            var remainTime = ret[3].toNumber()
            var state = ret[7].toNumber()
            assert.equal(rnd, 2, "輪數必須是2")
            //assert.equal(addedTime, 60, "剩餘時間必須增加60秒")
            assert.equal(state, 0, "結束後狀態必須是0")
            return main.getPlayerInfo({from: john})
        }).then(function(ret){
            logPlayerInfo("john:", ret)
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            logPlayerInfo("han:", ret)
            return main.getPlayerInfo({from: marry})
        }).then(function(ret){
            logPlayerInfo("marry:", ret)
        })
    })
    
    it("連續結束", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.endRound();
        }).then(function(){
            return main.endRound();
        }).then(function(ret){
            return main.endRound();
        }).then(function(){
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
        });
    })
    
    it("withdraw", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.withdraw({from: john})
        }).then(function(){
            return main.getPlayerInfo({from: john})
        }).then(function(ret){
            logPlayerInfo("john:", ret)
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