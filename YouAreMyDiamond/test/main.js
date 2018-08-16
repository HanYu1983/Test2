var Main = artifacts.require("./YAMDMain.sol");

contract('YAMDMain', function(accounts) {
    var oneEther = 1000000000000000000;
    var keyPriceAtStart = 100000000000000;
    var fixPointFactor = 1000;
    var han = accounts[0];
    var marry = accounts[1];
    var john = accounts[2];
    
    console.log(accounts)
    
    var startRemainTime = 0
    
    it("開盤", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getRoundInfo({from: han})
        }).then(function(ret){
            var rnd = ret[0].toNumber()
            var startTime = ret[1].toNumber()
            var endTime = ret[2].toNumber()
            var remainTime = ret[3].toNumber()
            var pot = ret[4].toNumber()
            var state = ret[5].toNumber()
            //console.log(rnd, startTime, endTime, remainTime, pot, state)
            
            startRemainTime = remainTime
            assert.equal(rnd, 0, "輪數必須是0")
            assert.equal(state, 0, "起始狀態必須是0")
        })
    })
    
    it("han買1個鑽石", function(){
        var useMoney = 1000000000000000000;
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("han投入"+(useMoney/oneEther))
            return main.buy({from: han, value: useMoney})
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            var key = ret[0].toNumber()
            var shouldBuyKey = useMoney/keyPriceAtStart
            //assert.equal(Math.floor(key/fixPointFactor), shouldBuyKey, "買的數量不符")
        })
    })
    
    it("買鑽石後", function(){
        return Main.deployed().then(function(ins){
            main = ins;
            return main.getRoundInfo()
        }).then(function(ret){
            var rnd = ret[0].toNumber()
            var startTime = ret[1].toNumber()
            var endTime = ret[2].toNumber()
            var remainTime = ret[3].toNumber()
            var pot = ret[4].toNumber()
            var state = ret[5].toNumber()
            //console.log(rnd, startTime, endTime, remainTime, pot, state)
            console.log("彩池"+(pot/oneEther/fixPointFactor))
            
            
            var addedTime = remainTime - startRemainTime
            assert.equal(rnd, 0, "輪數必須是0")
            //assert.equal(addedTime, 60, "剩餘時間必須增加60秒")
            assert.equal(state, 1, "有人買鑽石後狀態必須是1")
        })
    })
    
    it("路續買1個鑽石", function(){
        var useMoney = 1000000000000000000;
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
            var rnd = ret[0].toNumber()
            var startTime = ret[1].toNumber()
            var endTime = ret[2].toNumber()
            var remainTime = ret[3].toNumber()
            var pot = ret[4].toNumber()
            var state = ret[5].toNumber()
            //console.log(rnd, startTime, endTime, remainTime, pot, state)
            console.log("彩池"+(pot/oneEther/fixPointFactor))
        }).then(function(){
            return main.getPlayerInfo({from: han})
        }).then(function(ret){
            var key = ret[0].toNumber()
            var win = ret[1].toNumber()
            var gen = ret[2].toNumber()
            var fri = ret[3].toNumber()
            var par = ret[4].toNumber()
            console.log("han:", key/fixPointFactor, win/oneEther/fixPointFactor, gen/oneEther/fixPointFactor, fri/oneEther/fixPointFactor, par/oneEther/fixPointFactor)
        }).then(function(){
            return main.getPlayerInfo({from: marry})
        }).then(function(ret){
            var key = ret[0].toNumber()
            var win = ret[1].toNumber()
            var gen = ret[2].toNumber()
            var fri = ret[3].toNumber()
            var par = ret[4].toNumber()
            console.log("marry:", key/fixPointFactor, win/oneEther/fixPointFactor, gen/oneEther/fixPointFactor, fri/oneEther/fixPointFactor, par/oneEther/fixPointFactor)
        }).then(function(){
            return main.getPlayerInfo({from: john})
        }).then(function(ret){
            var key = ret[0].toNumber()
            var win = ret[1].toNumber()
            var gen = ret[2].toNumber()
            var fri = ret[3].toNumber()
            var par = ret[4].toNumber()
            console.log("john:", key/fixPointFactor, win/oneEther/fixPointFactor, gen/oneEther/fixPointFactor, fri/oneEther/fixPointFactor, par/oneEther/fixPointFactor)
        })
    })
    
    var hanPartnerLink;
    
    it("合夥人", function(){
        var main;
        return Main.deployed().then(function(ins){
            main = ins;
            console.log("han註冊合夥人")
            return main.reigsterPartnerTwo({from: han, value: 2*oneEther})
        }).then(function(){
            console.log("han註冊成功")
            return main.getPartnerLink({from: han})
        }).then(function(link){
            console.log("han的合夥人連結:", link)
            hanPartnerLink = link
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
            var key = ret[0].toNumber()
            var win = ret[1].toNumber()
            var gen = ret[2].toNumber()
            var fri = ret[3].toNumber()
            var par = ret[4].toNumber()
            console.log("han:", key/fixPointFactor, win/oneEther/fixPointFactor, gen/oneEther/fixPointFactor, fri/oneEther/fixPointFactor, par/oneEther/fixPointFactor)
        })
    })
});