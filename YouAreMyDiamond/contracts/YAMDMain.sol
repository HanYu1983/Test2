pragma solidity ^0.4.24;

import "./YAMDAlg.sol";
import "./PartnerMgr.sol";

contract YAMDMain {
    using YAMDAlg for YAMDAlg.Data;
    using PartnerMgr for PartnerMgr.Data;
    
    address public owner;
    modifier onlyOwner(){
        require(msg.sender == owner, "must owner");
        _;
    }
    
    constructor() public {
        owner = msg.sender;
        data.init();
    }
    // 
    // 階段
    //
    enum Phase {
        Before, Open
    }
    Phase phase;
    modifier onlyPhase(Phase p){
        require(phase == p, "phase not correct");
        _;
    }
    
    function nextPhase() onlyOwner() public {
        if(phase == Phase.Before){
            phase = Phase.Open;
            PartnerMgr.open(data.partnerMgr);
        }
    }
    // 
    // 主業務
    //
    YAMDAlg.Data data;
    
    function reigsterPartner(uint level) public payable{
        address user = msg.sender;
        uint value = msg.value;
        PartnerMgr.Project proj = PartnerMgr.Project.One;
        if(level == 2){
            proj = PartnerMgr.Project.Two;
        }
        data.partnerMgr.register(user, value, proj);
    }
    
    function getPartnerProjectFee(uint level) public view returns (uint){
        PartnerMgr.Project proj = PartnerMgr.Project.One;
        if(level == 2){
            proj = PartnerMgr.Project.Two;
        }
        return PartnerMgr.projFee(data.partnerMgr, proj);
    }
    
    function getPartnerLink() public view returns (bytes32){
        address user = msg.sender;
        return data.partnerMgr.getLink(user);
    }
    
    function buy() onlyPhase(Phase.Open) public payable {
        address user = msg.sender;
        uint eth = msg.value;
        data.buy(user, eth, 0, 0);
    }
    
    function buyWithPartnerLink(bytes32 partnerLink) onlyPhase(Phase.Open) public payable {
        address user = msg.sender;
        uint eth = msg.value;
        data.buy(user, eth, partnerLink, 0);
    }
    
    function buyWithFriendLink(bytes32 friendLink) onlyPhase(Phase.Open) public payable {
        address user = msg.sender;
        uint eth = msg.value;
        data.buy(user, eth, 0, friendLink);
    }
    
    function vaultBuy(uint eth) onlyPhase(Phase.Open) public {
        address user = msg.sender;
        data.buyWithVault(user, eth, 0, 0);
    }
    
    function vaultBuyWithPartnerLink(uint eth, bytes32 partnerLink) onlyPhase(Phase.Open) public {
        address user = msg.sender;
        data.buyWithVault(user, eth, partnerLink, 0);
    }
    
    function vaultBuyWithFriendLink(uint eth, bytes32 friendLink) onlyPhase(Phase.Open) public {
        address user = msg.sender;
        data.buyWithVault(user, eth, 0, friendLink);
    }
    
    function getKeyPrice(uint keyAmount) public view returns (uint){
        return YAMDAlg.calcKeyPrice(data, keyAmount);
    }
    
    function getRoundInfo() public view returns (uint,uint,uint,uint,uint,uint,uint,YAMDAlg.GameState,uint,uint){
        YAMDAlg.RoundInfo memory info = data.getRoundInfo();
        return (
            info.rnd,
            info.startTime,
            info.endTime,
            info.remainTime,
            info.comVault,
            info.potVault,
            info.pubVault,
            info.state,
            info.lastPlyrId,
            info.keyAmount
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
    
    function endRound() onlyOwner() public {
        data.endRound();
    }
    
    function kill() onlyOwner() public {
        selfdestruct(msg.sender);
    }
}