using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Babadzaki.Data;
using Nethereum.Web3;
using Nethereum.RLP;

namespace Babadzaki.Controllers
{
    [Consumes("application/json")]
    public class MetaMaskAuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FilterManagementController> _logger;
        private readonly Web3 _web3;
        public MetaMaskAuthController(ApplicationDbContext context, ILogger<FilterManagementController> logger, Web3 web3)
        {
            _context = context;
            _logger = logger;
            _web3 = web3;

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
            var randomBytes = new byte[32];
            var random = new Random();
            random.NextBytes(randomBytes);

            return randomBytes.ToHex();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult VerifySignature([FromBody] VerifySignatureRequest request)
        {
            // Get the user's wallet address and one-time code from the request
            string walletAddress = request.walletAddress;
            string oneTimeCode = request.oneTimeCode;
            string signature = request.signature;

            // Verify the signature
            var signer = new EthereumMessageSigner();

            var signerAddress = signer.EncodeUTF8AndEcRecover(oneTimeCode, signature);
            var isSignatureValid = signerAddress.Equals(signature, StringComparison.OrdinalIgnoreCase);
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

        public class VerifySignatureRequest
        {
            public string walletAddress { get; set; }
            public string oneTimeCode { get; set; }
            public string signature { get; set; }
        }
    }
}
