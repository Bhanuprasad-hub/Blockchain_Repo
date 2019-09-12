pragma solidity 0.4.23;


contract Migrations {
    address public owner; 
    uint public lastmigration;

    function  Migrations() public {
        owner = msg.sender;
    }

    modifier restricted() {
        if (msg.sender == owner)
            _;
    }

    function setCompleted(uint completed) public restricted {
        lastmigration = completed;
    }

    function upgrade(address newAddress) public restricted {
        Migrations upgraded = Migrations(newAddress);
        upgraded.setCompleted(lastmigration);
    }
}
