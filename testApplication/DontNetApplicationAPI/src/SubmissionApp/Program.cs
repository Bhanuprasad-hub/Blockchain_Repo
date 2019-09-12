using System;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using SubmissionApp.Models;
using System.IO;

namespace console
{
    public class Program
    {
        static IServiceProvider serviceProvider;
        static AppSettings _appSettings;
        static string DirectoryName;
        static void Main(string[] args)
        {
            try
            {
                bool isAppsettingEmpty = false;
                SetServiceProvider();

                if (string.IsNullOrEmpty(_appSettings.BlockchainNetworkURL))
                {
                    isAppsettingEmpty = true;
                    Console.WriteLine("Blockchain Network URL!");
                }
                if (string.IsNullOrEmpty(_appSettings.ContractAddress))
                {
                    isAppsettingEmpty = true;
                    Console.WriteLine("Contract Address!");
                }
                if (string.IsNullOrEmpty(_appSettings.ABIContract))
                {
                    isAppsettingEmpty = true;
                    Console.WriteLine("ABI Contract!");
                }

                if (!isAppsettingEmpty)
                {
                    //The URL endpoint for the blockchain network.
                    string url = "http://192.168.5.192:7545";

                    //The contract address.
                    string address = "0x09457112536fbad39a3c65b59108988e45d1fb04";

                    //string ABI = @"[{'constant':true,'inputs':[],'name':'candidate1','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'candidate','type':'uint256'}],'name':'castVote','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'candidate2','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'voted','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'}]";

                    //The ABI for the contract.
                    string ABI = @"[{'constant':true,'inputs':[],'name':'candidate1','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'candidate','type':'uint256'}],'name':'castSubmission','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'candidate2','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'submissioned','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'}]";

                    //Creates the connecto to the network and gets an instance of the contract.
                    Web3 web3 = new Web3(url);
                    Contract voteContract = web3.Eth.GetContract(ABI, address);

                    //Reads the vote count for Candidate 1 and Candidate 2
                    Task<BigInteger> candidate1Function = voteContract.GetFunction("candidate1").CallAsync<BigInteger>();
                    candidate1Function.Wait();
                    int candidate1 = (int)candidate1Function.Result;
                    Task<BigInteger> candidate2Function = voteContract.GetFunction("candidate2").CallAsync<BigInteger>();
                    candidate2Function.Wait();
                    int candidate2 = (int)candidate2Function.Result;
                    Console.WriteLine("Candidate 1 submissions: {0}", candidate1);
                    Console.WriteLine("Candidate 2 submissions: {0}", candidate2);

                    //Prompts for the account address.
                    Console.Write("Enter the address of your account: ");
                    string accountAddress = Console.ReadLine();

                    //Prompts for the users vote.
                    int vote = 0;
                    Console.Write("Press 1 to submission for candidate 1, Press 2 to submission for candidate 2: ");
                    Int32.TryParse(Convert.ToChar(Console.Read()).ToString(), out vote);
                    Console.WriteLine("You pressed {0}", vote);

                    //Executes the vote on the contract.
                    try
                    {
                        HexBigInteger gas = new HexBigInteger(new BigInteger(400000));
                        HexBigInteger value = new HexBigInteger(new BigInteger(0));
                        Task<string> castVoteFunction = voteContract.GetFunction("castSubmission").SendTransactionAsync(accountAddress, gas, value, vote);
                        castVoteFunction.Wait();
                        Console.WriteLine("Submission completed!");
                        Console.Read();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: {0}", e.Message);
                    }
                }
                else
                    Console.Read();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static void SetServiceProvider()
        {
            try
            {
                #region Hosting Environment
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var builder = new ConfigurationBuilder()
                                   .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                                  .AddJsonFile($"appsettings.json", optional: true)
                                  .AddEnvironmentVariables();
                //.AddEnvironmentVariables();

                IConfigurationRoot Configuration = builder.Build();

                IConfigurationSection AppsettingSection = Configuration.GetSection("AppSettings");
                #endregion

                #region service Provider

                //setup our DI
                serviceProvider = new ServiceCollection()
                    //.AddLogging()                   
                    //.AddSerilog()
                    .AddSingleton<IConfiguration>(Configuration)
                    .AddSingleton<AppSettings>()
                    .BuildServiceProvider();


                //configure console logging
                //serviceProvider
                //    .GetService<ILoggerFactory>()
                //    .AddDebug()
                //    .AddFile(DirectoryName + "/Logs/GADataSynchronizer-{Date}.txt");
                //.AddConsole(LogLevel.Debug);

                _appSettings = new AppSettings();
                AppsettingSection.Bind(_appSettings);

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}