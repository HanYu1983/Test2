pragma solidity ^0.4.24;

import "truffle/Assert.sol";
import "../contracts/YAMDAlg.sol";
import "../contracts/PartnerMgr.sol";
import "../contracts/lib/SafeMath.sol";

contract TestOneKeyAndBuy {
    using YAMDAlg for YAMDAlg.Data;
    using PartnerMgr for PartnerMgr.Data;
    using SafeMath for *;

    YAMDAlg.Data data;
    
    function beforeAll() public {
        data.init();
    }
    
    function testOneByOne() public {
        address user = msg.sender;
        uint oneKeyPrice;
        YAMDAlg.PlayerInfo memory plyrInfo;
        uint fixPointFactor = 1000000000;
        
        oneKeyPrice = YAMDAlg.calcKeyPrice(data, 1*fixPointFactor);
        data.buy(user, oneKeyPrice, 0, 0);
        plyrInfo = data.getPlayerInfo(user);
        Assert.equal((plyrInfo.key/fixPointFactor) >= 1, true, "買的數量不符");
        
        oneKeyPrice = YAMDAlg.calcKeyPrice(data, 1*fixPointFactor);
        data.buy(user, oneKeyPrice, 0, 0);
        plyrInfo = data.getPlayerInfo(user);
        Assert.equal((plyrInfo.key/fixPointFactor) >= 2, true, "買的數量不符");
        
        oneKeyPrice = YAMDAlg.calcKeyPrice(data, 1*fixPointFactor);
        data.buy(user, oneKeyPrice, 0, 0);
        plyrInfo = data.getPlayerInfo(user);
        Assert.equal((plyrInfo.key/fixPointFactor) >= 3, true, "買的數量不符");
        
        oneKeyPrice = YAMDAlg.calcKeyPrice(data, 1*fixPointFactor);
        data.buy(user, oneKeyPrice, 0, 0);
        plyrInfo = data.getPlayerInfo(user);
        Assert.equal((plyrInfo.key/fixPointFactor) >= 4, true, "買的數量不符");
        
        oneKeyPrice = YAMDAlg.calcKeyPrice(data, 1*fixPointFactor);
        data.buy(user, oneKeyPrice, 0, 0);
        plyrInfo = data.getPlayerInfo(user);
        Assert.equal((plyrInfo.key/fixPointFactor) >= 5, true, "買的數量不符");
    }
}