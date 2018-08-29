pragma solidity ^0.4.24;

import "truffle/Assert.sol";
import "../contracts/YAMDAlg.sol";
import "../contracts/PartnerMgr.sol";
import "../contracts/lib/SafeMath.sol";

contract TestFns {
    using YAMDAlg for YAMDAlg.Data;
    using PartnerMgr for PartnerMgr.Data;
    using SafeMath for *;

    YAMDAlg.Data data;
    
    function beforeAll() public {
        data.init();
    }
    
    function testEndRound() public {
        data.endRound();
        data.endRound();
        data.endRound();
    }
    
    function testBuy() public {
        address user = msg.sender;
        // 朋友連結
        data.buy(user, 1 ether, 0, 0);
        data.buy(user, 1 ether, 0, 0);
        data.buy(user, 1 ether, 0, 0);
    }
    
    /*function testBuyWithPartnerLink() public {
        address user = msg.sender;
        bool isValid;
        bytes32 partnerLink;
        data.partnerMgr.register(user, PartnerMgr.projFee(data.partnerMgr, PartnerMgr.Project.One), PartnerMgr.Project.One);
        (isValid, partnerLink) = data.partnerMgr.getLink(user);
        data.buy(user, 1 ether, partnerLink, 0);
        data.buy(user, 1 ether, partnerLink, 0);
        data.buy(user, 1 ether, partnerLink, 0);
    }*/
    
    function testBuyWithFriendLink() public {
        address user = msg.sender;
        // 朋友連結
        data.buy(user, 1 ether, 0, data.getPlayerInfo(user).friendLink);
        data.buy(user, 1 ether, 0, data.getPlayerInfo(user).friendLink);
        data.buy(user, 1 ether, 0, data.getPlayerInfo(user).friendLink);
    }
    
    function testBuyWithOddPartnerLink() public {
        address user = msg.sender;
        bytes32 oddParnterLink;
        // 使用奇怪的合夥人連結買
        oddParnterLink = 0x08c379a000000000000000000000000000000000000000000000000000000000;
        data.buy(user, 1 ether, oddParnterLink, 0);
        data.buy(user, 1 ether, oddParnterLink, 0);
        data.buy(user, 1 ether, oddParnterLink, 0);
    }
    
    function testBuyWithVault() public { 
        address user = msg.sender;
        // bool isValid;
        // bytes32 partnerLink;
        // win the money
        data.endRound();
        
        //(isValid, partnerLink) = data.partnerMgr.getLink(user);
        // use vault
        data.buyWithVault(user, 1000000000000000, 0, 0);
        //data.buyWithVault(user, 1000000000000000, partnerLink, 0);
        data.buyWithVault(user, 1000000000000000, 0, data.getPlayerInfo(user).friendLink);
    }
    
    function testWithdraw() public {
        address user = msg.sender;
        data.withdraw(user);
        data.withdraw(user);
        data.withdraw(user);
    }
}