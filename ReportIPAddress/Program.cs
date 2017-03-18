using log4net.Config;
using System;
using System.IO;
using Topshelf;

namespace ReportIPAddress
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\log4config.config"));

            HostFactory.Run(x =>
            {
                x.Service<IPAddressService>();

                x.RunAsLocalSystem();
                x.SetDisplayName("IPAddress Reporter");
                x.SetDescription("自动汇报IP地址");
            });
        }
    }
}
