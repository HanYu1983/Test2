pragma solidity ^0.4.24;

import "truffle/Assert.sol";
import "../contracts/YAMDAlg.sol";
import "../contracts/PartnerMgr.sol";
import "../contracts/lib/SafeMath.sol";

contract TestRound {
    using YAMDAlg for YAMDAlg.Data;
    using PartnerMgr for PartnerMgr.Data;
    using SafeMath for *;

    YAMDAlg.Data data;
    
    function beforeAll() public {
        data.init();
    }
    
    function testRound0StateAtStart() public {
        YAMDAlg.RoundInfo memory rndInfo;
        
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.rnd, 0, "rnd must be 0");
        Assert.equal(uint(rndInfo.state), 0, "state must be idle");
    }
    
    function testRound0StateAfterBuyOneKey() public {
        address user = msg.sender;
        YAMDAlg.RoundInfo memory rndInfo;
        
        data.buy(user, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(uint(rndInfo.state), 1, "state must be playing");
    }
    
    function testRound1StateAtStart() public {
        data.endRound();
        
        YAMDAlg.RoundInfo memory rndInfo;
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.rnd, 1, "rnd must be 1");
        Assert.equal(uint(rndInfo.state), 0, "state must be idle");
    }
    
    function testRound1StateAfterBuyOneKey() public {
        address user = msg.sender;
        YAMDAlg.RoundInfo memory rndInfo;
        
        data.buy(user, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(uint(rndInfo.state), 1, "state must be playing");
    }
    
    function testRound2StateAtStart() public {
        data.endRound();
        
        YAMDAlg.RoundInfo memory rndInfo;
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.rnd, 2, "rnd must be 2");
        Assert.equal(uint(rndInfo.state), 0, "state must be idle");
    }
    
    function testRound2StateAfterBuyOneKey() public {
        address user = msg.sender;
        YAMDAlg.RoundInfo memory rndInfo;
        
        data.buy(user, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(uint(rndInfo.state), 1, "state must be playing");
    }
    
    function testRound3StateAtStart() public {
        data.endRound();
        
        YAMDAlg.RoundInfo memory rndInfo;
        rndInfo = data.getRoundInfo();
        Assert.equal(rndInfo.rnd, 3, "rnd must be 3");
        Assert.equal(uint(rndInfo.state), 0, "state must be idle");
    }
    
    function testRound3StateAfterBuyOneKey() public {
        address user = msg.sender;
        YAMDAlg.RoundInfo memory rndInfo;
        
        data.buy(user, 1 ether, 0, 0);
        rndInfo = data.getRoundInfo();
        Assert.equal(uint(rndInfo.state), 1, "state must be playing");
    }
}