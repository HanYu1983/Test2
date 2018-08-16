pragma solidity ^0.4.24;

import "./YAMDAlg.sol";
import "./PartnerMgr.sol";

contract YAMDMain {
    using YAMDAlg for YAMDAlg.Data;
    using PartnerMgr for PartnerMgr.Data;
    
    address owner;
    YAMDAlg.Data data;
    
    constructor() public {
        owner = msg.sender;
        data.init();
    }
    
    function reigsterPartnerOne() public payable{
        address user = msg.sender;
        uint value = msg.value;
        data.partnerMgr.register(user, value, PartnerMgr.Project.One);
    }
    
    function reigsterPartnerTwo() public payable{
        address user = msg.sender;
        uint value = msg.value;
        data.partnerMgr.register(user, value, PartnerMgr.Project.Two);
    }
    
    function getPartnerLink() public view returns (bytes32){
        address user = msg.sender;
        return data.partnerMgr.getLink(user);
    }
    
    function getFriendLink() public view returns (bytes32){
        address user = msg.sender;
        return data.partnerMgr.getLink(user);
    }
    
    function buy() public payable {
        address user = msg.sender;
        uint eth = msg.value;
        data.buy(user, eth, 0, 0);
    }
    
    function buyWithPartnerLink(bytes32 partnerLink) public payable {
        address user = msg.sender;
        uint eth = msg.value;
        data.buy(user, eth, partnerLink, 0);
    }
    
    function buyWithFriendLink(bytes32 friendLink) public payable {
        address user = msg.sender;
        uint eth = msg.value;
        data.buy(user, eth, 0, friendLink);
    }
    
    function getRoundInfo() public view returns (uint,uint,uint,uint,uint, YAMDAlg.GameState){
        YAMDAlg.RoundInfo memory info = data.getRoundInfo();
        return (
            info.rnd,
            info.startTime,
            info.endTime,
            info.remainTime,
            info.potVault,
            info.state
        );
    }
    
    function getPlayerInfo() public view returns (uint, uint, uint, uint, uint, bytes32){
        YAMDAlg.PlayerInfo memory info = data.getPlayerInfo(msg.sender);
        return (
            info.key,
            info.winVault,
            info.genVault,
            info.friVault,
            info.parVault,
            info.friendLink
        );
    }
    
    function withdraw() public{
        address user = msg.sender;
        uint eth = data.withdraw(user);
        user.transfer(eth);
    }
}