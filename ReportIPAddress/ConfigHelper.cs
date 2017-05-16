using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportIPAddress
{
    public class ConfigHelper
    {
        public class EmailHelper
        {
            public static string From
            {
                get
                {
                    var fromEmail = ReadConfigAppsetting("FromEmail");
                    if (string.IsNullOrEmpty(fromEmail)) throw new Exception("FromEmail尚未配置");
                    return fromEmail.Trim();
                }
            }

            public static string FromName
            {
                get
                {
                    var fromName = ReadConfigAppsetting("FromName");
                    if (string.IsNullOrEmpty(FromName)) throw new Exception("FromName尚未配置");
                    return fromName.Trim();
                }
            }

            public static string Server
            {
                get
                {
                    var server = ReadConfigAppsetting("Server");
                    if (string.IsNullOrEmpty(server)) throw new Exception("Server尚未配置");
                    return server.Trim();
                }
            }

            public static string UserName
            {
                get
                {
                    var userName = ReadConfigAppsetting("UserName");
                    if (string.IsNullOrEmpty(userName)) throw new Exception("UserName尚未配置");
                    return userName.Trim();
                }
            }

            public static string Password
            {
                get
                {
                    var password = ReadConfigAppsetting("Password");
                    if (string.IsNullOrEmpty(password)) throw new Exception("Password尚未配置");
                    return password.Trim();
                }
            }

            public static List<string> ToEmailList
            {
                get
                {
                    var toEmailConfig = ReadConfigAppsetting("To");
                    if (string.IsNullOrEmpty(toEmailConfig)) throw new Exception("邮件发送者To尚未配置");
                    return toEmailConfig.Split(',').ToList();
                }
            }
        }

        public class SystemHelper
        {
            public static string ServiceName
            {
                get
                {
                    var serviceName = ReadConfigAppsetting("ServiceName");
                    return string.IsNullOrEmpty(serviceName) ? "IPAddress Reporter" : serviceName.Trim();
                }
            }

            public static string ServiceDescription
            {
                get
                {
                    var serviceDescription = ReadConfigAppsetting("ServiceDescription");
                    return string.IsNullOrEmpty(serviceDescription) ? "自动汇报IP地址" : serviceDescription.Trim();
                }
            }
        }

        /// <summary>
        /// 从AppSetting中读取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string ReadConfigAppsetting(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            var config = System.Configuration.ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(config) ? string.Empty : config.Trim();
        }
    }
}
