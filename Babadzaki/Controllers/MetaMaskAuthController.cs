using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;

namespace Babadzaki.Controllers
{
    public class MetaMaskAuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerateOneTimeCode()
        {
            var oneTimeCode = GenerateRandomString();

            return Ok(oneTimeCode);
        }

        private string GenerateRandomString()
        {
            var randomBytes = new byte[32];
            var random = new Random();
            random.NextBytes(randomBytes);

            return randomBytes.ToHex();
        }
        public IActionResult VerifySignature([FromBody] VerifySignatureRequest request)
        {
            // Get the user's wallet address and one-time code from the request
            string walletAddress = request.WalletAddress;
            string oneTimeCode = request.OneTimeCode;
            string signature = request.Signature;

            // Verify the signature
            var signer = new EthereumMessageSigner();
            var checksumAddress = AddressUtil.Current.ConvertToChecksumAddress(walletAddress);
            var signerAddress = signer.EncodeUTF8AndSign(oneTimeCode, checksumAddress).ToHex();
            var isSignatureValid = signerAddress.Equals(signature, StringComparison.OrdinalIgnoreCase);
            if (isSignatureValid)
            {
                // Signature is valid
                // Proceed with further actions
                return Ok();
            }
            else
            {
                return BadRequest("Invalid signature.");
            }
        }

        public class VerifySignatureRequest
        {
            public string WalletAddress { get; set; }
            public string OneTimeCode { get; set; }
            public string Signature { get; set; }
        }
    }
}
