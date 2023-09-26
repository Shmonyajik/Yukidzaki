using System.ComponentModel.DataAnnotations;

namespace Yukidzaki.ViewModel
{
    public class VerifySignatureRequest
    {
        [RegularExpression("^0x[0-9a-fA-F]{40}$", ErrorMessage = "Invalid wallet address.")]
        public string walletAddress { get; set; }
        [RegularExpression("^[0-9a-fA-F]+$", ErrorMessage = "Invalid one-time code.")]
        public string oneTimeCode { get; set; }
        //[RegularExpression("^[0-9a-fA-F]{200}$", ErrorMessage = "Invalid signature.")]
        public string signature { get; set; }
    }
}
