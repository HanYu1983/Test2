var Main = artifacts.require("./YAMDMain.sol");

contract('單1方法連續呼叫，測試程式內部錯誤', function(accounts) {
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
    it("開盤", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.nextPhase()
        })
    })
    it("開盤", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.nextPhase()
        })
    })
    
    it("結束", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.endRound()
        })
    })
    it("結束", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.endRound()
        })
    })
    it("結束", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.endRound()
        })
    })
    
    var hanFriendLink;
    it("取得han推薦人連結", function(){
        var useMoney = oneKeyPrice;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            var friendLink = ret[5]
            hanFriendLink = friendLink
        })
    })
    
    it("推薦人連結購買", function(){
        var useMoney = 1000000000000000000;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.buyWithFriendLink(hanFriendLink, {from: marry, value: useMoney})
        })
    })
    
    it("推薦人連結購買", function(){
        var useMoney = 1000000000000000000;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.buyWithFriendLink(hanFriendLink, {from: marry, value: useMoney})
        })
    })
    
    it("推薦人連結購買", function(){
        var useMoney = 1000000000000000000;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.buyWithFriendLink(hanFriendLink, {from: marry, value: useMoney})
        })
    })
    
    var parnterFee;
    it("取得註冊合夥人價格", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getPartnerProjectFee(2)
        }).then(function(ret){
            parnterFee = ret.toNumber()
            console.log("合夥人費用:"+parnterFee)
        })
    })
    
    
    var hanPartnerLink;
    it("註冊合夥人並取得連結", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("han註冊合夥人")
            return main.registerPartner(2, {from: han, value: parnterFee})
        }).then(function(){
            console.log("han註冊成功")
            return main.getPartnerLink({from: han})
        }).then(function(link){
            console.log("han的合夥人連結:", link)
            hanPartnerLink = link
        })
    })
    
    it("合夥人連結購買", function(){
        var useMoney = 1000000000000000000;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.buyWithPartnerLink(hanPartnerLink, {from: marry, value: useMoney})
        })
    })
    
    it("合夥人連結購買", function(){
        var useMoney = 1000000000000000000;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.buyWithPartnerLink(hanPartnerLink, {from: marry, value: useMoney})
        })
    })
    
    it("合夥人連結購買", function(){
        var useMoney = 1000000000000000000;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.buyWithPartnerLink(hanPartnerLink, {from: marry, value: useMoney})
        })
    })
    
    it("取款", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.withdraw({from: han})
        })
    })
    
    it("取款", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.withdraw({from: han})
        })
    })
    
    it("取款", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.withdraw({from: han})
        })
    })
    
    it("取得輪資料", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getRoundInfo()
        }).then(function(ret){
            logRoundInfo(ret)
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