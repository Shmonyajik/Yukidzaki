

namespace Babadzaki_Utility
{
    
    public interface IMailService
    {
        public void SendMessage(string to, string from = "vjxfkrf2000@gmail.com", string subject = "", string bodyText = "");

        public void SendMessage(IEnumerable<string> emailList, string from = "vjxfkrf2000@gmail.com", string subject = "", string bodyText = "");
    }
}
