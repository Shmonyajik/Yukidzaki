using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;

using Yukidzaki.ViewModel;
using Microsoft.Extensions.Caching.Memory;
using Yukidzaki_Domain;
using Yukidzaki_Domain.Responses;

namespace Yukidzaki.Controllers
{
    [Consumes("application/json")]
    public class MetaMaskAuthController : Controller
    {

        private IMemoryCache _cache;

        public MetaMaskAuthController(IMemoryCache cache)
        {
            _cache = cache;
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
        //[ValidateAntiForgeryToken]
        public JsonResult VerifySignature([FromBody] VerifySignatureRequest request)
        {
            try
            {
                var response = new BaseResponse<bool>();
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
                        response.Data = true;
                        response.StatusCode = Yukidzaki_Domain.Enums.StatusCode.OK;
                        // Signature is valid
                        // Proceed with further actions
                        _cache.Set("VerifyedUserAddress", walletAddress, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                        return new JsonResult(Ok(response));
                    }
                    else
                    {
                        response.Data = false;
                        response.StatusCode = Yukidzaki_Domain.Enums.StatusCode.AuthenticationFailure;
                        response.Description = "Invalid Signature";
                        return new JsonResult(BadRequest(response));
                    }

                }

                else
                {
                    response.Data = false;
                    response.StatusCode = Yukidzaki_Domain.Enums.StatusCode.ModelStateIsInvalid;
                    response.Description = "Model state is invalid";
                    return new JsonResult(BadRequest(response));
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = Yukidzaki_Domain.Enums.StatusCode.InternalServerError,
                    Data = false
                    
                });
            }
           

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Mint([FromBody] MintData mintData)
        {
            var response = new BaseResponse<bool>();
            if(ModelState.IsValid)
            {
                _cache.TryGetValue("VerifyedUserAddress", out string? _userAddress);
                if (_userAddress == mintData.userAddress && mintData.userAddress != null)
                {
                    response.Data = true;
                    response.StatusCode = Yukidzaki_Domain.Enums.StatusCode.OK;
                    return new JsonResult(Ok(response));
                }
                else
                {
                    response.Data = false;
                    response.StatusCode = Yukidzaki_Domain.Enums.StatusCode.AuthenticationFailure;
                    response.Description = "Invalid Signature";
                    return new JsonResult(Ok(response));
                }
            }
            else
            {
                response.StatusCode = Yukidzaki_Domain.Enums.StatusCode.ModelStateIsInvalid;
                response.Description = "Model state is not valid";
                response.Data = false;
              }
        }

        public class MintData
        {
            public string userAddress { get; set; }
            
        }
        #region old
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<JsonResult> Mint([FromBody] string userAddress)
        //{
        //   if(ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _cache.TryGetValue("VerifyedUserAddress", out string? _userAddress);
        //            if( _userAddress == userAddress)
        //            {
        //                return new JsonResult(Ok());
        //            }
        //            return new JsonResult(BadRequest($"Invalid address! {userAddress}"));
        //        }
        //        catch (Exception)
        //        {
        //            return new JsonResult(BadRequest($"Ssession is out of date!"));
        //        }

        //    }
        //    else
        //    {
        //        return new JsonResult(BadRequest(""));
        //    }
        //}

        //public class MintData
        //{
        //    public string user_address { get; set; }

        //    public int quantity { get; set; }

        //}


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

        //private async Task<BigInteger> GetGasPriceAsync(string rpcUrl)
        //{
        //    var web3 = new Web3(rpcUrl);
        //    var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();
        //    BigInteger gweiValue = gasPrice.Value / 1_000_000_000;
        //    return gweiValue;

        //}

        //public JsonResult GetContractData()
        //{
        //    var smartcontract = new Smartcontract { Address = "0x7631CbEF26474677abcF6B063f53B3741907177C" };  // Replace with your smart contract address
        //    using (var reader = new System.IO.StreamReader("wwwroot/ContractABI.json"))
        //    {
        //        smartcontract.ABI = reader.ReadToEnd();
        //    }
        //    return new JsonResult(smartcontract.ToJson());

        //}
        #endregion
        [HttpGet]
        public ActionResult ConnectWallet()
        {
            return PartialView("_ModalWallets");
        }

        [HttpPost]
        public ActionResult GetButton([FromBody] UserAddressModel userAddressModel)
        {
            if(userAddressModel.userAddress != null)
            {
                ViewBag.userAddress = userAddressModel.userAddress;
                return PartialView("_MintBtn");
            }
            else
            {
                return PartialView("_ConnectWalletBtn");
            }
        }

        public class UserAddressModel
        {
            public string? userAddress { get; set; }
        }
        [HttpGet]
        public ActionResult GetMintModal()
        {
            return PartialView("_ModalMint");
        }





    }
}
//TODO: 1)отслеживание смены сети/аккаунта
//2)Какие могут быть проблемы с безопасностью
//3)Будет ли это работать с другими узлами(в том числе и с нашим узлом infura)
//4)Подключатся ли другие кошельки( не MetaMask)
//5)Добавить сессию и все последующие мероприятия после аутентификации

//inpage.js:1 MetaMask no longer injects web3. For details, see: https://docs.metamask.io/guide/provider-migration.html#replacing-window-web3
