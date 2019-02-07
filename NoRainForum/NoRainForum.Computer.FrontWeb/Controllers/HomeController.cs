using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CaptchaGen.NetCore;
using Microsoft.AspNetCore.Mvc;
using NoRainForum.Computer.FrontWeb.Models;
using NoRainForumCommon;
using NoRainSDK.src;
using NoRainSDK.Models;
using Microsoft.AspNetCore.Http;

namespace NoRainForum.Computer.FrontWeb.Controllers
{
    public class HomeController : Controller
    {
        public PostService PostSvc { get; set; }
        public UserService UserSvc { get; set; }
        public HomeController(PostService PostSvc, UserService UserSvc)
        {
            this.PostSvc = PostSvc;
            this.UserSvc = UserSvc;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取首页 的帖子
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetPosts()
        {
            //获取置顶的帖子
            var stickPosts = await PostSvc.GetStickAsync();
            List<long> stickUserIds = stickPosts.Select(e => e.UserId).ToList();
            var stickUsers = await UserSvc.GetByIdsAsync(stickUserIds);
            //获取综合的帖子
            var colligatePosts = await PostSvc.GetColligatePostAsync();
            List<long> colligateUserIds = colligatePosts.Datas.Select(e => e.UserId).ToList();
            var colligateUsers = await UserSvc.GetByIdsAsync(colligateUserIds);

            var model = new ListPostUserModel()
            {
                StickPosts = stickPosts,
                StickUsers = stickUsers,
                ColligatePosts = colligatePosts.Datas,
                ColligateUsers = colligateUsers
            };
            return Json(new AjaxResult { Status = "ok", Data = model });
        }
        /// <summary>
        /// 获得本周热议的帖子
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetDayPost()
        {
            List<ListPostDTO> dayPost = await PostSvc.GetDayPostAsync();
            if (dayPost == null)
            {
                return Json(new AjaxResult { Status = "error", Data = PostSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok", Data = dayPost });
        }
        [HttpGet]
        public IActionResult GetValidCode()
        {
            string res = "";
            string code = ValidCodeHelper.CreateVerifyCode(out res);
            TempData[ConstList.VALIDCODE] = res;
            Stream stream = ImageFactory.BuildImage(code, 40, 120, 12, 2, ImageFormatType.Jpeg);
            return File(stream, "image/jpeg");
        }
        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetLoginUser()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(ConstList.USERID)))
            {
                return Json(new AjaxResult { Status = "unLogin"  });
            }
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            var user = await UserSvc.GetByIdAsync(userId);
            if (user==null)
            {
                return Json(new AjaxResult { Status = "unLogin" });
            }
            return Json(new AjaxResult { Status = "ok", Data = user });
        }
        [HttpPost]
        public IActionResult Exit()
        {
            HttpContext.Session.Clear();
            return Json(new AjaxResult { Status="ok"});
        }
    }
}
