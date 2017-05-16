using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ReportIPAddress
{
    public class EmailHelper
    {
        private string TaskId;
        private string From;
        private string FromName;
        private string Server;
        private string UserName;
        private string Password;
        private List<string> To;

        public EmailHelper(string taskId)
        {
                TaskId = string.IsNullOrEmpty(taskId) ? Guid.NewGuid().ToString() : taskId;
                From = ConfigHelper.EmailHelper.From;
                FromName = ConfigHelper.EmailHelper.FromName;
                Server = ConfigHelper.EmailHelper.Server;
                UserName = ConfigHelper.EmailHelper.UserName;
                Password = ConfigHelper.EmailHelper.Password;
                To = ConfigHelper.EmailHelper.ToEmailList;
        }

        static int ErrorTimes = 0;

        public bool SendMail(string message)
        {
            var mail = new MailMessage { From = new MailAddress(From, FromName) };
            To.ForEach(item => mail.To.Add(item));
            mail.Subject = "重启获取IP";
            mail.BodyEncoding = Encoding.Default;
            mail.Priority = MailPriority.High;
            mail.Body = string.Format("事件Id：{0} {1}{2}", TaskId, Environment.NewLine, message);
            mail.IsBodyHtml = true;
            var smtp = new SmtpClient(Server, 25)
            {
                UseDefaultCredentials = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(UserName, Password),
                Timeout = 10000
            };

            smtp.SendCompleted += SendCompletedCallback;
            smtp.SendAsync(mail, TaskId);
            return true;
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
