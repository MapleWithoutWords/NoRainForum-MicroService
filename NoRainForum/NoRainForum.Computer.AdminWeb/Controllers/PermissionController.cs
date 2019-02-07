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
    public class PermissionController : Controller
    {
        public PermissionService PerSvc { get; set; }
        public PermissionController(PermissionService PerSvc)
        {
            this.PerSvc = PerSvc;
        }
        [HttpGet]
        [CheckPermission("Permission.List")]
        public async Task<IActionResult> List(int pageIndex=1,int pageDataCount=10)
        {
            var pers = await PerSvc.GetPageDataAsync(pageIndex, pageDataCount);
            if (pers == null)
            {
                return Content(PerSvc.ErrorMsg);
            }
            NoRainPage page = new NoRainPage();
            page.DataCount = pers.TotalCount;
            page.PageIndex = pageIndex;
            page.Url = "/Permission/List?pageIndex=@parms";
            ViewData["Page"] = page.GetPaging();
            return View(pers);
        }
        [HttpPost]
        [CheckPermission("Permission.Delete")]
        public async Task<IActionResult> Delete(long id)
        {

            if (!await PerSvc.DeleteAsync(id))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PerSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        [CheckPermission("Permission.Add")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [CheckPermission("Permission.Add")]
        public async Task<IActionResult> Add(AddRolePermissionModel model)
        {
            if (await PerSvc.AddNewAsync(model)<1)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PerSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok"});
        }
        [HttpGet]
        [CheckPermission("Permission.Edit")]
        public async Task<IActionResult> Edit(long id)
        {
            ListRolePermissionDTO model = await PerSvc.GetByIdAsync(id);
            if (model==null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        [CheckPermission("Permission.Edit")]
        public async Task<IActionResult> Edit(UpdateRolePermissionModel model)
        {
            if (!await PerSvc.UpdateAsync(model) )
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PerSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}