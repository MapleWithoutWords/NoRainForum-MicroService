using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoRainForum.Computer.FrontWeb.Models;
using NoRainForumCommon;
using NoRainSDK.Models;
using NoRainSDK.src;

namespace NoRainForum.Computer.FrontWeb.Controllers
{
    public class RegisterController : Controller
    {
        public UserService UserSvc { get; set; }
        public SendEmailService SendEmailSvc { get; set; }
        public RegisterController(UserService UserSvc, SendEmailService SendEmailSvc)
        {
            this.UserSvc = UserSvc;
            this.SendEmailSvc = SendEmailSvc;
        }
        [HttpGet]
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(ConstList.USERID)))
            {
                return Redirect("/User/Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmailCode(EmailValidCodeModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(ConstList.USERID)))
            {
                return Json(new AjaxResult { Status = "redirect", Data = "/User/Home" });
            }
            if (TempData[ConstList.VALIDCODE] == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码过期" });
            }
            string code = (string)TempData[ConstList.VALIDCODE];
            TempData[ConstList.VALIDCODE] = null;
            if (!code.Equals(model.ValidCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            SendEmailModel send = new SendEmailModel();
            send.RecipientEmail = model.RecipientEmail;
            send.SendType = SendType.Register;
            string emailCode = await SendEmailSvc.SendRegisterEmailAsync(send);
            if (string.IsNullOrEmpty(emailCode))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = SendEmailSvc.ErrorMsg });
            }
            TempData[ConstList.EMAILVALIDCODE] = emailCode;
            TempData[ConstList.REGISTERORFOUNDPASSEMAIL] = model.RecipientEmail;
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpPost]
        public async Task<IActionResult> Index(RegisterModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(ConstList.USERID)))
            {
                return Json(new AjaxResult { Status = "redirect", Data = "/User/Home" });
            }
            if (TempData[ConstList.EMAILVALIDCODE] == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码过期" });
            }
            if (TempData[ConstList.REGISTERORFOUNDPASSEMAIL] == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "表单过期" });
            }
            string email= (string)TempData[ConstList.REGISTERORFOUNDPASSEMAIL];
            string emailCode = (string)TempData[ConstList.EMAILVALIDCODE];
            TempData[ConstList.EMAILVALIDCODE] = null;
            TempData[ConstList.REGISTERORFOUNDPASSEMAIL] = null;
            if (!emailCode.Equals(model.EmailCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            if (!email.Equals(model.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "邮箱不一致" });
            }
            AddUserModel addUser = new AddUserModel();
            addUser.Email = model.Email;
            addUser.Gender = model.Gender;
            addUser.NickName = model.NickName;
            addUser.Password = model.Password;
            if (await UserSvc.AddNewAsync(addUser) < 1)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = UserSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}