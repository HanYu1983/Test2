pragma solidity ^0.4.24;

import "truffle/Assert.sol";
import "../contracts/YAMDAlg.sol";
import "../contracts/PartnerMgr.sol";
import "../contracts/lib/SafeMath.sol";

contract TestShare {
    using YAMDAlg for YAMDAlg.Data;
    using PartnerMgr for PartnerMgr.Data;
    using SafeMath for *;

    YAMDAlg.Data data;
    
    function beforeAll() public {
        data.init(); 
    }
    
    uint constant fixPointFactor = 1 ether;
    address constant userA = 0x0;
    address constant userB = 0x1;
    
    function testShareLimit() public {
        YAMDAlg.PlayerInfo memory plyrInfo;
        YAMDAlg.Player memory plyr;
        uint offset;
        
        // 先把錢提光
        data.withdraw(userA);
        
        data.buy(userA, 1 ether, 0, 0);
        data.buy(userB, 100 ether, 0, 0);
        
        plyrInfo = data.getPlayerInfo(userA);
        offset = 2 ether.sub(plyrInfo.alreadyShareFromKey/fixPointFactor);
        Assert.equal(offset < 10000, true, "share limit must limit to 2 ether");
        
        plyr = data.plyrs[data.getPlayerId(userA)];
        offset = 2 ether.sub(data.vaults[plyr.genVaultId]/fixPointFactor);
        Assert.equal(offset < 10000, true, "share limit must limit to 2 ether");
    }
    
    function testShareToCom() public {
        // 先把錢提光
        data.withdrawCom();
        
        uint eth = 1;
        uint nowEth;
        
        data.shareToCom(eth);
        nowEth = data.withdrawCom();
        
        Assert.equal(eth, nowEth, "nowEth must be 1");
    }
    
    function testShare() public {
        YAMDAlg.PlayerInfo memory plyrInfo;
        uint lastShare;
        uint currShare;
        
        // 先把錢提光
        data.withdraw(userA);
        
        plyrInfo = data.getPlayerInfo(userA);
        currShare = plyrInfo.alreadyShareFromKey;
        lastShare = currShare;
        
        data.buy(userA, 1 ether, 0, 0);
        plyrInfo = data.getPlayerInfo(userA);
        currShare = plyrInfo.alreadyShareFromKey;
        lastShare = currShare;
        
        data.buy(userA, 1 ether, 0, 0);
        plyrInfo = data.getPlayerInfo(userA);
        currShare = plyrInfo.alreadyShareFromKey;
        Assert.equal(currShare > lastShare, true, "share must plus");
        lastShare = currShare;
        
        data.buy(userA, 1 ether, 0, 0);
        plyrInfo = data.getPlayerInfo(userA);
        currShare = plyrInfo.alreadyShareFromKey;
        Assert.equal(currShare > lastShare, true, "share must plus");
        lastShare = currShare;
    }
}