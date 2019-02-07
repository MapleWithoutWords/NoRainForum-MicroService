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
    public class LoginController : Controller
    {
        public UserService UserSvc { get; set; }
        public SendEmailService SendEmailSvc { get; set; }
        public LoginController(UserService UserSvc, SendEmailService SendEmailSvc)
        {
            this.UserSvc = UserSvc;
            this.SendEmailSvc = SendEmailSvc;
        }
        [HttpGet]
        public IActionResult Index()
        {
            if (TempData[ConstList.USERID] != null)
            {
                return Redirect("/user/home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(FrontUserLoginModel model)
        {
            if (TempData[ConstList.USERID] != null)
            {
                return Json(new AjaxResult { Status = "redirect",Data= "/user/home" });
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
            UserLoginModel login = new UserLoginModel();
            login.Email = model.Email;
            login.Password = model.Password;
            var user = await UserSvc.LoginAsync(login);
            if (user==null)
            {
                return Json(new AjaxResult {Status="error",ErrorMsg=UserSvc.ErrorMsg });
            }
            HttpContext.Session.SetString(ConstList.USERID, user.Id.ToString());
            return Json(new AjaxResult { Status = "redirect",Data="/User/Home" });
        }
        [HttpGet]
        public IActionResult FoundPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FoundPassword(FoundPasswordModel model)
        {
            if (TempData[ConstList.EMAILVALIDCODE] == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码过期" });
            }
            if (TempData[ConstList.REGISTERORFOUNDPASSEMAIL] == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "表单过期" });
            }
            string email = (string)TempData[ConstList.REGISTERORFOUNDPASSEMAIL];
            string emailCode = (string)TempData[ConstList.EMAILVALIDCODE];
            TempData[ConstList.EMAILVALIDCODE] = null;
            if (!emailCode.Equals(model.EmailCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            if (!email.Equals(model.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "邮箱不一致" });
            }
            RePasswordModel pass = new RePasswordModel();
            pass.Email = model.Email;
            pass.NewPassword = model.NewPassword;
            if (!await UserSvc.EditPasswordAsync(pass))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = UserSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }


        [HttpPost]
        public async Task<IActionResult> SendFoundPassEmail(EmailValidCodeModel model)
        {
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
            send.SendType = SendType.FoundPassword;
            string emailCode = await SendEmailSvc.SendRegisterEmailAsync(send);
            if (string.IsNullOrEmpty(emailCode))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = SendEmailSvc.ErrorMsg });
            }
            TempData[ConstList.EMAILVALIDCODE] = emailCode;
            TempData[ConstList.REGISTERORFOUNDPASSEMAIL] = model.RecipientEmail;
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}