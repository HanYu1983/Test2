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
    
    address constant userA = 0x0;
    address constant userB = 0x1;
    uint constant FixPointFactor = 1000000000;
    
    function testShareLimit() public {
        YAMDAlg.PlayerInfo memory plyrInfo;
        YAMDAlg.Player memory plyr;
        uint offset;
        
        data.buy(userA, 1 ether, 0, 0);
        data.buy(userB, 100 ether, 0, 0);
        
        plyrInfo = data.getPlayerInfo(userA);
        offset = 2 ether.sub(plyrInfo.alreadyShareFromKey/FixPointFactor);
        Assert.equal(offset < 10000, true, "share limit must limit to 2 ether");
        
        plyr = data.plyrs[data.getPlayerId(userA)];
        offset = 2 ether.sub(data.vaults[plyr.genVaultId]/FixPointFactor);
        Assert.equal(offset < 10000, true, "share limit must limit to 2 ether");
    }
}