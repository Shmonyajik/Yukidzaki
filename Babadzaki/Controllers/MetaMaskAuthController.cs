//using Microsoft.AspNetCore.Mvc;
//using System.Security.Cryptography;

//using Nethereum.Signer;
//using Nethereum.Hex.HexConvertors.Extensions;
//using Nethereum.Util;
//using Babadzaki.Data;
//using Nethereum.Web3;
//using Nethereum.RLP;
//using Babadzaki.ViewModel;
//using Microsoft.AspNetCore.Http.HttpResults;

//namespace Babadzaki.Controllers
//{
//    [Consumes("application/json")]
//    public class MetaMaskAuthController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<FilterManagementController> _logger;
        
//        public MetaMaskAuthController(ApplicationDbContext context, ILogger<FilterManagementController> logger)
//        {
//            _context = context;
//            _logger = logger;
            

//        }
//        public IActionResult Index()
//        {
//            return View();
//        }

//        public JsonResult GenerateOneTimeCode()
//        {
//            var oneTimeCode = GenerateRandomString();

//            return new JsonResult(Ok(oneTimeCode));
//        }

//        private string GenerateRandomString()
//        {
            

//            var randomBytes = EthECKey.GenerateKey().GetPubKeyNoPrefix();
//            var randomHex = randomBytes.ToHex();

//            return randomHex;
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public JsonResult VerifySignature([FromBody] VerifySignatureRequest request)
//        {
//            if (ModelState.IsValid)
//            {
//                // Get the user's wallet address and one-time code from the request
//                string walletAddress = request.walletAddress;
//                string oneTimeCode = request.oneTimeCode;
//                string signature = request.signature;

//                // Verify the signature
//                var signer = new EthereumMessageSigner();

//                var signerAddress = signer.EncodeUTF8AndEcRecover(oneTimeCode, signature);
//                var isSignatureValid = signerAddress.Equals(walletAddress, StringComparison.OrdinalIgnoreCase);
//                if (isSignatureValid)
//                {
//                    // Signature is valid
//                    // Proceed with further actions
//                    return new JsonResult(Ok(isSignatureValid));
//                }
//                else
//                {
//                    return new JsonResult(BadRequest("Invalid signature!@."));
//                }

//            }
            
//            else
//            {
//                return new JsonResult(BadRequest("Invalid signature/one time code/wallet address."));
//            }

//        }
        
//    }
//}
////TODO: 1)отслеживание смены сети/аккаунта
////2)Какие могут быть проблемы с безопасностью
////3)Будет ли это работать с другими узлами(в том числе и с нашим узлом infura)
////4)Подключатся ли другие кошельки( не MetaMask)
////5)Добавить сессию и все последующие мероприятия после аутентификации
