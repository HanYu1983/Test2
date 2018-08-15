pragma solidity ^0.4.24;

import "./lib/SafeMath.sol";
import "./PartnerMgr.sol";

library YAMDAlg {
    using SafeMath for *;
    using PartnerMgr for PartnerMgr.Data;
    
    uint8 constant ComRate = 4;
    uint8 constant Par1Rate = 2;
    uint8 constant Par2Rate = 4;
    uint8 constant InvRate = 10;
    uint8 constant GenRate = 62;
    uint8 constant PotRate = 20;
    
    uint8 constant PotWinRate = 50;
    uint8 constant PotLastOneRate = 30;
    uint8 constant PotBelParRate = 10;
    uint8 constant PotPubRate = 10;
    
    uint constant KeyEthAtStart = 100 szabo;
    uint constant FixPointFactor = 1000 wei;
    uint constant ExtendTime = 60 seconds;
    uint constant TimeAtStart = 1 minutes;
    uint constant MaxTime = 24 hours;
    
    struct Player{
        uint key;           // 固定點小數
        uint winVaultId;    // 固定點小數
        uint genVaultId;    // 固定點小數
        uint inviteVaultId; // 固定點小數
        uint parVaultId;    // 固定點小數
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
        Player[] plyrs;
        
        GameState state;
        History[] history;
        
        uint comVaultId;
        uint potVaultId;
        uint pubVaultId;
        uint totalVaultId;
        uint[] vaults;  // 固定點小數
        // only for query
        PartnerMgr.Data partnerMgr;
    }
    
    enum ErrorMsg{
        error
    }
    
    enum GameState{
        Idle, Playing
    }
    
    function init(Data storage data) internal {
        require(ComRate + Par2Rate + InvRate + GenRate + PotRate == 100, "");
        require(PotWinRate + PotLastOneRate + PotBelParRate + PotPubRate == 100, "");
        
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
        player.inviteVaultId = genVaultId(data);
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
        
        data.plyrs.push(plyr);
        return id;
    }
  
    function getTotalKeyAmount(Data storage data) internal view returns (uint){
        uint i;
        uint total;
        for(i=1; i<data.plyrs.length; ++i){
            total += data.plyrs[i].key;
        }
        return total;
    }
    
    struct buyLocal{
        uint plyrId;
        Player plyr;
        uint com;
        uint pot;
        uint gen;
        uint par;
        uint res;
        uint totalKey;
        uint genPerKey;
        uint i;
        uint keyAmount;
        PartnerMgr.Partner partner;
    }
    
    function getKeyPrice() internal pure returns (uint){
        return KeyEthAtStart;
    }
    
    function buy(Data storage data, address addr, uint value, uint partnerLink) internal returns(uint8) {
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
        local.keyAmount = value.mul(FixPointFactor) / getKeyPrice();
        // 取得玩家
        local.plyrId = getOrNewPlayer(data, addr);
        local.plyr = data.plyrs[local.plyrId];
        // 增加鑽石
        local.plyr.key = local.plyr.key.add(local.keyAmount);
        // 套用新資料!
        // 很多事情要在這行之後處理!
        data.plyrs[local.plyrId] = local.plyr;
        
        // 處理歷史訂單，這裡用來做最後1%鑽石的分紅
        data.history.push(History(local.plyrId, local.keyAmount));
        // TODO 只保留最後1%的歷史訂單
        
        // 處理合夥人
        local.partner = data.partnerMgr.getPartner(partnerLink);
        if(local.partner.proj == PartnerMgr.Project.Unknow){
            // 如果不是合夥人的下線（用合夥人提供的連結玩遊戲）
            // 分紅給公司
            local.com = value.mul(ComRate).mul(FixPointFactor)/100;
        }else{
            // 如果是合夥人的下線
            // 依專案分紅給公司和合夥人
            local.com = value.mul(PartnerMgr.comRate(local.partner.proj)).mul(FixPointFactor)/100;
            local.par = value.mul(PartnerMgr.partRate(local.partner.proj)).mul(FixPointFactor)/100;
        }
        // 彩池
        local.pot = value.mul(PotRate).mul(FixPointFactor)/100;
        // 鑽石回饋
        local.gen = value.mul(GenRate).mul(FixPointFactor)/100;
        
        // 套用分紅
        // 公司
        data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(local.com);
        // 彩池
        data.vaults[data.potVaultId] = data.vaults[data.potVaultId].add(local.pot);
        // 合夥人
        local.plyrId = data.plyrIdByAddr[local.partner.addr];
        if(local.plyrId != 0){
            // 若合夥人存在就分給合夥人
            local.plyr = data.plyrs[local.plyrId];
            data.vaults[local.plyr.parVaultId] = data.vaults[local.plyr.parVaultId].add(local.par);
        } else {
            // 若合夥人不存在就分給公司。合理的情況這裡應該不會運行
            data.vaults[data.comVaultId] = data.vaults[data.comVaultId].add(local.par);
        }
        // 記錄所有的錢
        data.vaults[data.totalVaultId] = data.vaults[data.totalVaultId].add(value);
        // 鑽石回饋
        // 不保含這次買的key
        local.totalKey = getTotalKeyAmount(data).sub(local.keyAmount);
        local.genPerKey = (local.gen / local.totalKey).mul(FixPointFactor);
        
        uint genPlus;
        for(local.i=1; local.i<data.plyrs.length; ++local.i){
            if(local.i == local.plyrId){
                // 如果是自己，減掉今次買的。這樣計算才正確
                local.plyr = data.plyrs[local.i];
                genPlus = (local.genPerKey * local.plyr.key.sub(local.keyAmount)) / FixPointFactor;
                data.vaults[local.plyr.genVaultId] = data.vaults[local.plyr.genVaultId].add(genPlus);
            }else{
                local.plyr = data.plyrs[local.i];
                genPlus = (local.genPerKey * local.plyr.key) / FixPointFactor;
                data.vaults[local.plyr.genVaultId] = data.vaults[local.plyr.genVaultId].add(genPlus);
            }
        }
        
        if(data.state == GameState.Idle){
            // 遊戲剛啟動
            data.startTime = now;
            data.endTime = data.startTime + TimeAtStart;
            data.state = GameState.Playing;
        }
        else if(data.state == GameState.Playing){
            // 增加時間
            data.endTime = data.endTime.add(ExtendTime);
            if(data.endTime > data.startTime + MaxTime){
                data.endTime = data.startTime + MaxTime;
            }
        }
    }
    
    function withdraw(Data storage data, address addr) internal returns (uint){
        uint id = getPlayerId(data, addr);
        Player memory plyr = data.plyrs[id];
        uint total = data.vaults[plyr.winVaultId]
            .add(data.vaults[plyr.genVaultId])
            .add(data.vaults[plyr.inviteVaultId]);
        data.vaults[plyr.winVaultId] = 0;
        data.vaults[plyr.genVaultId] = 0;
        data.vaults[plyr.inviteVaultId] = 0;
        return total / FixPointFactor;
    }
 
    struct endRoundLocal{
        uint i;
        uint pot;
        uint win;
        uint winLastOne;
        uint leadPlyrId;
        YAMDAlg.Player player;
        uint winLastOnePerKey;
        uint lastOneTotalKeys;
        uint key;
        uint pub;
    }
    
    function endRound(Data storage data) internal {
        endRoundLocal memory local;
        // 彩池
        local.pot = data.vaults[data.potVaultId];
        // 最後1%的鑽石數(固定點小點)
        local.lastOneTotalKeys = getTotalKeyAmount(data)/100;
        // 最後1位玩家的分紅
        local.win = local.pot.mul(PotWinRate) / 100;
        // 最後1%鑽石的分紅
        local.winLastOne = local.pot.mul(PotLastOneRate) / 100;
        // 公益分紅
        local.pub = local.pot.mul(PotPubRate) / 100;
        
        // 最後1%鑽石的分紅中的每一個鑽石分紅
        local.winLastOnePerKey = local.winLastOne / local.lastOneTotalKeys; 
        
        // 套用分紅
        // 最後1位玩家
        local.leadPlyrId = data.history[data.history.length-1].plyrId;
        local.player = data.plyrs[local.leadPlyrId];
        data.vaults[local.player.winVaultId] = data.vaults[local.player.winVaultId].add(local.win);
        // 最後1%鑽石
        for(local.i=0; local.i<data.history.length; ++local.i){
            // TODO 佔最後1%鑽石的比例
            local.leadPlyrId = data.history[local.i].plyrId;
            local.key = data.history[local.i].key;
            local.player = data.plyrs[local.leadPlyrId];
            data.vaults[local.player.winVaultId] = data.vaults[local.player.winVaultId].add(local.key.mul(local.winLastOnePerKey));
        }
        // 公益
        data.vaults[data.pubVaultId] = data.vaults[data.pubVaultId].add(local.pub);
        // 彩池歸零
        data.vaults[data.potVaultId] = 0;
        
        // 準備下個回合
        for(local.i=1; local.i<data.plyrs.length; ++local.i){
            data.plyrs[local.i].key = 0;
        }
        data.history.length = 0;
        data.rnd++;
        data.state = GameState.Idle;
    }
    
    struct RoundInfo {
        uint rnd;
        uint startTime;
        uint endTime;
        uint remainTime;
        uint potVault;
    }
    
    function getRoundInfo(Data storage data) internal view returns (RoundInfo){
        uint remainTime;
        if(data.endTime >= now){
            remainTime = data.endTime.sub(now);
        }
        return RoundInfo(
            data.rnd,
            data.startTime,
            data.endTime,
            remainTime,
            data.vaults[data.potVaultId]
        );
    }
    
    struct PlayerInfo {
        uint key;
        uint winVault;
        uint genVault;
        uint inviteVault;
    }
    
    function getPlayerInfo(Data storage data, address addr) internal view returns (PlayerInfo){
        uint id = getPlayerId(data, addr);
        YAMDAlg.Player memory plyr = data.plyrs[id];
        return PlayerInfo(
            plyr.key,
            data.vaults[plyr.winVaultId],
            data.vaults[plyr.genVaultId],
            data.vaults[plyr.inviteVaultId]
        );
    }
}