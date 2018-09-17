var model = model || {};
(function(module){
    
    var ContractAddress = "0x7d330d16de27bfe37663f46e2581837cf309b8e4"
    var contract = 0
    
    async function loadContract(){
        return $.getJSON('../contracts/YAMDMain.json', function(data) {
            var Clz = TruffleContract(data);
            Clz.setProvider(web3.currentProvider);
        
            contract = Clz.at(ContractAddress)
            console.log(contract)
        });
    }
    
    function getContract(){
        return contract
    }
    
    async function loadRoundInfo(){
        var info = await contract.getRoundInfo()
        var [rnd, startTime, endTime, remainTime, com, pot, pub, state, lastPlyrId, keyAmount, totalExtendTime] = info
        console.log(info)
        return {
            "rnd": rnd.toNumber(),
            "startTime": startTime.toNumber(),
            "endTime": endTime.toNumber(), 
            "remainTime": remainTime.toNumber(),
            "com": com.dividedBy(model.fixPointFactor*oneEther).toNumber(),
            "pot": pot.dividedBy(model.fixPointFactor*oneEther).toNumber(),
            "pub": pub.dividedBy(model.fixPointFactor*oneEther).toNumber(),
            "state": state.toNumber(),
            "lastPlyrId": lastPlyrId.toNumber(),
            "keyAmount": keyAmount.dividedBy(model.fixPointFactor).toNumber(),
            "totalExtendTime": totalExtendTime.toNumber()
        }
    }
    
    async function loadPlayerInfo(){
        var info = await contract.getPlayerInfo()
        var [key, win, gen, fri, par, friendLink, alreadyShareFromKey, eth, id] = info
        console.log(info)
        return {
            "key": key.dividedBy(model.fixPointFactor).toNumber(),
            "win": win.dividedBy(model.fixPointFactor*oneEther).toNumber(),
            "gen": gen.dividedBy(model.fixPointFactor*oneEther).toNumber(),
            "fri": fri.dividedBy(model.fixPointFactor*oneEther).toNumber(),
            "par": par.dividedBy(model.fixPointFactor*oneEther).toNumber(),
            "friendLink": friendLink,
            "alreadyShareFromKey": alreadyShareFromKey.dividedBy(model.fixPointFactor*oneEther).toNumber(),
            "eth": eth.dividedBy(model.fixPointFactor*oneEther).toNumber(),
            "id": id.toNumber()
        }
    }
    
    function addListener(obj){
        contract.onMsg().watch((err, result)=>{
            var msg = byteStr2str(result.args.msg)
            obj.onMsg(msg)
        })
    }
    
    module.loadContract = loadContract
    module.getContract = getContract
    module.loadRoundInfo = loadRoundInfo
    module.fixPointFactor = fixPointFactor
    module.loadPlayerInfo = loadPlayerInfo
    module.addListener = addListener
})(model)