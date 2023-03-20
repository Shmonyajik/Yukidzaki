using Babadzaki.Models;

namespace Babadzaki.Utility
{
    
    public interface IMailService
    {
        public void SendMessage(string to,string from = "vjxfkrf2000@gmail.com", string subject = "", string bodyText = "");

        public void SendMessage(IEnumerable<Email> emailList, string from = "vjxfkrf2000@gmail.com", string subject = "", string bodyText = "");
    }
}
