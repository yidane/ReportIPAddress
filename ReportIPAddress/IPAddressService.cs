using Topshelf;

namespace ReportIPAddress
{
    public class IPAddressService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            new IPAddressHelper().SendMail();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}
