using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Services.APICommon
{
    public class APIExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            
            SmtpClient client = new SmtpClient("smtp.qq.com");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("1028139084@qq.com", "123465");
            client.Port = 587;
            MailAddress from = new MailAddress("1028139084@qq.com", "NoRain论坛", Encoding.UTF8);//初始化发件人

            MailAddress to = new MailAddress("1028139084@qq.com", "1028139084@qq.com", Encoding.UTF8);//初始化收件人

            //设置邮件内容
            MailMessage message = new MailMessage();
            message.From = from;
            message.To.Add(to);
            message.Body = context.Exception.Message;
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = "NoRain论坛";
            message.SubjectEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;


            //发送邮件
            try
            {
                client.Send(message);
            }
            finally
            {
                client.Dispose();
                message.Dispose();
            }
            context.Result = new JsonResult(new APIResult<long> { ErrorMsg="服务器出现点小问题，管理员正在加急修复" }) { StatusCode=400};
        }
    }
}
