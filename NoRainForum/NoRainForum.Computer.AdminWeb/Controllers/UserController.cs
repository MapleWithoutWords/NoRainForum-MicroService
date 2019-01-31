using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoRainForumCommon;
using NoRainSDK.Models;
using NoRainSDK.src;

namespace NoRainForum.Computer.AdminWeb.Controllers
{
    public class UserController : Controller
    {
        public UserService UserSvc { get; set; }
        public UserController(UserService UserSvc)
        {
            this.UserSvc = UserSvc;
        }
        public async Task<IActionResult> List(int pageIndex = 1, int pageDataCount = 10)
        {
            var model = await UserSvc.GetPageDataAsync(pageIndex, pageDataCount);
            if (model == null)
            {
                return Content(UserSvc.ErrorMsg);
            }
            NoRainPage page = new NoRainPage();
            page.DataCount = model.TotalCount;
            page.PageIndex = pageIndex;
            page.Url = "/user/List?pageIndex=@parms";
            ViewData["Page"] = page.GetPaging();
            return View(model);

        }
        [HttpGet]
        public  IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddUserModel model)
        {
            if (await UserSvc.AddNewAsync(model)<1)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = UserSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok"});
        }
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var user = await UserSvc.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserModel model)
        {
            if (!await UserSvc.UpdateAsync(model))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = UserSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            if (!await UserSvc.DeleteAsync(id))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = UserSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}