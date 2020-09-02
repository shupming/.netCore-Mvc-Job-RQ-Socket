using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HospitalReport.Common
{
    public static class SendEmail
    {
        public static int SendtoEmail(string mailContent,
            string mailSubject,
            string[] mailTo, string sfile="")
        {
            // 设置例网易的smtp
            string smtpServer = "smtp.163.com";// "14.18.245.164"; //SMTP服务器
            string mailFrom = "15279176561@163.com"; //登陆用户名
            string userPassword = "MSONGTPZROAHTRIT";//授权密码
           
            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Timeout = 5000;
            smtpClient.Port = 25;
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

            MailMessage mailMessage = new MailMessage(mailFrom, mailTo[0]); // 发送人和收件人
            mailMessage.Subject = mailSubject;//主题
            mailMessage.Body = mailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级
           
            if (mailTo.Length > 1)
            {
                for (var i = 1; i < mailTo.Length;i++) {
                    mailMessage.To.Add(mailTo[i]);
                }
            }
            if (sfile.Length > 0)
            {
                mailMessage.Attachments.Add(new Attachment(sfile));
            }
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                           delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; };
                smtpClient.Send(mailMessage); // 发送邮件
              
                return 1;
            }
            catch (SmtpException ex)
            {
                return 0;
            }
        }
    }
}
