using Bdev.Net.Dns;
using Bdev.Net.Dns.Records;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
namespace Babadzaki_Utility
{
    
    public class PingTest
    {
        private readonly ILogger<PingTest> _logger;
       
        public PingTest(ILogger<PingTest> logger)
        {
            _logger = logger;
        }
        //public void GetPing()
        //{

        //    PingStatus = false;
        //    try
        //    {
        //        Ping ping = new Ping();
        //        PingReply Status = ping.Send(hostname, 5);
        //        PingStatus = (Status.Status == IPStatus.Success);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public bool GetMX(string domainName)
        {

            MXRecord[] records = Resolver.MXLookup(domainName);
            if (records.Count() > 0)
            { 
                _logger.LogInformation("MX records exist");
                return true;
            }

            _logger.LogWarning("MX records not exist");
             return false;
        }


    }
}
