using System;
using System.Linq;
using System.Net;
using Topshelf;

namespace ReportIPAddress
{
    public class IPAddressService : ServiceControl, IReporter
    {
        #region ServiceControl
        public bool Start(HostControl hostControl)
        {
            var taskId = Guid.NewGuid().ToString();
            try
            {
                Report(taskId, GetIPAddress());
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("logerror").Error(taskId, ex);
            }

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }
        #endregion

        #region IReporter
        public void Report(string taskId, string message)
        {
            new EmailHelper(taskId).SendMail(message);
        }
        #endregion

        public string GetIPAddress()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            return string.Join(";", ipHost.AddressList.Select(item => item.ToString()));
        }
    }
}
