using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CaptchaGen.NetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoRainForum.Computer.AdminWeb.common;
using NoRainForum.Computer.AdminWeb.Models;
using NoRainForumCommon;
using NoRainSDK.Models;
using NoRainSDK.src;

namespace NoRainForum.Computer.AdminWeb.Controllers
{
    public class HomeController : Controller
    {
        public AdminUserService AdminSvc { get; set; }
        public HomeController(AdminUserService AdminSvc)
        {
            this.AdminSvc = AdminSvc;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            AdminUserLoginModel login = new AdminUserLoginModel();
            login.Password = model.Password;
            login.PhoneNum = model.PhoneNum;
            if (TempData[ConstList.LOGINSESSION] == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码过期" });
            }
            string code = (string)TempData[ConstList.LOGINSESSION];
            if (!code.Equals(model.ValidCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            var adminUser = await AdminSvc.LoginAsync(login);
            if (adminUser == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = AdminSvc.ErrorMsg });
            }
            HttpContext.Session.SetString(ConstList.ADMINUSERID, adminUser.Id.ToString());
            return Json(new AjaxResult { Status = "redirect", Data = "/Home/Index" });
        }
        public IActionResult GetValidCode()
        {
            string res = "";
            string code = ValidCodeHelper.CreateVerifyCode(out res);
            TempData[ConstList.LOGINSESSION] = res;
            Stream stream = ImageFactory.BuildImage(code, 40, 120, 12, 2, ImageFormatType.Jpeg);
            return File(stream, "image/jpeg");
        }
        [HttpGet]
        public async Task<IActionResult> PersonalInfo()
        {
            string admin = HttpContext.Session.GetString(ConstList.ADMINUSERID);
            if (string.IsNullOrEmpty(admin))
            {
                return Redirect("/Home/Login");
            }
            long id = Convert.ToInt64(admin);
            var model =await AdminSvc.GetByIdAsync(id);
            if (model==null)
            {
                return Content(AdminSvc.ErrorMsg);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> PersonalInfo(UpdateAdminUserModel model)
        {
            if (!await AdminSvc.UpdateAsync(model))
            {
                return Json(new AjaxResult { Status="error", ErrorMsg=AdminSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok"});
        }
    }
}