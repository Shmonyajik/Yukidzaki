

using Babadzaki_Domain.Models;
using Babadzaki_Domain.Responses;

namespace Babadzaki_Services
{
    
    public interface IMailService
    {
        //public void SendMessage(string to, string from = "vjxfkrf2000@gmail.com", string subject = "", string bodyText = "");

        //public void SendMessage(IEnumerable<string> emailList, string from = "vjxfkrf2000@gmail.com", string subject = "", string bodyText = "");

        Task<BaseResponse<bool>> SendMessage(Email email, string from , string? subject , string bodyText = "");
        Task<BaseResponse<bool>> SendMessage(IEnumerable<Email> emailList, string from , string? subject , string? bodyText );
    }
}
