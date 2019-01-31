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
    public class PostTypeController : Controller
    {
        public PostTypeService TypeSvc { get; set; }
        public PostTypeController(PostTypeService TypeSvc)
        {
            this.TypeSvc = TypeSvc;
        }
        [HttpGet]
        public async Task<IActionResult> List(int pageIndex=1,int pageDataCount=10)
        {
            var postTypes =await TypeSvc.GetPageDataAsync(pageIndex,pageDataCount);
            if (postTypes == null)
            {
                return Content(TypeSvc.ErrorMsg);
            }
            NoRainPage page = new NoRainPage();
            page.DataCount = postTypes.TotalCount;
            page.PageIndex = pageIndex;
            page.Url = "/PostType/List?pageIndex=@parms";
            ViewData["Page"] = page.GetPaging();
            return View(postTypes);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {

            if (!await TypeSvc.DeleteAsync(id))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = TypeSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddIdNameModel model)
        {
            if (await TypeSvc.AddNewAsync(model) < 1)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = TypeSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var model = await TypeSvc.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateIdNameModel model)
        {
            if (!await TypeSvc.UpdateAsync(model))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = TypeSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }

    }
}