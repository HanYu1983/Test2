pragma solidity ^0.4.24;

import "truffle/Assert.sol";
import "../contracts/YAMDAlg.sol";
import "../contracts/PartnerMgr.sol";
import "../contracts/lib/SafeMath.sol";

contract TestPartner {
    using YAMDAlg for YAMDAlg.Data;
    using PartnerMgr for PartnerMgr.Data;
    using SafeMath for *;

    YAMDAlg.Data data;
    
    function beforeAll() public {
        data.init();
    }
    
    function testPartnerInfo() public {
        address han = 0x1234123412;
        PartnerMgr.Partner memory partner;
        
        partner = data.partnerMgr.getPartner(han);
        Assert.equal(partner.proj == PartnerMgr.Project.Unknow, true, "must has no proj");
        
        data.partnerMgr.register(han, PartnerMgr.projFee(data.partnerMgr, PartnerMgr.Project.Two), PartnerMgr.Project.Two);
        partner = data.partnerMgr.getPartner(han);
        Assert.equal(partner.proj == PartnerMgr.Project.Two, true, "must has proj two");
    }
    
    function testHasFriendLink() public {
        address user1 = 0x1000;
        address user2 = 0x2000;
        
        data.getOrNewPlayer(user1);
        
        Assert.equal(data.hasFriendLinkPointToAddr(user1), false, "must no has friend link point to");
        data.buy(user2, 1 ether, 0, data.plyrs[data.getPlayerId(user1)].friendLink);
        
        Assert.equal(data.hasFriendLinkPointToAddr(user1), true, "must has friend link point to");
    }
    
    function testCircleFriendLinkWithoutCrash() public {
        address userE = 0x4;
        address userF = 0x5;
        address userG = 0x6;
        address userH = 0x7;
        address userI = 0x8;
        
        // 先新增玩家!!
        data.getOrNewPlayer(userE);
        data.buy(userF, 1 ether, 0, data.plyrs[data.getPlayerId(userE)].friendLink);
        data.buy(userE, 1 ether, 0, data.plyrs[data.getPlayerId(userF)].friendLink);
        
        // 先新增玩家!!
        data.getOrNewPlayer(userG);
        data.buy(userH, 1 ether, 0, data.plyrs[data.getPlayerId(userG)].friendLink);
        data.buy(userI, 1 ether, 0, data.plyrs[data.getPlayerId(userH)].friendLink);
        data.buy(userG, 1 ether, 0, data.plyrs[data.getPlayerId(userI)].friendLink);
    }
    
    function testPartnerShare() public {
        address user1 = 0x100000;
        address user2 = 0x200000;
        bool isValid;
        uint lastParVault;
        uint currParVault;
        uint rootPlayerId;
        
        isValid = data.partnerMgr.register(user1, PartnerMgr.projFee(data.partnerMgr, PartnerMgr.Project.Two), PartnerMgr.Project.Two);
        Assert.equal(isValid, true, "must register success");
        data.getOrNewPlayer(user1);
        
        rootPlayerId = data.testCalcRootPlayerId(data.plyrs[data.getPlayerId(user1)].friendLink);
        Assert.equal(rootPlayerId, data.getPlayerId(user1), "root player id must be user1 player id");
        Assert.equal(data.plyrs[rootPlayerId].addr, user1, "root player address must be user1");
        Assert.equal(data.partnerMgr.getPartner(user1).proj == PartnerMgr.Project.Two, true, "user1 proj must be TWO");
        Assert.equal(data.partnerMgr.getPartner(data.plyrs[rootPlayerId].addr).proj == PartnerMgr.Project.Two, true, "root player proj must be TWO");
        
        lastParVault = data.vaults[data.plyrs[data.getPlayerId(user1)].parVaultId];
        
        data.buy(user2, 1 ether, 0, data.plyrs[data.getPlayerId(user1)].friendLink);
        Assert.equal(data.plyrs[data.getPlayerId(user2)].usedFriendLink, data.plyrs[data.getPlayerId(user1)].friendLink, "link must be set");
        Assert.equal(data.lastPlyrId, data.getPlayerId(user2), "last player must be user2");
        
        currParVault = data.vaults[data.plyrs[data.getPlayerId(user1)].parVaultId];
        Assert.equal(currParVault > lastParVault, true, "parVault must plus");
        
        lastParVault = currParVault;
        
        data.endRound();
        currParVault = data.vaults[data.plyrs[data.getPlayerId(user1)].parVaultId];
        Assert.equal(currParVault > lastParVault, true, "parVault must plus");
    }
}