pragma solidity 0.4.23;


contract Assessment {    
    mapping (address => string) public submissionData;

    function postSubmission(string submitResult) public {
        submissionData[msg.sender] = submitResult;
    }

    function getSubmissions(string addres) public view returns (string) {
        return submissionData[msg.sender];
    }

 
}