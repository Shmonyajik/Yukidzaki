using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.RLP;
using Yukidzaki.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System.Numerics;
using Nethereum.ABI;
using Yukidzaki_Domain.ViewModels;
using NuGet.Protocol;
using Nethereum.ABI.Model;

namespace Yukidzaki.Controllers
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
        [HttpPost]
        public async Task<JsonResult> Mint([FromBody]MintData mintData)
        {
            try
            {
                string ethereumNetworkUrl = "https://sepolia.infura.io/v3/96eb1666f8a142ed9c21d5e2fa776874";
                Web3 _web3 = new Web3(ethereumNetworkUrl);
                var contractAddress = "0xF416fAa0185070AE542c795f41EC4580Dd35C584"; // Адрес вашего контракта
                var userAddress = mintData.user_address; // Адрес пользователя
                var quantity = mintData.quantity; // Количество монет для монетного двора
                string contractABI;
                using (var reader = new System.IO.StreamReader("wwwroot/ContractABI.json"))
                {
                    contractABI = reader.ReadToEnd();
                }
                var contract = _web3.Eth.GetContract(contractABI, contractAddress);
                var function = contract.GetFunction("safeMint");

                
                var gasLimit = new HexBigInteger(2000000);
                var nonce = await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(userAddress);
                var encodedInput = function.GetData(quantity);
                var transactionInput = new TransactionInput
                {
                    From = userAddress,
                    To = contractAddress,
                    GasPrice = new HexBigInteger(await GetGasPriceAsync(ethereumNetworkUrl)),
                    Gas = gasLimit,
                    Nonce = new HexBigInteger(nonce),
                    Value = new HexBigInteger(0),
                    Data = encodedInput
                };

                return new JsonResult(Ok(new { TransactionInput = transactionInput }));

            }
            catch (Exception ex)
            {
                return new JsonResult(BadRequest(new { Message = ex.Message }));
            }
        }

        public class MintData
        {
            public string user_address { get; set; }

            public int quantity { get; set; }
            
        }
        //public async Task<bool> Mint(string senderAddress = "0xEefeED9305B87a571CBA2974D9643c7BA1106547")
        //{
        //    string ethereumNetworkUrl = "https://mainnet.infura.io/v3/96eb1666f8a142ed9c21d5e2fa776874";
        //    Web3 web3 = new Web3(ethereumNetworkUrl);
        //    string contractAddress = "0xd075875c6C3718c40e0F0eBbBeA46a6e09ec14D1";  // Replace with your smart contract address
        //    string contractABI;
        //    using (var reader = new System.IO.StreamReader("wwwroot/ContractABI.json"))
        //    {
        //        contractABI = reader.ReadToEnd();
        //    }
        //    // Replace with your smart contract ABI
        //    var contract = web3.Eth.GetContract(contractABI, contractAddress);
        //    var mintFunction = contract.GetFunction("safeMint");
        //    // Create the transaction input
        //    var transactionInput = new TransactionInput
        //    {
        //        To = contractAddress,
        //        From = senderAddress, // Your wallet address that will send the transaction
        //        Value = new HexBigInteger(0),
        //        Gas = new HexBigInteger("400000"), // Set the appropriate gas limit
        //        GasPrice = new HexBigInteger(await GetGasPriceAsync(ethereumNetworkUrl)), // Set the appropriate gas price
        //    };

        //    // Send the transaction and wait for the receipt
        //    var transactionReceipt = await mintFunction.SendTransactionAndWaitForReceiptAsync(transactionInput, new CancellationToken());

        //    return transactionReceipt.Status.Value == 1;

        //}//recipientAddress, new HexBigInteger(0), new HexBigInteger(500000), recipientAddress, amountToMint

        private async Task<BigInteger> GetGasPriceAsync(string rpcUrl)
        {
            var web3 = new Web3(rpcUrl);
            var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();
            BigInteger gweiValue = gasPrice.Value / 1_000_000_000;
            return gweiValue;

        }

        public JsonResult GetContractData()
        {
            var smartcontract = new Smartcontract { Address = "0x7631CbEF26474677abcF6B063f53B3741907177C" };  // Replace with your smart contract address
            using (var reader = new System.IO.StreamReader("wwwroot/ContractABI.json"))
            {
                smartcontract.ABI = reader.ReadToEnd();
            }
            return new JsonResult(smartcontract.ToJson());

        }
        [HttpGet]
        public ActionResult ConnectWallet()
        {
            return PartialView("_ModalWallets");
        }





    }
}
//TODO: 1)отслеживание смены сети/аккаунта
//2)Какие могут быть проблемы с безопасностью
//3)Будет ли это работать с другими узлами(в том числе и с нашим узлом infura)
//4)Подключатся ли другие кошельки( не MetaMask)
//5)Добавить сессию и все последующие мероприятия после аутентификации

//inpage.js:1 MetaMask no longer injects web3. For details, see: https://docs.metamask.io/guide/provider-migration.html#replacing-window-web3
