pragma solidity ^0.4.24;

import "./lib/SafeMath.sol";

contract YAMDDistribute {
    using SafeMath for *;
    
    address public owner;
    modifier onlyOwner(){
        require(msg.sender == owner, "must owner");
        _;
    }
    
    constructor() public {
        owner = msg.sender;
        initTargets();
    }
    
    function kill() onlyOwner() public {
        selfdestruct(msg.sender);
    }
    //
    //
    //
    struct ShareTarget {
        address addr;
        uint percent;
    }
    
    ShareTarget[] targets;
    
    function initTargets() private {
        targets.push(ShareTarget(0x0, 5));
    }
    
    function distribute() onlyOwner() public payable{
        uint value = msg.value;
        uint remain = value;
        uint i;
        uint eth;
        for(i=0; i<targets.length; ++i){
            eth = value.mul(targets[i].percent) / 100;
            if(eth > remain){
                eth = remain;
            }
            remain -= eth;
            if(eth > 0){
                targets[i].addr.transfer(eth);
            }
            if(remain == 0){
                break;
            }
        }
        if(remain > 0){
            msg.sender.transfer(remain);
        }
    }
}