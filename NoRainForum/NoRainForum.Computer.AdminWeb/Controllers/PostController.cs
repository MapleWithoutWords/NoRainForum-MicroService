using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoRainForum.Computer.AdminWeb.filters;
using NoRainForum.Computer.AdminWeb.Models;
using NoRainForumCommon;
using NoRainSDK.Models;
using NoRainSDK.src;

namespace NoRainForum.Computer.AdminWeb.Controllers
{
    public class PostController : Controller
    {
        public PostService PostSvc { get; set; }
        public UserService UserSvc { get; set; }
        public PostStatusService PsSVC { get; set; }
        public PostController(PostService PostSvc, UserService UserSvc, PostStatusService PsSVC)
        {
            this.PostSvc = PostSvc;
            this.UserSvc = UserSvc;
            this.PsSVC = PsSVC;
        }
        [HttpGet]
        [CheckPermission("Post.List")]
        public async Task<IActionResult> List(int pageIndex = 1, int pageDataCount = 10)
        {
            var pers = await PostSvc.GetAdminWebPageDataAsync(pageIndex, pageDataCount);
            if (pers == null)
            {
                return Content(PostSvc.ErrorMsg);
            }
            List<long> userIds = pers.Datas.Select(e => e.UserId).ToList();
            var users = await UserSvc.GetByIdsAsync(userIds);
            AdminWebListPostModel model = new AdminWebListPostModel();
            model.Posts = pers.Datas;
            model.TotalCount = pers.TotalCount;
            model.Users = users;

            NoRainPage page = new NoRainPage();
            page.DataCount = pers.TotalCount;
            page.PageIndex = pageIndex;
            page.Url = "/Post/List?pageIndex=@parms";
            model.Page= page.GetPaging();
            return View(model);
        }
        [HttpPost]
        [CheckPermission("Post.Delete")]
        public async Task<IActionResult> Delete(long id)
        {

            if (!await PostSvc.DeleteAsync(id))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PostSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        [CheckPermission("Post.Add")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [CheckPermission("Post.Add")]
        public async Task<IActionResult> Add(AddPostModel model)
        {
            if (await PostSvc.AddNewAsync(model) < 1)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PostSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        [CheckPermission("Post.Edit")]
        public async Task<IActionResult> Edit(long id)
        {
            var post = await PostSvc.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var user = await UserSvc.GetByIdAsync(post.UserId);
            if (user==null)
            {
                return NotFound();
            }
            var status =await PsSVC.GetAllAsync();
            LookPostModel model = new LookPostModel();
            model.Post = post;
            model.User = user;
            model.PostStatuses = status;
            return View(model);
        }
        [HttpPost]
        [CheckPermission("Post.Edit")]
        public async Task<IActionResult> Edit(UpdatePostModel model)
        {
            if (!await PostSvc.UpdateAsync(model))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PostSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}