pragma solidity ^0.4.24;

import "./lib/SafeMath.sol";

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
        Partner[] partners; // 索引為0的位置不用
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
    
    function getPartner(Data storage data, address addr) internal view returns (Partner){
        uint id = data.partnerIdByAddr[addr];
        Partner memory partner =  data.partners[id];
        return partner;
    }
    
    function open(Data storage data) internal {
        data.open = true;
    }
    /*
    // 不使用
    function getLink(Data storage data, address addr) private view returns (bool, bytes32){
        uint id = data.partnerIdByAddr[addr];
        bool isValid = id != 0;
        return (isValid, data.partners[id].link);
    }
    
    // 不使用
    function getPartnerByLink(Data storage data, bytes32 link) private view returns (Partner){
        uint id = data.partnerIdByLink[link];
        Partner memory partner =  data.partners[id];
        return partner;
    }*/
}