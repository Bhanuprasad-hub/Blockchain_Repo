using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using System.Numerics;
using WebApplication2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    public class AssessmentController : Controller
    {
        public IConfiguration config;
        string addressURL;
        string address;
        string ABIContract = "";
        public AssessmentController(IConfiguration _config)
        {
            config = _config;
            addressURL = config["AppSettings:BlockchainNetworkURL"];
            address = config["AppSettings:ContractAddress"];
            ABIContract = @config["AppSettings:ABIContract"];
        }

        [HttpPost]
        [Route("PostSubmission")]
        [EnableCors("AllowSpecificOrigin")]
        public IActionResult PostSubmission([FromBody]CastResult submittedResult)
        {

            ////The URL endpoint for the blockchain network.
            //string url = config["AppSettings:BlockchainNetworkURL"];

            ////The contract address.
            //string address = config["AppSettings:ContractAddress"];

            ////string ABI = @"[{'constant':true,'inputs':[],'name':'candidate1','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'candidate','type':'uint256'}],'name':'castVote','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'candidate2','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'voted','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'}]";

            ////The ABI for the contract.
            //string ABI = config["AppSettings:ABIContract"];//@"[{'constant':true,'inputs':[],'name':'candidate1','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'candidate','type':'uint256'}],'name':'castSubmission','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'candidate2','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'submissioned','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'}]";

            //Creates the connecto to the network and gets an instance of the contract.
            Web3 web3 = new Web3(addressURL);
            Contract submissionContract = web3.Eth.GetContract(ABIContract, address);


            //Executes the vote on the contract.
            try
            {
                HexBigInteger gas = new HexBigInteger(new BigInteger(400000));
                HexBigInteger value = new HexBigInteger(new BigInteger(0));
                Task<string> castVoteFunction = submissionContract.GetFunction("postSubmission")
                                                                    .SendTransactionAsync(submittedResult.YourAddress, gas, value, submittedResult.SubmittedResult);
                castVoteFunction.Wait();
                return Ok(true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("GetSubmission/{yourAddress}")]
        [EnableCors("AllowSpecificOrigin")]
        public IActionResult GetSubmission(string yourAddress)
        {
            ////The URL endpoint for the blockchain network.
            //string url = config["AppSettings:BlockchainNetworkURL"];

            ////The contract address.
            //string address = config["AppSettings:ContractAddress"];

            ////The ABI for the contract.
            //string ABI = config["AppSettings:ABIContract"];//@"[{'constant':true,'inputs':[],'name':'candidate1','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'candidate','type':'uint256'}],'name':'castSubmission','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'candidate2','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'submissioned','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'}]";

            //Creates the connecto to the network and gets an instance of the contract.
            Web3 web3 = new Web3(addressURL);
            Contract submissionContract = web3.Eth.GetContract(ABIContract, address);
            try
            {
                HexBigInteger gas = new HexBigInteger(new BigInteger(400000));
                HexBigInteger value = new HexBigInteger(new BigInteger(0));
                Task<string> castgetSubmissionFunction = submissionContract.GetFunction("getSubmissions")
                                                                    .CallAsync<string>(yourAddress, gas, value, "");
                castgetSubmissionFunction.Wait();

                //Reads the vote count for Candidate 1 and Candidate 2
                //Task<string> submissionFunction = submissionContract.GetFunction("submission").CallAsync<string>();
                //submissionFunction.Wait();
                string res = castgetSubmissionFunction.Result;
                if (string.IsNullOrEmpty(res))
                    throw new NullReferenceException("No data");

                //SubmitResult submitResult = JsonConvert.DeserializeObject<SubmitResult>(res);
                return Ok(res);
            }
            catch (Exception e)
            {
                throw e;
                //Console.WriteLine("Error: {0}", e.Message);
            }
        }

        [HttpGet]
        [Route("GetSubmissionVal")]
        public string Get() {
            return "string";
        }
    }
}
