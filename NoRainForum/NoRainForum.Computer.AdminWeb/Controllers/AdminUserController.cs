using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class AdminUserController : Controller
    {
        public AdminUserService AdminUserSvc { get; set; }
        public RoleService RoleSvc { get; set; }
        public AdminUserController(AdminUserService AdminUserSvc, RoleService RoleSvc)
        {
            this.AdminUserSvc = AdminUserSvc;
            this.RoleSvc = RoleSvc;
        }
        public async Task<IActionResult> List(int pageIndex = 1, int pageDataCount = 10)
        {
            ListModel<ListAdminUserDTO> model = await AdminUserSvc.GetPageDataAsync(pageIndex, pageDataCount);
            if (model == null)
            {
                return Content(AdminUserSvc.ErrorMsg);
            }
            NoRainPage page = new NoRainPage();
            page.DataCount = model.TotalCount;
            page.PageIndex = pageIndex;
            page.Url = "/AdminUser/List?pageIndex=@parms";
            ViewData["Page"] = page.GetPaging();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await RoleSvc.GetAllAsync();
            if (roles == null)
            {
                roles = new List<ListRolePermissionDTO>();
            }
            return View(roles);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddAdminUserAndRoleModel model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                AddAdminUserModel addAdminUserModel = new AddAdminUserModel();
                addAdminUserModel.Age = model.Age;
                addAdminUserModel.Gender = model.Gender;
                addAdminUserModel.Name = model.Name;
                addAdminUserModel.Password = model.Password;
                addAdminUserModel.PhoneNum = model.PhoneNum;
                long adminuserId = await AdminUserSvc.AddNewAsync(addAdminUserModel);
                if (adminuserId < 1)
                {
                    return Json(new AjaxResult { Status = "error", ErrorMsg = AdminUserSvc.ErrorMsg });
                }
                UpdateRoleOrPermissionModel updaterole = new UpdateRoleOrPermissionModel();
                updaterole.Id = adminuserId;
                updaterole.Ids = model.Ids;
                if (!await RoleSvc.UpdateAdminUserRolesAsynv(updaterole))
                {
                    return Json(new AjaxResult { Status = "error", ErrorMsg = AdminUserSvc.ErrorMsg });
                }
                scope.Complete();
                return Json(new AjaxResult { Status = "ok" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ListAdminUserDTO adminUser = await AdminUserSvc.GetByIdAsync(id);
            if (adminUser == null)
            {
                return NotFound();
            }

            var roles = await RoleSvc.GetAllAsync();
            var adminUserRoles = await RoleSvc.GetByAdminUserId(id);
            if (adminUserRoles == null)
            {
                adminUserRoles = new List<ListRolePermissionDTO>();
            }
            if (roles == null)
            {
                roles = new List<ListRolePermissionDTO>();
            }
            UpdateAdminUserListModel model = new UpdateAdminUserListModel();
            model.Age = adminUser.Age;
            model.CreateTime = adminUser.CreateTime;
            model.Gender = adminUser.Gender;
            model.Id = adminUser.Id;
            model.Name = adminUser.Name;
            model.PhoneNum = adminUser.PhoneNum;
            model.Roles = roles;
            model.AdminUserRoles = adminUserRoles;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAdminUserEditModel model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                UpdateAdminUserModel updateAdminUserModel = new UpdateAdminUserModel();
                updateAdminUserModel.Age = model.Age;
                updateAdminUserModel.Gender = model.Gender;
                updateAdminUserModel.Id = model.Id;
                updateAdminUserModel.Name = model.Name;
                updateAdminUserModel.PhoneNum = model.PhoneNum;
                //更新管理员
                if (!await AdminUserSvc.UpdateAsync(updateAdminUserModel))
                {
                    return Json(new AjaxResult { Status = "error", ErrorMsg = AdminUserSvc.ErrorMsg });
                }
                UpdateRoleOrPermissionModel updateRole = new UpdateRoleOrPermissionModel();
                updateRole.Id = model.Id;
                updateRole.Ids = model.RoleIds;
                //更新管理员角色
                if (!await RoleSvc.UpdateAdminUserRolesAsynv(updateRole))
                {
                    return Json(new AjaxResult { Status = "error", ErrorMsg = AdminUserSvc.ErrorMsg });
                }
                scope.Complete();
                return Json(new AjaxResult { Status = "ok" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            if (await AdminUserSvc.DeleteAsync(id))
            {

                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = AdminUserSvc.ErrorMsg });
            }
        }
    }
}
