using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using NoRainForum.Computer.AdminWeb.Models;
using NoRainForumCommon;
using NoRainSDK.Models;
using NoRainSDK.src;

namespace NoRainForum.Computer.AdminWeb.Controllers
{
    public class RoleController : Controller
    {
        public RoleService RoleSvc { get; set; }
        public PermissionService PerSvc { get; set; }
        public RoleController(RoleService RoleSvc,PermissionService PerSvc)
        {
            this.RoleSvc = RoleSvc;
            this.PerSvc = PerSvc;
        }
        public async Task<IActionResult> List(int pageIndex=1,int pageDataCount=10)
        {
            var model =await RoleSvc.GetPageDataAsync(pageIndex, pageDataCount);
            if (model == null)
            {
                return Content(RoleSvc.ErrorMsg);
            }
            NoRainPage page = new NoRainPage();
            page.DataCount = model.TotalCount;
            page.PageIndex = pageIndex;
            page.Url = "/role/List?pageIndex=@parms";
            ViewData["Page"] = page.GetPaging();
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var pers =await PerSvc.GetAllAsync<List<ListRolePermissionDTO>>();
            return View(pers);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddRoleModel model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                AddRolePermissionModel addrole = new AddRolePermissionModel();
                addrole.Description = model.Description;
                addrole.Name = model.Name;
                long roleId = await RoleSvc.AddNewAsync(addrole);
                if (roleId < 1)
                {
                    return Json(new AjaxResult { Status = "error", ErrorMsg = RoleSvc.ErrorMsg });
                }
                UpdateRoleOrPermissionModel update = new UpdateRoleOrPermissionModel();
                update.Id = roleId;
                update.Ids = model.Ids;
                if (!await PerSvc.UpdateRoleToPermissesAsync(update))
                {
                    return Json(new AjaxResult { Status = "error", ErrorMsg = RoleSvc.ErrorMsg });
                }
                scope.Complete();
                return Json(new AjaxResult { Status = "ok" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var role = await RoleSvc.GetByIdAsync(id);
            if (role==null)
            {
                return NotFound();
            }
            var rolePers = await PerSvc.GetByRoleIdAsync(role.Id);
            var pers =await PerSvc.GetAllAsync<List<ListRolePermissionDTO>>();
            RoleListModel model = new RoleListModel();
            model.Role = role;
            model.RolePers = rolePers;
            model.Pers = pers;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleModel model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                UpdateRolePermissionModel editrole = new UpdateRolePermissionModel();
                editrole.Description = model.Description;
                editrole.Name = model.Name;
                editrole.Id = model.Id;
                if (!await RoleSvc.UpdateAsync(editrole))
                {
                    return Json(new AjaxResult { Status = "error", ErrorMsg = RoleSvc.ErrorMsg });
                }
                UpdateRoleOrPermissionModel update = new UpdateRoleOrPermissionModel();
                update.Id = editrole.Id;
                update.Ids = model.Ids;
                if (!await PerSvc.UpdateRoleToPermissesAsync(update))
                {
                    return Json(new AjaxResult { Status = "error", ErrorMsg = RoleSvc.ErrorMsg });
                }
                scope.Complete();
                return Json(new AjaxResult { Status = "ok" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            if (!await RoleSvc.DeleteAsync(id))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = RoleSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status="ok"});
        }

    }
}