using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Babadzaki.Controllers
{
    public class MintController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       //public async Task<JsonResult> Mint(string senderAddress = "0xEefeED9305B87a571CBA2974D9643c7BA1106547")
       // {
       //     string ethereumNetworkUrl = "https://sepolia.infura.io/v3/96eb1666f8a142ed9c21d5e2fa776874";
       //     Web3 web3 = new Web3(ethereumNetworkUrl);
       //     string contractAddress = "0x336aE9e2AAeeecd1C9468ED53DB2471c239233bd";  // Replace with your smart contract address
       //     string contractABI;
       //     using (var reader = new System.IO.StreamReader("~/ContractABI.json"))
       //     {
       //         contractABI = reader.ReadToEnd();
       //     }
       //     // Replace with your smart contract ABI
       //     var contract = web3.Eth.GetContract(contractABI, contractAddress);

       //     BigInteger amountToMint = new BigInteger(1);  // Replace with the amount of tokens to mint
       //     BigInteger gasPriceInGwei = new BigInteger(10);
       //     var mintFunction = contract.GetFunction("mint");
       //     var transactionReceipt = await mintFunction.SendTransactionAndWaitForReceiptAsync(senderAddress, new CancellationToken(), new HexBigInteger(0), new HexBigInteger(500000), contractAddress, amountToMint, null, gasPriceInGwei);
       //     //var transactionReceipt = await mintFunction.SendTransactionAndWaitForReceiptAsync(
       //     //    senderAddress, new HexBigInteger(0), new HexBigInteger(500000), new CancellationToken(),
       //     //    null,
       //     //    amountToMint);

       //     if (transactionReceipt.Status.Value == 1)
       //     {
       //         // Transaction succeeded
       //         return new JsonResult(transactionReceipt.TransactionHash);
       //         // Handle the successful minting process

       //     }
       //     else
       //     {
       //         // Transaction failed
       //         return new JsonResult(transactionReceipt.TransactionHash);

       //         // Handle the error condition
       //     }

       // }//recipientAddress, new HexBigInteger(0), new HexBigInteger(500000), recipientAddress, amountToMint
        public async Task<bool> MintToken(string senderAddress = "0xEefeED9305B87a571CBA2974D9643c7BA1106547")
        {
            string ethereumNetworkUrl = "https://polygon-rpc.com";
            Web3 web3 = new Web3(ethereumNetworkUrl);
            string contractAddress = "0x7631CbEF26474677abcF6B063f53B3741907177C";  // Replace with your smart contract address
            string contractABI;
            using (var reader = new System.IO.StreamReader("~/ContractABI.json"))
            {
                contractABI = reader.ReadToEnd();
            }
            // Replace with your smart contract ABI
            var contract = web3.Eth.GetContract(contractABI, contractAddress);
            var mintFunction = contract.GetFunction("publicMint"); // Add a "mint" function to the smart contract.

            var transactionReceipt = await mintFunction.SendTransactionAndWaitForReceiptAsync(
                from: senderAddress, // Replace with the account address that will mint the tokens.
                gas: new HexBigInteger(400000),
                value: new HexBigInteger(0),
                functionInput: new object[] { contractAddress, 1 }
            );

            return transactionReceipt.Status.Value == 1;
        }


    }
}
