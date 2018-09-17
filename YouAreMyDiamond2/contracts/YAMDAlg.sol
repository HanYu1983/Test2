pragma solidity ^0.4.24;

import "./lib/SafeMath.sol";
import "./lib/KeysCalc.sol";
import "./PartnerMgr.sol";

library YAMDAlg {
    using SafeMath for *;
    using KeysCalc for *;
    using PartnerMgr for PartnerMgr.Data;
    
    // 公司分紅比例(百分之)
    uint8 constant ComRate = 4;
    // 合夥人分紅比例
    // 單純用來計算合計是否為100
    uint8 constant ParRate = 4;
    // 推薦人分紅比例
    uint8 constant FriRate = 10;
    // 鑽石回饋分紅比例
    uint8 constant GenRate = 62;
    // 彩池比例
    uint8 constant PotRate = 20;
    
    // 彩池中領先玩家分紅比例
    uint8 constant PotWinRate = 50;
    // 彩池中最後百分之1鑽石分紅比例
    uint8 constant PotLastOneRate = 30;
    // 彩池中推薦人分紅比例
    uint8 constant PotParRate = 10;
    // 彩池中公益分紅比例
    uint8 constant PotPubRate = 10;
    
    // 最後幾%鑽石的分紅
    // 用來測試用。值應該是1
    uint8 constant LastWinP = 1;
    // 鑽石最低價
    // 這個時不能修改
    // 公式計算出最低價剛好是這個值
    uint constant KeyEthAtStart = 75 szabo;
    // 每買一個鑽石的延長時間
    uint constant ExtendTime = 60 seconds;
    // 剛買第1顆鑽石的起始倒計時間
    uint constant TimeAtStart = 5 minutes;
    // 最大延長時間
    uint constant MaxTime = 24 hours;
    // 玩家最大鑽石回饋係數(固定點小數)
    uint constant ShareLimitRate = 2 * FixPointFactor;
    // 計算固定點小數的係數
    // 越高越精確
    // 注意，若修改這個值則前台js部分也要一並修改
    uint constant FixPointFactor = 1 ether;
    
    struct Player{
        address addr;
        uint key;                   // 所買鑽石。固定點小數，回合結束必須歸零
        uint eth;
        uint alreadyShareFromKey;   // 已取得的鑽石分紅。用來計算最大分紅，回合結束必須歸零
        //bytes32 usedPartnerLink;    // 所使用的合夥人連結
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
        uint totalExtendTime;
        
        mapping (address=>uint) plyrIdByAddr;
        mapping (bytes32=>uint) plyrIdByFriendLink;
        Player[] plyrs; // 索引為0的位置不用
        
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
        Idle,       // 等待第一顆鑽石被買
        Playing     // 倒計中
    }
    
    function init(Data storage data) internal {
        require(ComRate + ParRate + FriRate + GenRate + PotRate == 100, "");
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
    
    function getOrNewPlayer(Data storage data, address addr) internal returns (uint) {
        uint id = data.plyrIdByAddr[addr];
        if(id != 0){
            return id;
        }
        id = data.plyrs.length;
        data.plyrIdByAddr[addr] = id;

        Player memory plyr;
        plyr.addr = addr;
        plyr.winVaultId = genVaultId(data);
        plyr.genVaultId = genVaultId(data);
        plyr.friVaultId = genVaultId(data);
        plyr.parVaultId = genVaultId(data);
        plyr.friendLink = bytes32(now + (id << 224));
        
        data.plyrs.push(plyr);
        data.plyrIdByFriendLink[plyr.friendLink] = id;
        return id;
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
    
    function buy(Data storage data, address addr, uint value, bytes32 /* partnerLink */, bytes32 friendLink) internal returns(bool) {
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
                return true;
            }
        }
        // 買到的鑽石，有小數點
        local.keyAmount = calcKeyAmount(data.vaults[data.totalVaultId] / FixPointFactor, value);
        // 取得玩家
        local.plyrId = getOrNewPlayer(data, addr);
        local.plyr = data.plyrs[local.plyrId];
        // 只能使用之前綁定的 
        /*if(local.plyr.usedPartnerLink == 0){
            local.plyr.usedPartnerLink = partnerLink;
        }*/
        // 只能使用之前綁定的
        if(local.plyr.usedFriendLink == 0){
            if(data.plyrIdByFriendLink[friendLink] != 0){
                // 先找找是不是合法的推薦人
                // 更新連結
                local.plyr.usedFriendLink = friendLink;
            } else {
                // 若不是推薦人，找合夥人
                local.partner = calcRootPartner(data, friendLink);
                // 若合夥人存在並且合夥人不是自己
                // 更新連結
                if(local.partner.addr != addr && local.partner.proj != PartnerMgr.Project.Unknow){
                    local.plyr.usedFriendLink = friendLink;
                } else {
                    // 其它情況不更新連結
                }
            }
        }
        // 增加鑽石
        local.plyr.key = local.plyr.key.add(local.keyAmount);
        local.plyr.eth = local.plyr.eth.add(value.mul(FixPointFactor));
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
        /*if(local.plyr.usedPartnerLink != 0){
            // 若使用合夥人連結，直接找出合夥人
            local.partner = data.partnerMgr.getPartnerByLink(local.plyr.usedPartnerLink);
        } else {
            // 若沒使用合夥人連結，找出最根部的合夥人
            local.partner = calcRootPartner(data, local.plyr.usedFriendLink);
        }*/
        // 找出最根部的合夥人
        local.partner = calcRootPartner(data, local.plyr.usedFriendLink);
        // 合夥人是自己 || 沒找到專案
        if(local.partner.addr == addr || local.partner.proj == PartnerMgr.Project.Unknow){
            // 不是合夥人的下線，分紅給公司
            local.com = value.mul(ComRate).mul(FixPointFactor)/100;
        }else{
            // 是合夥人的下線
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
                // 注意：合理的情況這裡應該不會運行，若運行到這裡可能代表合夥人有註冊，但沒有呼叫到getOrNewPlayer來建立玩家。所以在註冊成功時請一定要呼叫getOrNewPlayer
                // 該存在的合夥人不存在就分給公司。
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
                // 推薦人如果是自己
                // 分給公司
                data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(local.fri);
            } else {
                // 推薦人分紅不限制
                data.vaults[local.plyr.friVaultId] = data.vaults[local.plyr.friVaultId].add(local.fri);
                /*
                // 分紅限制
                (local.plyr, local.shareToPlyr, local.shareToCom) = calcShareLimit(ShareLimitRate, local.plyr, local.fri);
                // 套用修改
                data.plyrs[local.i] = local.plyr;
                // 套用分紅
                data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(local.shareToCom);
                data.vaults[local.plyr.friVaultId] = data.vaults[local.plyr.friVaultId].add(local.shareToPlyr);
                */
            }
        } else {
            // 若推薦人不存在就分給公司
            data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(local.fri);
        }
        
        // 鑽石回饋
        // ver1.不保含這次買的key
        local.totalKey = getTotalKeyAmount(data).sub(local.keyAmount);
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
                // 分紅限制
                (local.plyr, local.shareToPlyr, local.shareToCom) = calcShareLimit(ShareLimitRate, local.plyr, genPlus);
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
                // 記錄所增加的時間
                data.totalExtendTime = data.totalExtendTime.add(ExtendTime);
            }
        }
        return false;
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
        // 最後1位玩家ver1.
        if(data.lastPlyrId != 0){
            local.lastPlyrId = data.lastPlyrId;
            local.lastPlyr = data.plyrs[local.lastPlyrId];
            data.vaults[local.lastPlyr.winVaultId] = data.vaults[local.lastPlyr.winVaultId].add(local.win);
        }
        // 最後1位玩家ver2.
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
        /*if(local.plyr.usedPartnerLink != 0){
            // 若使用合夥人連結，直接找出合夥人
            local.partner = data.partnerMgr.getPartnerByLink(local.plyr.usedPartnerLink);
        } else {
            // 若沒使用合夥人連結，找出最根部的合夥人
            local.partner = calcRootPartner(data, local.plyr.usedFriendLink);
        }
        */
        if(data.lastPlyrId != 0){
            local.lastPlyrId = data.lastPlyrId;
            local.lastPlyr = data.plyrs[local.lastPlyrId];
            // 找出最根部的合夥人
            local.partner = calcRootPartner(data, local.lastPlyr.usedFriendLink);
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
        }
        // 公益
        data.vaults[data.pubVaultId] = data.vaults[data.pubVaultId].add(local.pub);
        // 彩池歸零
        data.vaults[data.potVaultId] = 0;
        
        // 準備下個回合
        for(local.i=1; local.i<data.plyrs.length; ++local.i){
            data.plyrs[local.i].key = 0;
            data.plyrs[local.i].eth = 0;
            data.plyrs[local.i].alreadyShareFromKey = 0;
        }
        data.lastPlyrId = 0;
        data.history.length = 0;
        data.rnd++;
        data.state = GameState.Idle;
        data.totalExtendTime = 0;
    }
    
    // 提款
    // 這個方法會修改到玩家的錢包，呼叫時一定要把回傳的值(錢)發送給玩家
    function withdraw(Data storage data, address addr) internal returns (uint){
        // 觸發結算endRound
        // 有2個地方觸發結算
        // 1. buy
        // 2. withdraw
        if(data.state == GameState.Playing){
            RoundInfo memory info = getRoundInfo(data);
            if(info.remainTime == 0){
                endRound(data);
            }
        }
        uint id = getPlayerId(data, addr);
        if(id == 0){
            return 0;
        }
        Player memory plyr = data.plyrs[id];
        uint total = data.vaults[plyr.winVaultId]
            .add(data.vaults[plyr.genVaultId])
            .add(data.vaults[plyr.friVaultId]);
        total = total  / FixPointFactor;
        // 保留小數點
        if(total == 0){
            return 0;
        }
        // 取款後小數點不保留，尾數算在合約擁有者上
        data.vaults[plyr.winVaultId] = 0;
        data.vaults[plyr.genVaultId] = 0;
        data.vaults[plyr.friVaultId] = 0;
        return total;
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
        uint totalExtendTime;   // 總延長時間
    }
    
    function getRoundInfo(Data storage data) internal view returns (RoundInfo){
        uint remainTime;
        if(data.endTime >= now){
            remainTime = data.endTime.sub(now);
        }
        // ver1.
        uint lastPlyrId = data.lastPlyrId;
        // ver2.
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
            keyAmount,
            data.totalExtendTime
        );
    }
    
    struct PlayerInfo {
        uint key;
        uint winVault;
        uint genVault;
        uint friVault;
        uint parVault;
        bytes32 friendLink;
        uint alreadyShareFromKey;   // 已分發獎金
        uint eth;                   // 總投資額
        uint id;
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
            plyr.friendLink,
            plyr.alreadyShareFromKey,
            plyr.eth,
            id
        );
    }
    
    function depositToCom(Data storage data, uint value) internal {
        data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(value.mul(FixPointFactor));
    }
    
    function withdrawCom(Data storage data) internal returns (uint){
        uint total = data.vaults[data.comVaultId] / FixPointFactor;
        // 保留小數點
        if(total == 0){
            return 0;
        }
        // 保留小數點
        data.vaults[data.comVaultId] = data.vaults[data.comVaultId].sub(total.mul(FixPointFactor));
        return total;
    }
    
    function withdrawPub(Data storage data) internal returns (uint){
        uint total = data.vaults[data.pubVaultId] / FixPointFactor;
        // 保留小數點
        if(total == 0){
            return 0;
        }
        // 保留小數點
        data.vaults[data.pubVaultId] = data.vaults[data.pubVaultId].sub(total.mul(FixPointFactor));
        return total;
    }
    
    //
    // Helper
    // 
    
    function getTotalKeyAmount(Data memory data) private pure returns (uint){
        uint i;
        uint total;
        for(i=1; i<data.plyrs.length; ++i){
            total += data.plyrs[i].key;
        }
        return total;
    }
    
    function calcKeyAmount(uint totalValue, uint value) internal pure returns (uint){
        /*
        uint totalEth = totalValue;
        uint keyAmountWithCalcFixPoint = totalEth.keysRec(value);
        uint format = keyAmountWithCalcFixPoint.mul(FixPointFactor)/KeysCalc.fixPointFactor();
        return format;
        */
        
        uint totalEth = totalValue;
        uint keyAmountWithCalcFixPoint = totalEth.keysRec(value);
        // 固定點小數同樣都是1 ether, 不必轉換
        uint format = keyAmountWithCalcFixPoint;
        return format;
    }
    
    function calcKeyPrice(Data memory data, uint keyAmount) internal pure returns (uint){
        /*
        uint totalKey = getTotalKeyAmount(data).add(keyAmount);
        uint format = (totalKey * KeysCalc.fixPointFactor())/FixPointFactor;
        return format.ethRec(keyAmount.mul(KeysCalc.fixPointFactor())/FixPointFactor);
        */
        uint totalKey = getTotalKeyAmount(data).add(keyAmount);
        // 固定點小數同樣都是1 ether, 不必轉換
        uint format = totalKey;
        return format.ethRec(keyAmount);
    }
    
    function reduceHistory(Data storage data) private {
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
    
    function hasFriendLinkPointToAddr(Data storage data, address addr) internal view returns (bool){
        uint i;
        uint id = getPlayerId(data, addr);
        if(id == 0){
            return false;
        }
        for(i=1; i<data.plyrs.length; ++i){
            Player memory plyr = data.plyrs[i];
            uint linkToPlyrId = data.plyrIdByFriendLink[plyr.usedFriendLink];
            if(id == linkToPlyrId){
                return true;
            }
        }
        return false;
    }
    
    function calcRootPlayerId(Data storage data, bytes32 friendLink) private view returns (uint){
        if(friendLink == 0){
            return 0;
        }
        bool[] memory alreadyUse = new bool[](data.plyrs.length);
        
        uint friendId = data.plyrIdByFriendLink[friendLink];
        Player memory friend = data.plyrs[friendId];
        alreadyUse[friendId] = true;
        
        for(;friend.usedFriendLink != 0;){
            friendId = data.plyrIdByFriendLink[friend.usedFriendLink];
            if(friendId == 0){
                break;
            }
            // 如果有循環參照，就當成沒有找到
            if(alreadyUse[friendId]){
                return 0;
            }
            alreadyUse[friendId] = true;
            friend = data.plyrs[friendId];
        }
        return friendId;
    }
    
    function calcRootPartner(Data storage data, bytes32 friendLink) private view returns (PartnerMgr.Partner){
        uint friendId = calcRootPlayerId(data, friendLink);
        Player memory friend = data.plyrs[friendId];
        return data.partnerMgr.getPartner(friend.addr);
    }
    
    function calcShareLimit(uint rateFP, Player memory plyr, uint genPlus) private pure returns (Player, uint, uint){
        uint shareToPlyr = genPlus;
        uint shareToCom = 0;
        // 玩家最大鑽石分紅為所買鑽石的2倍
        uint maxShareFromKey = plyr.eth.mul(rateFP)/FixPointFactor; // (plyr.key.mul(KeysCalc.fixPointFactor())/FixPointFactor).eth().mul(rateFP);
        // 如果超過最大分紅，則只取補足的值
        if(plyr.alreadyShareFromKey.add(genPlus) > maxShareFromKey){
            shareToPlyr = maxShareFromKey - plyr.alreadyShareFromKey;
            // 記錄取得分紅
            plyr.alreadyShareFromKey = maxShareFromKey;
        } else {
            // 記錄取得分紅
            plyr.alreadyShareFromKey = plyr.alreadyShareFromKey.add(shareToPlyr);
        }
        // 有剩餘的話分到公司
        if(shareToPlyr < genPlus){
            shareToCom = genPlus.sub(shareToPlyr);
        } else {
            shareToCom = 0;
        }
        return (plyr, shareToPlyr, shareToCom);
    }
    
    //
    // public for test
    //
    
    function testCalcRootPlayerId(Data storage data, bytes32 friendLink) internal view returns (uint){
        return calcRootPlayerId(data, friendLink);
    }
}