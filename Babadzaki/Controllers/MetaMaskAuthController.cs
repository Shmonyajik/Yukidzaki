using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Babadzaki.Data;
using Nethereum.Web3;
using Nethereum.RLP;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Babadzaki.Controllers
{
    [Consumes("application/json")]
    public class MetaMaskAuthController : Controller
    {
        
        

        public MetaMaskAuthController()
        {
            
            


        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GenerateOneTimeCode()
        {
            var oneTimeCode = GenerateRandomString();

            return new JsonResult(Ok(oneTimeCode));
        }

        private string GenerateRandomString()
        {


            var randomBytes = EthECKey.GenerateKey().GetPubKeyNoPrefix();
            var randomHex = randomBytes.ToHex();

            return randomHex;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult VerifySignature([FromBody] VerifySignatureRequest request)
        {
            if (ModelState.IsValid)
            {
                // Get the user's wallet address and one-time code from the request
                string walletAddress = request.walletAddress;
                string oneTimeCode = request.oneTimeCode;
                string signature = request.signature;

                // Verify the signature
                var signer = new EthereumMessageSigner();

                var signerAddress = signer.EncodeUTF8AndEcRecover(oneTimeCode, signature);
                var isSignatureValid = signerAddress.Equals(walletAddress, StringComparison.OrdinalIgnoreCase);
                if (isSignatureValid)
                {
                    // Signature is valid
                    // Proceed with further actions
                    return new JsonResult(Ok(isSignatureValid));
                }
                else
                {
                    return new JsonResult(BadRequest("Invalid signature!@."));
                }

            }

            else
            {
                return new JsonResult(BadRequest("Invalid signature/one time code/wallet address."));
            }

        }
        public async Task<bool> Mint(string senderAddress = "0xEefeED9305B87a571CBA2974D9643c7BA1106547")
        {
            string ethereumNetworkUrl = "https://polygon.llamarpc.com";
            Web3 web3 = new Web3(ethereumNetworkUrl);
            string contractAddress = "0x7631CbEF26474677abcF6B063f53B3741907177C";  // Replace with your smart contract address
            string contractABI;
            using (var reader = new System.IO.StreamReader("wwwroot/ContractABI.json"))
            {
                contractABI = reader.ReadToEnd();
            }
            // Replace with your smart contract ABI
            var contract = web3.Eth.GetContract(contractABI, contractAddress);
            var mintFunction = contract.GetFunction("publicMint");
            // Create the transaction input
            var transactionInput = new TransactionInput
            {
                To = contractAddress,
                From = senderAddress, // Your wallet address that will send the transaction
                Value = new HexBigInteger(0),
                Gas = new HexBigInteger("400000"), // Set the appropriate gas limit
                GasPrice = new HexBigInteger("1000"), // Set the appropriate gas price
            };

            // Send the transaction and wait for the receipt
            var transactionReceipt = await mintFunction.SendTransactionAndWaitForReceiptAsync(transactionInput, new CancellationToken());

            return transactionReceipt.Status.Value == 1;

        }//recipientAddress, new HexBigInteger(0), new HexBigInteger(500000), recipientAddress, amountToMint

    }
}
//TODO: 1)отслеживание смены сети/аккаунта
//2)Какие могут быть проблемы с безопасностью
//3)Будет ли это работать с другими узлами(в том числе и с нашим узлом infura)
//4)Подключатся ли другие кошельки( не MetaMask)
//5)Добавить сессию и все последующие мероприятия после аутентификации
