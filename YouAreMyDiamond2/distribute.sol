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

// File: contracts/IForwarder.sol

interface IForwarder{
    function deposit() external payable;
}

// File: contracts/YAMDDistribute.sol

contract YAMDDistribute is IForwarder{
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
    
    function deposit() external payable{
        distribute();
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
    
    function distribute() public payable{
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
