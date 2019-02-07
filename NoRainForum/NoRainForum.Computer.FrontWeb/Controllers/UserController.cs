using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoRainForum.Computer.FrontWeb.Filters;
using NoRainForum.Computer.FrontWeb.Models;
using NoRainForumCommon;
using NoRainSDK.Models;
using NoRainSDK.src;

namespace NoRainForum.Computer.FrontWeb.Controllers
{
    public class UserController : Controller
    {
        public UserService UserSvc { get; set; }
        public PostCommentService CommentSvc { get; set; }
        public SendEmailService SendEmailSvc { get; set; }
        public PostService PostSvc { get; set; }
        public UserController(UserService UserSvc, PostCommentService CommentSvc, PostService PostSvc, SendEmailService SendEmailSvc)
        {
            this.UserSvc = UserSvc;
            this.CommentSvc = CommentSvc;
            this.PostSvc = PostSvc;
            this.SendEmailSvc = SendEmailSvc;

        }
        /// <summary>
        /// 用户主页
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Home(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(ConstList.USERID)))
                {
                    return Redirect("/login/index");
                }
                long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
                var idUser = await UserSvc.GetByIdAsync(userId);
                return View(idUser);
            }
            var user = await UserSvc.GetByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Home(long userId)
        {
            var user = await UserSvc.GetByIdAsync(userId);
            if (user == null)
            {
                return Json(new AjaxResult
                {
                    Data = "/Error/Error404" +
                    "",
                    Status = "redirect"
                });
            }
            var posts = await PostSvc.GetQuestionPostByUserIdAsync(user.Id);
            var comments = await CommentSvc.GetByCommentUserIdAsync(userId);
            if (posts == null || comments == null)
            {
                return Json(new AjaxResult
                {
                    Data = "/Error/Error500" +
                    "",
                    Status = "redirect"
                });
            }
            ListUserHomeModel model = new ListUserHomeModel();
            var postIds = posts.Select(e => e.Id).ToList();
            foreach (var item in postIds)
            {
                long look = await RedisHelper.IncrByAsync("post_" + item, 0);
                model.LookCount.Add(look);
            }

            model.Comments = comments;
            model.Posts = posts;
            return Json(new AjaxResult
            {
                Data = model,
                Status = "ok"
            });
        }


        [HttpGet]
        [CheckLogin]
        public async Task<IActionResult> Set()
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            var user = await UserSvc.GetByIdAsync(userId);
            return View(user);
        }

        [HttpPost]
        [CheckLogin]
        public async Task<IActionResult> Set(UpdateUserModel model)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            var user = await UserSvc.GetByIdAsync(userId);
            model.Id = userId;
            if (model.Email != user.Email)
            {
                model.IsActive = false;
            }
            if (!await UserSvc.UpdateAsync(model))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = UserSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        [CheckLogin]
        public async Task<IActionResult> Index()
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            var user = await UserSvc.GetByIdAsync(userId);
            return View(user);
        }
        /// <summary>
        /// 获取用户收藏的帖子
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckLogin]
        public async Task<IActionResult> LoadCollectionPost(int pageIndex, int pageDataCount)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            var user = await UserSvc.GetByIdAsync(userId);
            var posts = await PostSvc.GetCollectionPostByUserIdAsync(userId, pageIndex, pageDataCount);
            if (posts == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PostSvc.ErrorMsg });
            }
            List<long> looks = new List<long>();
            var postIds = posts.Datas.Select(e => e.Id).ToList();
            return Json(new AjaxResult { Status = "ok", Data = posts });
        }
        /// <summary>
        /// 获取用户发的帖子
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckLogin]
        public async Task<IActionResult> LoadUserSendPost(int pageIndex, int pageDataCount)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            var user = await UserSvc.GetByIdAsync(userId);
            var posts = await PostSvc.GetByUserIdAsync(userId, pageIndex, pageDataCount);
            if (posts == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PostSvc.ErrorMsg });
            }
            List<long> looks = new List<long>();
            var postIds = posts.Datas.Select(e => e.Id).ToList();
            foreach (var item in postIds)
            {
                long look = await RedisHelper.IncrByAsync("post_" + item, 0);
                looks.Add(look);
            }
            return Json(new AjaxResult { Status = "ok", Data = new { Posts = posts, LookCounts = looks } });
        }

        [CheckLogin]
        [HttpGet]
        public async Task<IActionResult> Active()
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            var user = await UserSvc.GetByIdAsync(userId);
            return View(user);
        }

        [CheckLogin]
        [HttpPost]
        public async Task<IActionResult> Active(string emailCode, string email)
        {
            if (TempData[ConstList.EMAILVALIDCODE] == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码过期" });
            }
            if (TempData[ConstList.REGISTERORFOUNDPASSEMAIL] == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "邮箱错误" });
            }
            string code = (string)TempData[ConstList.EMAILVALIDCODE];
            string sessionEmail = (string)TempData[ConstList.REGISTERORFOUNDPASSEMAIL];
            TempData[ConstList.REGISTERORFOUNDPASSEMAIL] = null;
            TempData[ConstList.EMAILVALIDCODE] = null;
            if (!code.Equals(emailCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            if (!sessionEmail.Equals(email, StringComparison.InvariantCultureIgnoreCase))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "邮箱不一致" });
            }
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            if (!await UserSvc.ActiveEmailAsync(userId))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = UserSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpPost]
        [CheckLogin]
        public async Task<IActionResult> SendActiveEmailCode(EmailValidCodeModel model)
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
            send.SendType = SendType.ActiveEmail;
            string emailCode = await SendEmailSvc.SendRegisterEmailAsync(send);
            if (string.IsNullOrEmpty(emailCode))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = SendEmailSvc.ErrorMsg });
            }
            TempData[ConstList.EMAILVALIDCODE] = emailCode;
            TempData[ConstList.REGISTERORFOUNDPASSEMAIL] = model.RecipientEmail;
            return Json(new AjaxResult { Status = "ok" });
        }

        [CheckLogin]
        [HttpGet]
        public async Task<IActionResult> Message()
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            var user = await UserSvc.GetByIdAsync(userId);
            return View(user);
        }
        [CheckLogin]
        [HttpPost]
        public async Task<IActionResult> GetMsg()
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            var msgs = await RedisHelper.SMembersAsync<MessageModel>($"msg_{userId}");
            return Json(new AjaxResult {Status="ok",Data= msgs });
        }
    }
}