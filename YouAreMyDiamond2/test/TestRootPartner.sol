pragma solidity ^0.4.24;

import "truffle/Assert.sol";
import "../contracts/YAMDAlg.sol";
import "../contracts/PartnerMgr.sol";
import "../contracts/lib/SafeMath.sol";

contract TestRootPartner {
    using YAMDAlg for YAMDAlg.Data;
    using PartnerMgr for PartnerMgr.Data;
    using SafeMath for *;

    YAMDAlg.Data data;
    
    function beforeAll() public {
        data.init();
    }
    
    function test() public {
        address userA = 0x0;
        address userB = 0x1;
        address userC = 0x2;
        bool isValid;
        bytes32 userAPartnerLink;
        uint parVault;
        uint lastParVault;
        uint comVault;
        uint lastComVault;
        uint i;
        
        Assert.equal(PartnerMgr.comRate(PartnerMgr.Project.Two), 0, "comRate must be 0");
        
        isValid = data.partnerMgr.register(userA, PartnerMgr.projFee(data.partnerMgr, PartnerMgr.Project.Two), PartnerMgr.Project.Two);
        Assert.equal(isValid, true, "must register success");
        // 重要：新增玩家!!
        // 請注意YAMDMain.sol中的註冊方法中有沒有這行!!
        data.getOrNewPlayer(userA);
        
        (isValid, userAPartnerLink) = data.partnerMgr.getLink(userA);
        Assert.equal(isValid, true, "must can register");
        // 合夥人連結，會分紅給合夥人
        // 沒有使用推廌人連結，推薦人部分會分紅給公司
        data.buy(userB, 1 ether, userAPartnerLink, 0);
        parVault = data.vaults[data.plyrs[data.getPlayerId(userA)].parVaultId];
        comVault = data.vaults[data.comVaultId];
        
        Assert.equal(parVault>lastParVault, true, "parValut must plus");
        Assert.equal(comVault>lastComVault, true, "comVault must plus");
        
        lastParVault = parVault;
        lastComVault = comVault;
        
        // 合夥人下線的推薦人連結，會分紅給推薦人和頂部合夥人，不會分紅給公司
        // 沒有使用合夥人連結，會從推薦人找到合夥人
        for(i=0; i<3; ++i){
            data.buy(userC, 1 ether, 0, data.plyrs[data.getPlayerId(userB)].friendLink);
            parVault = data.vaults[data.plyrs[data.getPlayerId(userA)].parVaultId];
            comVault = data.vaults[data.comVaultId];
        
            Assert.equal(parVault>lastParVault, true, "parValut must plus");
            Assert.equal(comVault, lastComVault, "comVault must not plus");
        
            lastParVault = parVault;
            lastComVault = comVault;
        }
    }
}