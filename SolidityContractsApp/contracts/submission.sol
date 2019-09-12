pragma solidity 0.4.23;


contract Submission {
    uint public candidate1;
    uint public candidate2;
    
    mapping (address => bool) public submissioned;

    function castSubmission(uint candidate) public {
        
        if (candidate == 1)
            candidate1++;        
        else
            candidate2++;            
        
        submissioned[msg.sender] = true;
    }
}