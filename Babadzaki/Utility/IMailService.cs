using Babadzaki.Models;

namespace Babadzaki.Utility
{
    
    public interface IMailService
    {
        public void SendMessage(string to, string subject, string bodyText);

        public void SendMessage(IEnumerable<Email> emailList, string subject, string bodyText);
    }
}
