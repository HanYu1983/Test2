pragma solidity ^0.4.24;

// File: contracts/lib/SafeMath.sol

/**
 * @title SafeMath v0.1.9
 * @dev Math operations with safety checks that throw on error
 * change notes:  original SafeMath library from OpenZeppelin modified by Inventor
 * - added sqrt
 * - added sq
 * - added pwr 
 * - changed asserts to requires with error log outputs
 * - removed div, its useless
 */
library SafeMath {
    
    /**
    * @dev Multiplies two numbers, throws on overflow.
    */
    function mul(uint256 a, uint256 b) 
        internal 
        pure 
        returns (uint256 c) 
    {
        if (a == 0) {
            return 0;
        }
        c = a * b;
        require(c / a == b, "SafeMath mul failed");
        return c;
    }

    /**
    * @dev Integer division of two numbers, truncating the quotient.
    */
    function div(uint256 a, uint256 b) internal pure returns (uint256) {
        // assert(b > 0); // Solidity automatically throws when dividing by 0
        uint256 c = a / b;
        // assert(a == b * c + a % b); // There is no case in which this doesn't hold
        return c;
    }
    
    /**
    * @dev Subtracts two numbers, throws on overflow (i.e. if subtrahend is greater than minuend).
    */
    function sub(uint256 a, uint256 b)
        internal
        pure
        returns (uint256) 
    {
        require(b <= a, "SafeMath sub failed");
        return a - b;
    }

    /**
    * @dev Adds two numbers, throws on overflow.
    */
    function add(uint256 a, uint256 b)
        internal
        pure
        returns (uint256 c) 
    {
        c = a + b;
        require(c >= a, "SafeMath add failed");
        return c;
    }
    
    /**
     * @dev gives square root of given x.
     */
    function sqrt(uint256 x)
        internal
        pure
        returns (uint256 y) 
    {
        uint256 z = ((add(x,1)) / 2);
        y = x;
        while (z < y) 
        {
            y = z;
            z = ((add((x / z),z)) / 2);
        }
    }
    
    /**
     * @dev gives square. multiplies x by x
     */
    function sq(uint256 x)
        internal
        pure
        returns (uint256)
    {
        return (mul(x,x));
    }
    
    /**
     * @dev x to the power of y 
     */
    function pwr(uint256 x, uint256 y)
        internal 
        pure 
        returns (uint256)
    {
        if (x==0)
            return (0);
        else if (y==0)
            return (1);
        else 
        {
            uint256 z = x;
            for (uint256 i=1; i < y; i++)
                z = mul(z,x);
            return (z);
        }
    }
}

// File: contracts/lib/KeysCalc.sol

//==============================================================================
//  |  _      _ _ | _  .
//  |<(/_\/  (_(_||(_  .
//=======/======================================================================
library KeysCalc {
    using SafeMath for *;
    uint constant FixPointFactor = 1 ether;
    
    function fixPointFactor() internal pure returns (uint){
        return FixPointFactor;
    }
    /**
     * @dev calculates number of keys received given X eth 
     * @param _curEth current amount of eth in contract 
     * @param _newEth eth being spent
     * @return amount of ticket purchased
     */
    function keysRec(uint256 _curEth, uint256 _newEth)
        internal
        pure
        returns (uint256)
    {
        return(keys((_curEth).add(_newEth)).sub(keys(_curEth)));
    }
    
    /**
     * @dev calculates amount of eth received if you sold X keys 
     * @param _curKeys current amount of keys that exist 
     * @param _sellKeys amount of keys you wish to sell
     * @return amount of eth received
     */
    function ethRec(uint256 _curKeys, uint256 _sellKeys)
        internal
        pure
        returns (uint256)
    {
        return((eth(_curKeys)).sub(eth(_curKeys.sub(_sellKeys))));
    }

    /**
     * @dev calculates how many keys would exist with given an amount of eth
     * @param _eth eth "in contract"
     * @return number of keys that would exist
     */
    function keys(uint256 _eth) 
        internal
        pure
        returns(uint256)
    {
        return ((((((_eth).mul(FixPointFactor)).mul(312500000000000000000000000)).add(5624988281256103515625000000000000000000000000000000000000000000)).sqrt()).sub(74999921875000000000000000000000)) / (156250000);
    }
    
    /**
     * @dev calculates how much eth would be in contract given a number of keys
     * @param _keys number of keys "in contract" 
     * @return eth that would exists
     */
    function eth(uint256 _keys) 
        internal
        pure
        returns(uint256)  
    {
        return ((78125000).mul(_keys.sq()).add(((149999843750000).mul(_keys.mul(FixPointFactor))) / (2))) / ((FixPointFactor).sq());
    }
}

