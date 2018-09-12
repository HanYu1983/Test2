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
    
    uint constant fixPointFactor = 1 ether;
    
    function beforeAll() public {
        data.init();
    }
    
    function testOneByOne() public {
        address user = msg.sender;
        uint oneKeyPrice;
        YAMDAlg.PlayerInfo memory plyrInfo;
        uint i;
        
        for(i=0; i<10; ++i){
            oneKeyPrice = YAMDAlg.calcKeyPrice(data, 1*fixPointFactor);
            data.buy(user, oneKeyPrice, 0, 0);
            plyrInfo = data.getPlayerInfo(user);
            Assert.equal((plyrInfo.key/fixPointFactor) == (i+1), true, "買的數量不符");
        }
    }
}