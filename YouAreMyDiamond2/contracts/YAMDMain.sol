pragma solidity ^0.4.24;

import "./YAMDAlg.sol";
import "./PartnerMgr.sol";
import "./lib/SafeMath.sol";

/*interface IFowarder{
    function deposit() external payable returns (bool);
}*/

contract YAMDMain {
    using YAMDAlg for YAMDAlg.Data;
    using PartnerMgr for PartnerMgr.Data;
    using SafeMath for *;
    
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
    function getInfo() public view returns (Phase){
        return (phase);
    }
    //
    // 分潤
    //
    uint constant DepositGate = 0;
    
    address public comAddr;
    function setComAddr(address addr) onlyOwner() public {
        comAddr = addr;
    }
    function depositToCom(YAMDAlg.Data storage data) private {
        if(comAddr == 0){
            return;
        }
        if(data.vaults[data.comVaultId] >= DepositGate){
            uint value = data.withdrawCom();
            comAddr.transfer(value);
            //require(IFowarder(comAddr).deposit.value(value)(), "depositToCom fail");
        }
    }
    
    address public pubAddr;
    function setPubAddr(address addr) onlyOwner() public {
        pubAddr = addr;
    }
    function depositToPub(YAMDAlg.Data storage data) private {
        if(pubAddr == 0){
            return;
        }
        if(data.vaults[data.pubVaultId] >= DepositGate){
            uint value = data.withdrawPub();
            pubAddr.transfer(value);
            //require(IFowarder(pubAddr).deposit.value(value)(), "depositToPub fail");
        }
    }
    
    function depositAuto(YAMDAlg.Data storage data) private {
        depositToCom(data);
        depositToPub(data);
    }
    // 
    // 主業務
    //
    YAMDAlg.Data data;
    
    event onMsg(bytes32 msg);
    
    function isCanRegisterPartner() public view returns (bool){
        address user = msg.sender;
        // 如果有人使用過你的推薦人連結，就沒辨法註冊合夥人
        if(data.hasFriendLinkPointToAddr(user)){
            return false;
        }
        /*
        bytes32 ignore;
        (bool isValid, bytes32 link) = data.partnerMgr.getLink(user);
        ignore = link;
        
        if(isValid){
            return false;
        }*/
        return true;
    }
    
    function registerPartner(uint level) isHuman() public payable{
        address user = msg.sender;
        uint value = msg.value;
        PartnerMgr.Project proj = PartnerMgr.Project.One;
        if(level == 2){
            proj = PartnerMgr.Project.Two;
        }
        if(isCanRegisterPartner() == false){
            // 還給玩家
            user.transfer(value);
            return;
        }
        if(data.partnerMgr.register(user, value, proj)){
            // 重要：新增玩家!!
            data.getOrNewPlayer(user);
            // TODO 錢要流向公司
        } else {
            // 還給玩家
            user.transfer(value);
            return;
        }
        emit onMsg("registerPartner");
    }
    
    function getPartnerProjectFee(uint level) public view returns (uint){
        PartnerMgr.Project proj = PartnerMgr.Project.One;
        if(level == 2){
            proj = PartnerMgr.Project.Two;
        }
        return PartnerMgr.projFee(data.partnerMgr, proj);
    }
    
    function buy() isHuman() onlyPhase(Phase.Open) public payable {
        address user = msg.sender;
        uint eth = msg.value;
        data.buy(user, eth, 0, 0);
        depositAuto(data);
        emit onMsg("buy");
    }
    
    function buyWithFriendLink(bytes32 friendLink) isHuman() onlyPhase(Phase.Open) public payable {
        address user = msg.sender;
        uint eth = msg.value;
        data.buy(user, eth, 0, friendLink);
        depositAuto(data);
        emit onMsg("buyWithFriendLink");
    }
    
    function vaultBuy(uint eth) isHuman() onlyPhase(Phase.Open) public {
        address user = msg.sender;
        data.buyWithVault(user, eth, 0, 0);
        depositAuto(data);
        emit onMsg("vaultBuy");
    }
    
    function vaultBuyWithFriendLink(uint eth, bytes32 friendLink) isHuman() onlyPhase(Phase.Open) public {
        address user = msg.sender;
        data.buyWithVault(user, eth, 0, friendLink);
        depositAuto(data);
        emit onMsg("vaultBuyWithFriendLink");
    }
    
    function depositManual() onlyOwner() public {
        depositAuto(data);
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
    
    function getPlayerInfo() public view returns (uint, uint, uint, uint, uint, bytes32, uint){
        YAMDAlg.PlayerInfo memory info = data.getPlayerInfo(msg.sender);
        return (
            info.key,
            info.winVault,
            info.genVault,
            info.friVault,
            info.parVault,
            info.friendLink,
            info.alreadyShareFromKey
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
    
    // common 
    modifier isHuman() {
        address _addr = msg.sender;
        uint256 _codeLength;
        
        assembly {_codeLength := extcodesize(_addr)}
        require(_codeLength == 0, "sorry humans only");
        _;
    }
    
    /*
    function getPartnerLink() private view returns (bool, bytes32){
        address user = msg.sender;
        return data.partnerMgr.getLink(user);
    }
    
    function buyWithPartnerLink(bytes32 partnerLink) onlyPhase(Phase.Open) private payable {
        address user = msg.sender;
        uint eth = msg.value;
        data.buy(user, eth, partnerLink, 0);
        emit onMsg("buyWithPartnerLink");
    }
    
    function vaultBuyWithPartnerLink(uint eth, bytes32 partnerLink) onlyPhase(Phase.Open) private {
        address user = msg.sender;
        data.buyWithVault(user, eth, partnerLink, 0);
        emit onMsg("vaultBuyWithPartnerLink");
    }*/
}