// File: contracts/PartnerMgr.sol

library PartnerMgr {
    using SafeMath for *;
    
    uint constant Fee1 = 1 ether/2;
    uint constant Fee2 = 1 ether;
    uint constant Fee1af = 1 ether;
    uint constant Fee2af = 2 ether;
    uint constant Rate1 = 2;
    uint constant Rate2 = 4;
    
    enum Project {
        Unknow, One, Two
    }
    
    struct Partner{
        address addr;
        Project proj;
        bytes32 link;
    }
    
    struct Data{
        bool open;
        mapping(address=>uint) partnerIdByAddr;
        mapping(bytes32=>uint) partnerIdByLink;
        Partner[] partners;
    }
    
    function init(Data storage data) internal {
        Partner memory ignore;
        data.partners.push(ignore);
    }
    
    function projFee(Data memory data, Project proj) internal pure returns (uint){
        require(proj != Project.Unknow, "you must select a project");
        if(data.open){
            if(proj == Project.One){
                return Fee1af;
            }
            if(proj == Project.Two){
                return Fee2af;
            }
        }else{
            if(proj == Project.One){
                return Fee1;
            }
            if(proj == Project.Two){
                return Fee2;
            }
        }
    }
    
    function partRate(Project proj) internal pure returns (uint){
      if(proj == Project.One){
          return Rate1;
      }
      if(proj == Project.Two){
          return Rate2;
      }
      return 0;
    }
    
    function comRate(Project proj) internal pure returns (uint){
      if(proj == Project.One){
          return Rate2.sub(Rate1);
      }
      if(proj == Project.Two){
          return Rate2.sub(Rate2);
      }
      return 0;
    }
    
    function getOrNewPartner(Data storage data, address addr) private returns (uint){
        uint id = data.partnerIdByAddr[addr];
        if(id != 0){
            return id;
        }
        id = data.partners.length;
        Partner memory part;
        data.partners.push(part);
        data.partnerIdByAddr[addr] = id;
        return id;
    }
    
    function register(Data storage data, address addr, uint value, Project proj) internal returns(bool) {
        uint fee = projFee(data, proj);
        require(value == fee, "eth not enougth");
        uint id = getOrNewPartner(data, addr);
        // 如果已註冊過，就直接將錢歸還
        if(data.partners[id].proj != Project.Unknow){
            return false;
        }
        bytes32 link = bytes32(now.add(id << 224));
        data.partners[id].addr = addr;
        data.partners[id].proj = proj;
        data.partners[id].link = link;
        data.partnerIdByLink[link] = id;
        return true;
    }
    
    function getLink(Data storage data, address addr) internal view returns (bytes32){
        uint id = data.partnerIdByAddr[addr];
        require(id != 0, "you must register");
        return data.partners[id].link;
    }
    
    function getPartner(Data storage data, address user, bytes32 link) internal view returns (Partner){
        uint id = data.partnerIdByLink[link];
        Partner memory partner =  data.partners[id];
        // 合夥人不能是自己
        if(partner.addr == user){
            return data.partners[0];
        }
        return partner;
    }
    
    function open(Data storage data) internal {
        data.open = true;
    }
}

// File: contracts/YAMDAlg.sol

