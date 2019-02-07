using Services.APICommon;
using Services.Other.DTO;
using Services.Other.IService;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.Other.API
{
    public class SendSmtp
    {
        private ISettingService SettingSvc { get; set; }
        public SendSmtp(ISettingService SettingSvc)
        {
            this.SettingSvc = SettingSvc;
        }
        public string Code { get; set; }
        /// <summary>
        /// 接收邮箱
        /// </summary>
        public string Recipient { get; set; }
        public SendType SendType { get; set; } = SendType.Register;

        [HystrixCommand("SendSmtpExQQ")]
        public virtual async Task<bool> SendSmtpQQ()
        {
            SettingDTO dto = await SettingSvc.GetByKeyAsync("sendQQEmail");
            SmtpClient client = new SmtpClient("smtp.qq.com");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(dto.Key, dto.Value);//("1028139084@qq.com", "jwrwtwthyxaobcje");
            client.Port = 587;
            MailAddress from = new MailAddress(dto.Key, "NoRain论坛", Encoding.UTF8);//初始化发件人

            MailAddress to = new MailAddress(Recipient, Recipient, Encoding.UTF8);//初始化收件人

            //设置邮件内容
            MailMessage message = new MailMessage();
            message.From = from;
            message.To.Add(to);
            switch (SendType)
            {
                case SendType.Register:
                    message.Body = $"欢迎您注册NoRain论坛，你的验证码是{Code},20分钟内有效，请尽快填写。";
                    break;
                case SendType.FoundPassword:
                    message.Body = $"找回密码，你的验证码是{Code},20分钟内有效，请尽快填写。";
                    break;
                case SendType.ActiveEmail:
                    message.Body = $"激活邮箱，你的验证码是{Code},20分钟内有效，请尽快填写。";
                    break;
                default:
                    message.Body = $"欢迎您注册NoRain论坛，你的验证码是{Code},20分钟内有效，请尽快填写。";
                    break;
            }
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = "NoRain论坛";
            message.SubjectEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;


            //发送邮件
            try
            {
                client.Send(message);
                return true;
            }
            finally
            {
                client.Dispose();
            }
        }
        public virtual async Task<bool> SendSmtpExQQ()
        {

            SettingDTO dto = await SettingSvc.GetByKeyAsync("sendExQQEmail");
            SmtpClient client = new SmtpClient("smtp.exmail.qq.com");

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(dto.Key, dto.Value);//("18479281670@norain.top", "lm@SHUIMODANQING123");
            client.Port = 587;

            MailAddress from = new MailAddress(dto.Value, "NoRain论坛", Encoding.UTF8);//初始化发件人

            MailAddress to = new MailAddress(Recipient, "", Encoding.UTF8);//初始化收件人

            //设置邮件内容
            MailMessage message = new MailMessage(from, to);
            switch (SendType)
            {
                case SendType.Register:
                    message.Body = $"欢迎您注册NoRain论坛，你的验证码是{Code},20分钟内有效，请尽快填写。";
                    break;
                case SendType.FoundPassword:
                    message.Body = $"找回密码，你的验证码是{Code},20分钟内有效，请尽快填写。";
                    break;
                case SendType.ActiveEmail:
                    message.Body = $"激活邮箱，你的验证码是{Code},20分钟内有效，请尽快填写。";
                    break;
                default:
                    message.Body = $"欢迎您注册NoRain论坛，你的验证码是{Code},20分钟内有效，请尽快填写。";
                    break;
            }
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = "NoRain论坛";
            message.SubjectEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;


            //发送邮件
            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                client.Dispose();
            }
        }
    }
    public enum SendType
    {
        Register=2,
        FoundPassword=4,
        ActiveEmail = 8,
    }
}
