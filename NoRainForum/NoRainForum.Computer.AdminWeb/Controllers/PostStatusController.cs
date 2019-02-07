using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoRainForum.Computer.AdminWeb.filters;
using NoRainForumCommon;
using NoRainSDK.Models;
using NoRainSDK.src;

namespace NoRainForum.Computer.AdminWeb.Controllers
{
    public class PostStatusController : Controller
    {
        public PostStatusService StatuSvc { get; set; }
        public PostStatusController(PostStatusService StatuSvc)
        {
            this.StatuSvc = StatuSvc;
        }
        [HttpGet]
        [CheckPermission("PostStatus.List")]
        public async Task<IActionResult> List(int pageIndex = 1, int pageDataCount = 10)
        {
            var postTypes = await StatuSvc.GetPageDataAsync(pageIndex, pageDataCount);
            if (postTypes == null)
            {
                return Content(StatuSvc.ErrorMsg);
            }
            NoRainPage page = new NoRainPage();
            page.DataCount = postTypes.TotalCount;
            page.PageIndex = pageIndex;
            page.Url = "/PostStatus/List?pageIndex=@parms";
            ViewData["Page"] = page.GetPaging();
            return View(postTypes);
        }

        [HttpPost]
        [CheckPermission("PostStatus.Delete")]
        public async Task<IActionResult> Delete(long id)
        {

            if (!await StatuSvc.DeleteAsync(id))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = StatuSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        [CheckPermission("PostStatus.Add")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [CheckPermission("PostStatus.Add")]
        public async Task<IActionResult> Add(AddIdNameModel model)
        {
            if (await StatuSvc.AddNewAsync(model) < 1)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = StatuSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        [CheckPermission("PostStatus.Edit")]
        public async Task<IActionResult> Edit(long id)
        {
            var model = await StatuSvc.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        [CheckPermission("PostStatus.Edit")]
        public async Task<IActionResult> Edit(UpdateIdNameModel model)
        {
            if (!await StatuSvc.UpdateAsync(model))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = StatuSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}