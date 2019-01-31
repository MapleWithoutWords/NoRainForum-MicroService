using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Admin.API.Filters;
using Services.Admin.API.Models;
using Services.Admin.DTO;
using Services.Admin.IService;
using Services.APICommon;

namespace Services.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public IRoleService RoleSvc { get; set; }
        public IAdminUserService AdminUserSvc { get; set; }
        public RoleController(IRoleService RoleSvc, IAdminUserService AdminUserSvc)
        {
            this.RoleSvc = RoleSvc;
            this.AdminUserSvc = AdminUserSvc;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(new APIResult<List<ListRolePermissionDTO>> { Data = await RoleSvc.GetAllAsync() }) { };
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            var model = await RoleSvc.GetByIdAsync(id);
            if (model==null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "id不存在" }) { StatusCode=400};
            }
            return new JsonResult(new APIResult<ListRolePermissionDTO> { Data = model });
        }

        [HttpGet("GetByAdminUserId")]
        public async Task<IActionResult> GetByAdminUserId(long adminUserId)
        {
            var model = await RoleSvc.GetByAdminUserIdAsync(adminUserId);
            if (model == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "id不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<List<ListRolePermissionDTO>> { Data = model });
        }
        [HttpGet("GetPageData")]
        public async Task<IActionResult> GetPageData(int pageIndex = 1, int pageDataCount = 10)
        {
            var roles = await RoleSvc.GetPageDataAsync(pageIndex, pageDataCount);
            var totalCount = await RoleSvc.TotalCountAsync();
            return new JsonResult(new APIResult<ListModel<ListRolePermissionDTO>> { Data=new ListModel<ListRolePermissionDTO> { Datas=roles, TotalCount=totalCount } });
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put(AddRolePermissionModel model)
        {
            if (await RoleSvc.GetByNameAsync(model.Name) != null)
            {
                return new JsonResult(new APIResult<int> { ErrorMsg = "该权限已存在" }) { StatusCode = 400 };
            }
            AddRolePermissionDTO dto = new AddRolePermissionDTO();
            dto.Name = model.Name;
            dto.Description = model.Description;
            return new JsonResult(new APIResult<long> { Data = await RoleSvc.AddNewAsync(dto) });
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(UpdateRolePermissionModel model)
        {
            var role = await RoleSvc.GetByNameAsync(model.Name);
            if (role != null)
            {
                if (role.Id != model.Id)
                {
                    return new JsonResult(new APIResult<int> { ErrorMsg = "该角色已存在" }) { StatusCode = 400 };
                }
            }
            UpdateRolePermissionDTO dto = new UpdateRolePermissionDTO();
            dto.Description = model.Description;
            dto.Id = model.Id;
            dto.Name = model.Name;
            await RoleSvc.UpdateAsync(dto);
            return Ok();
        }

        [HttpPost("UpdateAdminUserRoles")]
        public async Task<IActionResult> UpdateAdminUserRoles(UpdateRoleOrPermissionModel model)
        {
            //string token = JWTHelper.GetToken(HttpContext, "token");
            //if (!JWTHelper.Decrypt(token, out ListAdminUserDTO adminUser))
            //{
            //    return new JsonResult(new APIResult<long> { ErrorMsg = "请先登录！" }) { StatusCode = 401 };
            //}
            await RoleSvc.UpdateAdminUserToRolesAsync(model.Id, model.Ids);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            await RoleSvc.MarkDeleteAsync(id);
            return Ok();
        }
    }
}