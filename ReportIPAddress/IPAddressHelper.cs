using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ReportIPAddress
{
    public class IPAddressHelper
    {
        const string From = "uniontactics@aliyun.com";
        const string FromName = "IPReportService";
        const string Server = "smtp.aliyun.com";
        const string UserName = "uniontactics@aliyun.com";
        const string Password = "sike1987qq";
        const string To = "yidane@163.com";

        static int ErrorTimes = 0;
        public string GetIPAddress()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            return string.Join(";", ipHost.AddressList.Select(item => item.ToString()));
        }

        public void SendMail(string taskId = "")
        {
            var mail = new MailMessage { From = new MailAddress(From, FromName) };
            taskId = string.IsNullOrEmpty(taskId) ? Guid.NewGuid().ToString() : taskId;
            mail.To.Add(To);
            mail.Subject = "重启获取IP";
            mail.BodyEncoding = Encoding.Default;
            mail.Priority = MailPriority.High;
            mail.Body = string.Format("事件Id：{0} {1}{2}", taskId, Environment.NewLine, GetIPAddress());
            mail.IsBodyHtml = true;
            var smtp = new SmtpClient(Server, 25)
            {
                UseDefaultCredentials = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(UserName, Password),
                Timeout = 10000
            };

            smtp.SendCompleted += SendCompletedCallback;
            smtp.SendAsync(mail, taskId);
        }

        private void SendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                log4net.LogManager.GetLogger("logerror").Error(string.Format("Send Mail Faild For {1} Times. [{0}]", e.UserState.ToString(), ErrorTimes), e.Error);

                System.Threading.Thread.Sleep(20000);
                if (ErrorTimes < 5)
                {
                    ErrorTimes++;
                    SendMail(e.UserState.ToString());
                }
                else
                {
                    ErrorTimes = 0;
                }
            }
            else
            {
                log4net.LogManager.GetLogger("loginfo").Info(string.Format("Send Mail Succeed[{0}]", e.UserState.ToString()));
            }

            var smtpClient = (SmtpClient)sender;
            if (smtpClient != null)
                smtpClient.Dispose();
        }
    }
}
