var SafeMath = artifacts.require("./SafeMath.sol");
var KeysCalc = artifacts.require("./KeysCalc.sol");
var PartnerMgr = artifacts.require("./PartnerMgr.sol");
var YAMDAlg = artifacts.require("./YAMDAlg.sol");
var YAMDMain = artifacts.require("./YAMDMain.sol");

module.exports = function(deployer) {
    deployer.deploy(SafeMath).then(function(obj){
        deployer.link(SafeMath, KeysCalc)
        return deployer.deploy(KeysCalc)
    }).then(function(obj){
        deployer.link(SafeMath, PartnerMgr)
        return deployer.deploy(PartnerMgr)
    }).then(function(obj){
        deployer.link(SafeMath, YAMDAlg)
        deployer.link(KeysCalc, YAMDAlg)
        deployer.link(PartnerMgr, YAMDAlg)
        return deployer.deploy(YAMDAlg)
    }).then(function(obj){
        deployer.link(YAMDAlg, YAMDMain)
        deployer.link(PartnerMgr, YAMDMain)
        return deployer.deploy(YAMDMain)
    })
};
