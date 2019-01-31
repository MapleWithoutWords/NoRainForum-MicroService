using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.APICommon;
using Services.Other.API.Models;
using Services.Other.IService;

namespace Services.Other.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        public SendSmtp SendSmtp { get; set; }
        public IAppInfoService AppInfoSvc { get; set; }
        public SendEmailController(IAppInfoService AppInfoSvc, SendSmtp SendSmtp)
        {
            this.AppInfoSvc = AppInfoSvc;
            this.SendSmtp = SendSmtp;
        }
        [HttpPost("SendRegisterEmail")]
        public async Task<IActionResult> SendRegisterEmail(SendEmailModel model)
        {
            SendSmtp.Code = new Random().Next(10000, 99999).ToString();
            SendSmtp.Recipient = model.RecipientEmail;
            SendSmtp.SendType = model.SendType;
            if (!await SendSmtp.SendSmtpQQ())
            {
                return new JsonResult(new APIResult<long> { ErrorMsg="发送邮件失败，请稍后再试" }) {StatusCode=400 };
            }
            return new JsonResult(new APIResult<string> { Data=SendSmtp.Code });
        }

    }
}
