using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
namespace All.Class
{
    public class Email
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">发件人邮箱,如abc@163.com</param>
        /// <param name="password">发件人密码</param>
        /// <param name="recive">收伯人</param>
        /// <param name="value">发送消息</param>
        /// <param name="title">邮件标题</param>
        public static bool Send(string email,string password, string[] recive,string title,string value)
        {
            if (recive == null || recive.Length == 0)
            {
                return false;
            }
            try
            {
                MailMessage message = new MailMessage();
                //设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
                MailAddress fromAddr = new MailAddress(email);
                message.From = fromAddr;
                //设置收件人,可添加多个,添加方法与下面的一样
                for (int i = 0; i < recive.Length; i++)
                {
                    message.To.Add(recive[i]);
                }
                //设置抄送人
                //message.CC.Add("izhaofu@163.com");
                //设置邮件标题
                message.Subject = title;
                //设置邮件内容
                message.Body = value;
                //设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的 邮箱管理后台查看,下面是QQ的
                SmtpClient client = new SmtpClient();
                if (email.ToUpper().Contains("@QQ.COM"))
                {
                    client.Host = "smtp.qq.com";
                    client.Port = 25;
                }
                if (email.ToUpper().Contains("@163.COM"))
                {
                    client.Host = "smtp.163.com";
                    client.Port = 25;
                }
                if (email.ToUpper().Contains("@126.COM"))
                {
                    client.Host = "smtp.126.com";
                    client.Port = 25;
                }
                //设置发送人的邮箱账号和密码
                client.Credentials = new NetworkCredential(email.Substring(0, email.IndexOf('@')), password);
                //启用ssl,也就是安全发送
                client.EnableSsl = true;
                //发送邮件
                client.Send(message);
            }
            catch 
            {
                All.Class.Error.Add("邮件发送失败", string.Format("Email:{0},Password:{1},Recive:{2},Title:{3},Value:{4}", email, password, recive[0], title, value));
                return false; 
            }
            return true;
        }
    }
}
