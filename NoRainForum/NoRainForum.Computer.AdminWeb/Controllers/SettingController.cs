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
    public class SettingController : Controller
    {
        public SettingService SettingSvc { get; set; }
        public SettingController(SettingService SettingSvc)
        {
            this.SettingSvc = SettingSvc;
        }
        [CheckPermission("Setting.List")]
        public async Task<IActionResult> List(int pageIndex = 1, int pageDataCount = 10)
        {
            var model = await SettingSvc.GetPageDataAsync(pageIndex, pageDataCount);
            if (model == null)
            {
                return Content(SettingSvc.ErrorMsg);
            }
            NoRainPage page = new NoRainPage();
            page.DataCount = model.TotalCount;
            page.PageIndex = pageIndex;
            page.Url = "/setting/List?pageIndex=@parms";
            ViewData["Page"] = page.GetPaging();
            return View(model);

        }
        [HttpPost]
        [CheckPermission("Setting.Delete")]
        public async Task<IActionResult> Delete(long id)
        {

            if (!await SettingSvc.DeleteAsync(id))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = SettingSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        [CheckPermission("Setting.Add")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [CheckPermission("Setting.Add")]
        public async Task<IActionResult> Add(AddSettingModel model)
        {
            if (await SettingSvc.AddNewAsync(model) < 1)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = SettingSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        [CheckPermission("Setting.Edit")]
        public async Task<IActionResult> Edit(long id)
        {
            var model = await SettingSvc.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        [CheckPermission("Setting.Edit")]
        public async Task<IActionResult> Edit(UpdateSettingModel model)
        {
            if (!await SettingSvc.UpdateAsync(model))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = SettingSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}