library YAMDAlg {
    using SafeMath for *;
    using KeysCalc for *;
    using PartnerMgr for PartnerMgr.Data;
    
    uint8 constant ComRate = 4;
    uint8 constant Par1Rate = 2;
    uint8 constant Par2Rate = 4;
    uint8 constant FriRate = 10;
    uint8 constant GenRate = 62;
    uint8 constant PotRate = 20;
    
    uint8 constant PotWinRate = 50;
    uint8 constant PotLastOneRate = 30;
    uint8 constant PotParRate = 10;
    uint8 constant PotPubRate = 10;
    
    // 最後?%鑽石的分紅，用來測試用。值應該是1
    uint8 constant LastWinP = 1;
    
    uint constant KeyEthAtStart = 75 szabo;
    uint constant FixPointFactor = 1000000000;
    uint constant ExtendTime = 60 seconds;
    uint constant TimeAtStart = 5 minutes;
    uint constant MaxTime = 24 hours;
    
    struct Player{
        address addr;
        uint key;                   // 所買鑽石。固定點小數，回合結束必須歸零
        uint alreadyShareFromKey;   // 已取得的鑽石分紅。用來計算最大分紅，回合結束必須歸零
        bytes32 usedPartnerLink;    // 所使用的合夥人連結
        bytes32 usedFriendLink;    // 所使用的推薦人連結
        bytes32 friendLink;
        uint winVaultId;
        uint genVaultId;
        uint friVaultId;
        uint parVaultId;
    }
    
    struct History{
        uint plyrId;
        uint key;   // 固定點小數
    }
    
    struct Data {
        uint32 rnd;
        uint startTime;
        uint endTime;
        
        mapping (address=>uint) plyrIdByAddr;
        mapping (bytes32=>uint) plyrIdByFriendLink;
        Player[] plyrs;
        
        GameState state;
        uint lastPlyrId;
        History[] history;
        
        uint comVaultId;
        uint potVaultId;
        uint pubVaultId;
        uint totalVaultId;
        uint[] vaults;  // 固定點小數
        // 合夥人的資料
        // 為了好一點的程式碼，在YAMDAlg中只能呼叫getter
        PartnerMgr.Data partnerMgr;
    }
    
    enum GameState{
        Idle, Playing
    }
    
    function init(Data storage data) internal {
        require(ComRate + Par2Rate + FriRate + GenRate + PotRate == 100, "");
        require(PotWinRate + PotLastOneRate + PotParRate + PotPubRate == 100, "");
        
        data.comVaultId = genVaultId(data);
        data.potVaultId = genVaultId(data);
        data.totalVaultId = genVaultId(data);
        
        Player memory ignore;
        data.plyrs.push(ignore);
        
        data.partnerMgr.init();
    }
    
    function genVaultId(Data storage data) private returns (uint){
        uint id = data.vaults.length;
        data.vaults.push(0);
        return id;
    }
    
    function getPlayerId(Data storage data, address addr) internal view returns (uint) {
        return data.plyrIdByAddr[addr];
    }
    
    function assignVaults(Data storage data, Player memory player) internal returns (Player){
        player.winVaultId = genVaultId(data);
        player.genVaultId = genVaultId(data);
        player.friVaultId = genVaultId(data);
        player.parVaultId = genVaultId(data);
        return player;
    }
    
    function getOrNewPlayer(Data storage data, address addr) internal returns (uint) {
        uint id = data.plyrIdByAddr[addr];
        if(id != 0){
            return id;
        }
        id = data.plyrs.length;
        data.plyrIdByAddr[addr] = id;

        Player memory plyr;
        plyr = assignVaults(data, plyr);
        plyr.friendLink = bytes32(now + (id << 224));
        
        data.plyrs.push(plyr);
        data.plyrIdByFriendLink[plyr.friendLink] = id;
        return id;
    }
  
    function getTotalKeyAmount(Data memory data) internal pure returns (uint){
        uint i;
        uint total;
        for(i=1; i<data.plyrs.length; ++i){
            total += data.plyrs[i].key;
        }
        return total;
    }
    
    function calcKeyAmount(uint totalValue, uint value) internal pure returns (uint){
        //return value.keys().mul(FixPointFactor)/KeysCalc.fixPointFactor();
        uint totalEth = totalValue;
        uint keyAmountWithCalcFixPoint = totalEth.keysRec(value);
        uint format = keyAmountWithCalcFixPoint.mul(FixPointFactor)/KeysCalc.fixPointFactor();
        return format;
    }
    
    function calcKeyPrice(Data memory data, uint keyAmount) internal pure returns (uint){
        //return ((keyAmount * KeysCalc.fixPointFactor())/FixPointFactor).eth();
        uint totalKey = getTotalKeyAmount(data).add(keyAmount);
        uint format = (totalKey * KeysCalc.fixPointFactor())/FixPointFactor;
        return format.ethRec(keyAmount.mul(KeysCalc.fixPointFactor())/FixPointFactor);
    }
    
    function reduceHistory(Data storage data) internal {
        // 最後一筆必須留下來
        if(data.history.length <= 1){
            return;
        }
        // 最後1%鑽石
        uint lastOneTotalKeys = getTotalKeyAmount(data).mul(LastWinP)/100;
        uint currKey = 0;
        uint i;
        for(i=data.history.length-1;; --i){
            // 佔最後1%鑽石的比例
            currKey += data.history[i].key;
            if(currKey >= lastOneTotalKeys){
                break;
            }
            if(i == 0){
                break;
            }
        }
        if(i == 0){
            return;
        }
        uint copyStart = i;
        uint finalLen = data.history.length - i;
        // 最後一筆必須留下來
        if(finalLen == 0){
            return;
        }
        for(i=0; i<finalLen; ++i){
            data.history[i] = data.history[i+copyStart];
        }
        data.history.length = finalLen;
    }
    
    struct buyLocal{
        uint plyrId;
        Player plyr;
        uint friendId;
        uint com;
        uint pot;
        uint gen;
        uint par;
        uint fri;
        uint totalKey;
        uint genPerKey;
        uint i;
        uint keyAmount;
        PartnerMgr.Partner partner;
        uint maxShareFromKey;
        uint shareToPlyr;
        uint shareToCom;
    }
    
    function buy(Data storage data, address addr, uint value, bytes32 partnerLink, bytes32 friendLink) internal returns(uint8) {
        // 不能低於最低價
        require(value >= KeyEthAtStart, "value must >= KeyEthAtStart");
        buyLocal memory local;
        // 先判斷時間是否結束，分配彩金
        if(data.state == GameState.Playing){
            RoundInfo memory info = getRoundInfo(data);
            if(info.remainTime == 0){
                local.plyrId = getOrNewPlayer(data, addr);
                local.plyr = data.plyrs[local.plyrId];
                // 如果時間已結束，直接將使用者花的錢存到錢包中
                // 就當這次沒買鑽石了
                data.vaults[local.plyr.genVaultId] = data.vaults[local.plyr.genVaultId].add(value.mul(FixPointFactor));
                endRound(data);
                return;
            }
        }
        // 買到的鑽石，有小數點
        local.keyAmount = calcKeyAmount(data.vaults[data.totalVaultId] / FixPointFactor, value);
        // 取得玩家
        local.plyrId = getOrNewPlayer(data, addr);
        local.plyr = data.plyrs[local.plyrId];
        local.plyr.addr = addr;
        // 只能使用之前綁定的 
        if(local.plyr.usedPartnerLink == 0){
            local.plyr.usedPartnerLink = partnerLink;
        }
        // 只能使用之前綁定的
        if(local.plyr.usedFriendLink == 0){
            local.plyr.usedFriendLink = friendLink;
        }
        // 增加鑽石
        local.plyr.key = local.plyr.key.add(local.keyAmount);
        // 套用新資料!
        // 很多事情要在這行之後處理!
        data.plyrs[local.plyrId] = local.plyr;
        
        // 買超過1個鑽石才有勝利的機會
        if(local.keyAmount / FixPointFactor >= 1){
            data.lastPlyrId = local.plyrId;
        }
        // 處理歷史訂單，這裡用來做最後1%鑽石的分紅
        data.history.push(History(local.plyrId, local.keyAmount));
        // 只保留最後1%的歷史訂單
        reduceHistory(data);
        
        // 處理合夥人
        local.partner = data.partnerMgr.getPartner(addr, local.plyr.usedPartnerLink);
        if(local.partner.proj == PartnerMgr.Project.Unknow){
            // 如果不是合夥人的下線
            // 分紅給公司
            local.com = value.mul(ComRate).mul(FixPointFactor)/100;
        }else{
            // 如果是合夥人的下線（用合夥人提供的連結玩遊戲）
            // 依專案分紅給公司和合夥人
            local.com = value.mul(PartnerMgr.comRate(local.partner.proj)).mul(FixPointFactor)/100;
            local.par = value.mul(PartnerMgr.partRate(local.partner.proj)).mul(FixPointFactor)/100;
        }
        // 彩池
        local.pot = value.mul(PotRate).mul(FixPointFactor)/100;
        // 鑽石回饋
        local.gen = value.mul(GenRate).mul(FixPointFactor)/100;
        // 推薦人
        local.fri = value.mul(FriRate).mul(FixPointFactor)/100;
        // 套用分紅
        // 公司
        data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(local.com);
        // 彩池
        data.vaults[data.potVaultId] = data.vaults[data.potVaultId].add(local.pot);
        // 合夥人
        if(local.partner.proj != PartnerMgr.Project.Unknow){
            local.plyrId = data.plyrIdByAddr[local.partner.addr];
            if(local.plyrId != 0){
                // 若合夥人存在就分給合夥人
                local.plyr = data.plyrs[local.plyrId];
                data.vaults[local.plyr.parVaultId] = data.vaults[local.plyr.parVaultId].add(local.par);
            } else {
                // 該存在的合夥人不存在就分給公司。合理的情況這裡應該不會運行
                data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(local.par);
            }
        }
        // 推薦人
        local.plyrId = getOrNewPlayer(data, addr);
        local.plyr = data.plyrs[local.plyrId];
        local.friendId = data.plyrIdByFriendLink[local.plyr.usedFriendLink];
        if(local.friendId != 0){
            local.plyr = data.plyrs[local.friendId];
            if(local.plyr.addr == addr){
                // 推薦人不能是自己
                // 分給公司
                data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(local.fri);
            } else {
                data.vaults[local.plyr.friVaultId] = data.vaults[local.plyr.friVaultId].add(local.fri);
            }
        } else {
            // 若推薦人不存在就分給公司
            data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(local.fri);
        }
        
        // 鑽石回饋
        // ver1.不保含這次買的key
        local.totalKey = getTotalKeyAmount(data).sub(local.plyr.key);
        // ver2.減掉自己的key, 因為自己不分紅
        // local.plyrId = getOrNewPlayer(data, addr);
        // local.plyr = data.plyrs[local.plyrId];
        // local.totalKey = getTotalKeyAmount(data).sub(local.plyr.key);
        // 第一個買會使totalKey等於0, 略過第一個買的(沒有分紅的對象)
        if(local.totalKey > 0){
            local.genPerKey = (local.gen / local.totalKey).mul(FixPointFactor);
            uint genPlus;
            for(local.i=1; local.i<data.plyrs.length; ++local.i){
                local.plyr = data.plyrs[local.i];
                if(local.plyr.addr == addr){
                    // ver1.如果是自己，減掉今次買的。這樣計算才正確
                    genPlus = (local.genPerKey * local.plyr.key.sub(local.keyAmount)) / FixPointFactor;
                    // ver2.自己不分紅
                    // nothing to do
                }else{
                    // 這個玩家的鑽石分紅
                    genPlus = (local.genPerKey * local.plyr.key) / FixPointFactor;
                }
                local.shareToPlyr = genPlus;
                // 玩家最大鑽石分紅為所買鑽石的2倍
                local.maxShareFromKey = (local.plyr.key.mul(KeysCalc.fixPointFactor())/FixPointFactor).eth().mul(2);
                // 如果超過最大分紅，則只取補足的值
                if(local.plyr.alreadyShareFromKey.add(genPlus) > local.maxShareFromKey){
                    local.shareToPlyr = local.maxShareFromKey - local.plyr.alreadyShareFromKey;
                    // 記錄取得分紅
                    local.plyr.alreadyShareFromKey = local.maxShareFromKey;
                } else {
                    // 記錄取得分紅
                    local.plyr.alreadyShareFromKey = local.plyr.alreadyShareFromKey.add(local.shareToPlyr);
                }
                // 有剩餘的話分到公司
                if(local.shareToPlyr < genPlus){
                    local.shareToCom = genPlus.sub(local.shareToPlyr);
                } else {
                    local.shareToCom = 0;
                }
                // 套用修改
                data.plyrs[local.i] = local.plyr;
                // 套用分紅
                data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(local.shareToCom);
                data.vaults[local.plyr.genVaultId] = data.vaults[local.plyr.genVaultId].add(local.shareToPlyr);
            }
        }
        // 記錄所有的錢
        data.vaults[data.totalVaultId] = data.vaults[data.totalVaultId].add(value);
        
        if(data.state == GameState.Idle){
            // 遊戲剛啟動
            data.startTime = now;
            data.endTime = data.startTime + TimeAtStart;
            data.state = GameState.Playing;
        }
        else if(data.state == GameState.Playing){
            // 買超過1個鑽石才會增加時間
            if(local.keyAmount / FixPointFactor >= 1){
                // 增加時間
                data.endTime = data.endTime.add(ExtendTime);
                // 最大限定為24小時
                if(data.endTime > data.startTime + MaxTime){
                    data.endTime = data.startTime + MaxTime;
                }
            }
        }
    }
    
    function buyWithVault(Data storage data, address addr, uint value, bytes32 partnerLink, bytes32 friendLink) internal returns(uint8) {
        uint plyrId = getOrNewPlayer(data, addr);
        Player memory plyr = data.plyrs[plyrId];
        uint totalVault = withdraw(data, addr);
        if(value > totalVault){
            value = totalVault;
        }
        uint remainVault = totalVault - value;
        // 改為固定點小數存入記憶體
        data.vaults[plyr.genVaultId] = remainVault * FixPointFactor;
        buy(data, addr, value, partnerLink, friendLink);
    }
    
    function withdraw(Data storage data, address addr) internal returns (uint){
        uint id = getPlayerId(data, addr);
        if(id == 0){
            return 0;
        }
        Player memory plyr = data.plyrs[id];
        uint total = data.vaults[plyr.winVaultId]
            .add(data.vaults[plyr.genVaultId])
            .add(data.vaults[plyr.friVaultId]);
        data.vaults[plyr.winVaultId] = 0;
        data.vaults[plyr.genVaultId] = 0;
        data.vaults[plyr.friVaultId] = 0;
        return total / FixPointFactor;
    }
 
    struct endRoundLocal{
        uint i;
        uint pot;
        uint win;
        uint winLastOne;
        uint lastPlyrId;
        YAMDAlg.Player lastPlyr;
        uint plyrId;
        YAMDAlg.Player plyr;
        uint winLastOnePerKey;
        uint lastOneTotalKeys;
        uint key;
        uint pub;
        uint par;
        PartnerMgr.Partner partner;
        uint currKey;
        uint occupyKey;
    }
    
    function endRound(Data storage data) internal {
        endRoundLocal memory local;
        // 彩池(固定點小點)
        local.pot = data.vaults[data.potVaultId];
        // 最後1%的鑽石數(固定點小點)
        local.lastOneTotalKeys = getTotalKeyAmount(data).mul(LastWinP)/100;
        // 最後1位玩家的分紅
        local.win = local.pot.mul(PotWinRate) / 100;
        // 最後1%鑽石的分紅
        local.winLastOne = local.pot.mul(PotLastOneRate) / 100;
        // 合夥人的分紅
        local.par = local.pot.mul(PotParRate) / 100;
        // 公益分紅
        local.pub = local.pot.mul(PotPubRate) / 100;
        // 套用分紅
        // 最後1位玩家
        if(data.lastPlyrId != 0){
            local.lastPlyrId = data.lastPlyrId;
            local.lastPlyr = data.plyrs[local.lastPlyrId];
            data.vaults[local.lastPlyr.winVaultId] = data.vaults[local.lastPlyr.winVaultId].add(local.win);
        }
        /*if(data.history.length > 0){
            local.lastPlyrId = data.history[data.history.length-1].plyrId;
            local.lastPlyr = data.plyrs[local.lastPlyrId];
            data.vaults[local.lastPlyr.winVaultId] = data.vaults[local.lastPlyr.winVaultId].add(local.win);
        }*/
        // 最後1%鑽石
        if(data.history.length > 0 && local.lastOneTotalKeys > 0){
            // 最後1%鑽石的分紅中的每一個鑽石分紅
            // 固定點小數相除要乘回來
            local.winLastOnePerKey = (local.winLastOne / local.lastOneTotalKeys)*FixPointFactor; 
            
            local.currKey = 0;
            for(local.i=data.history.length-1;; --local.i){
                // 最後1位不分紅
                if(local.i != data.history.length-1){
                    // 佔最後1%鑽石的比例
                    local.currKey += data.history[local.i].key;
                    local.occupyKey = data.history[local.i].key;
                    if(local.currKey > local.lastOneTotalKeys){
                        local.occupyKey = local.lastOneTotalKeys.sub(local.currKey.sub(data.history[local.i].key));
                    }
                    
                    local.plyrId = data.history[local.i].plyrId;
                    local.plyr = data.plyrs[local.plyrId];
                    data.vaults[local.plyr.winVaultId] = data.vaults[local.plyr.winVaultId].add(local.occupyKey.mul(local.winLastOnePerKey)/FixPointFactor);
                }
                if(local.i == 0){
                    break;
                }
            }
        }
        // 合夥人
        local.partner = data.partnerMgr.getPartner(local.lastPlyr.addr, local.lastPlyr.usedPartnerLink);
        if(local.partner.proj == PartnerMgr.Project.Unknow){
            // 如果不是合夥人的下線
            // 公益
            data.vaults[data.pubVaultId] = data.vaults[data.pubVaultId].add(local.par);
        }else{
            // 如果是合夥人的下線（用合夥人提供的連結玩遊戲）
            // 分紅給合夥人
            local.plyrId = getPlayerId(data, local.partner.addr);
            if(local.plyrId != 0){
                local.plyr = data.plyrs[local.plyrId];
                data.vaults[local.plyr.parVaultId] = data.vaults[local.plyr.parVaultId].add(local.par);
            }
        }
        // 公益
        data.vaults[data.pubVaultId] = data.vaults[data.pubVaultId].add(local.pub);
        // 彩池歸零
        data.vaults[data.potVaultId] = 0;
        
        // 準備下個回合
        for(local.i=1; local.i<data.plyrs.length; ++local.i){
            data.plyrs[local.i].key = 0;
            data.plyrs[local.i].alreadyShareFromKey = 0;
        }
        data.lastPlyrId = 0;
        data.history.length = 0;
        data.rnd++;
        data.state = GameState.Idle;
    }
    
    struct RoundInfo {
        uint rnd;
        uint startTime;
        uint endTime;
        uint remainTime;
        uint comVault;
        uint potVault;
        uint pubVault;
        GameState state;
        uint lastPlyrId;
        uint keyAmount;
    }
    
    function getRoundInfo(Data storage data) internal view returns (RoundInfo){
        uint remainTime;
        if(data.endTime >= now){
            remainTime = data.endTime.sub(now);
        }
        uint lastPlyrId = data.lastPlyrId;
        /*uint lastPlyrId = 0;
        // 避免error
        if(data.history.length > 0){
            lastPlyrId = data.history[data.history.length-1].plyrId;
        }*/
        uint keyAmount = getTotalKeyAmount(data);
        return RoundInfo(
            data.rnd,
            data.startTime,
            data.endTime,
            remainTime,
            data.vaults[data.comVaultId],
            data.vaults[data.potVaultId],
            data.vaults[data.pubVaultId],
            data.state,
            lastPlyrId,
            keyAmount
        );
    }
    
    struct PlayerInfo {
        uint key;
        uint winVault;
        uint genVault;
        uint friVault;
        uint parVault;
        bytes32 friendLink;
    }
    
    function getPlayerInfo(Data storage data, address addr) internal view returns (PlayerInfo){
        uint id = getPlayerId(data, addr);
        YAMDAlg.Player memory plyr = data.plyrs[id];
        return PlayerInfo(
            plyr.key,
            data.vaults[plyr.winVaultId],
            data.vaults[plyr.genVaultId],
            data.vaults[plyr.friVaultId],
            data.vaults[plyr.parVaultId],
            plyr.friendLink
        );
    }
}

// File: contracts/YAMDMain.sol

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
    // 
    // 主業務
    //
    YAMDAlg.Data data;
    
    function registerPartner(uint level) public payable{
        address user = msg.sender;
        uint value = msg.value;
        PartnerMgr.Project proj = PartnerMgr.Project.One;
        if(level == 2){
            proj = PartnerMgr.Project.Two;
        }
        if(data.partnerMgr.register(user, value, proj)){
            // TODO 錢要流向公司
        } else {
            // 還給玩家
            uint plyrId = data.getPlayerId(user);
            data.vaults[data.plyrs[plyrId].genVaultId] = data.vaults[data.plyrs[plyrId].genVaultId].add(value);
        }
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
