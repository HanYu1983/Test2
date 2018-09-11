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
}