pragma solidity ^0.4.24;

import "truffle/Assert.sol";
import "../contracts/YAMDAlg.sol";
import "../contracts/PartnerMgr.sol";
import "../contracts/lib/SafeMath.sol";

contract TestLeadPlayer {
    using YAMDAlg for YAMDAlg.Data;
    using PartnerMgr for PartnerMgr.Data;
    using SafeMath for *;

    YAMDAlg.Data data;
    
    function beforeAll() public {
        data.init();
    }
    
    function test() public {
        address user = msg.sender;
        address user2 = 0x0;
        address user3 = 0x1;
        YAMDAlg.RoundInfo memory rndInfo;
        
        data.buy(user, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.lastPlyrId, 1, "lead plyrId must be 1");
        
        data.buy(user2, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.lastPlyrId, 2, "lead plyrId must be 2");
        
        data.buy(user3, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.lastPlyrId, 3, "lead plyrId must be 3");
        
        data.buy(user, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.lastPlyrId, 1, "lead plyrId must be 1");
        
        data.buy(user2, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.lastPlyrId, 2, "lead plyrId must be 2");
        
        data.buy(user3, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.lastPlyrId, 3, "lead plyrId must be 3");
        
        data.buy(user, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.lastPlyrId, 1, "lead plyrId must be 1");
        
        data.buy(user2, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.lastPlyrId, 2, "lead plyrId must be 2");
        
        data.buy(user3, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.lastPlyrId, 3, "lead plyrId must be 3");
    }
}