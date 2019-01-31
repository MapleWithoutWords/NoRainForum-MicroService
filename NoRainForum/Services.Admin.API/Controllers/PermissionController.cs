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
    public class PermissionController : ControllerBase
    {
        public IPermissionService PerSvc { get; set; }
        public IAdminUserService AdminSvc { get; set; }
        public PermissionController(IPermissionService PerSvc,IAdminUserService AdminSvc)
        {
            this.PerSvc = PerSvc;
            this.AdminSvc = AdminSvc;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(new APIResult<List<ListRolePermissionDTO>> { Data = await PerSvc.GetAllAsync() }) { };
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            var model = await PerSvc.GetByIdAsync(id);
            if (model == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "id不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<ListRolePermissionDTO> { Data = model });
        }
        
        [HttpGet("GetByRoleId")]
        public async Task<IActionResult> GetByRoleId(long roleId)
        {
            var model = await PerSvc.GetByRoleIdAsync(roleId);
            if (model == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "id不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<List<ListRolePermissionDTO>> { Data = model });
        }
        [HttpGet("GetPageData")]
        public async Task<IActionResult> GetPageData(int pageIndex = 1, int pageDataCount = 10)
        {
            var roles = await PerSvc.GetPageDataAsync(pageIndex, pageDataCount);
            var totalCount = await PerSvc.TotalCountAsync();
            return new JsonResult(new APIResult<ListModel<ListRolePermissionDTO>> { Data = new ListModel<ListRolePermissionDTO> { Datas = roles, TotalCount = totalCount } });
        }

        [HttpGet("CheckPermission")]
        public async Task<IActionResult> CheckPermission(string perName)
        {
            string token = JWTHelper.GetToken(HttpContext, "token");
            if (!JWTHelper.Decrypt(token, out ListAdminUserDTO adminUser))
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "请先登录！" }) { StatusCode = 401 };

            }
            if (await PerSvc.CheckPermissionAsync(adminUser.Id, perName))
            {
                return Ok();
            }
            return Unauthorized();
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put(AddRolePermissionModel model)
        {
            if (await PerSvc.GetByNameAsync(model.Name) != null)
            {
                return new JsonResult(new APIResult<int> { ErrorMsg = "该权限已存在" }) { StatusCode = 400 };
            }
            AddRolePermissionDTO dto = new AddRolePermissionDTO();
            dto.Name = model.Name;
            dto.Description = model.Description;
            return new JsonResult(new APIResult<long> { Data = await PerSvc.AddNewAsync(dto) });
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(UpdateRolePermissionModel model)
        {
            var per = await PerSvc.GetByNameAsync(model.Name);
            if (per != null)
            {
                if (per.Id != model.Id)
                {
                    return new JsonResult(new APIResult<int> { ErrorMsg = "该权限已存在" }) { StatusCode = 400 };
                }
            }
            UpdateRolePermissionDTO dto = new UpdateRolePermissionDTO();
            dto.Description = model.Description;
            dto.Id = model.Id;
            dto.Name = model.Name;
            await PerSvc.UpdateAsync(dto);
            return Ok();
        }

        [HttpPost("CheckPermission")]
        public async Task<IActionResult> CheckPermission(CheckPermissionModel model)
        {
            var adminUser =await AdminSvc.GetByIdAsync(model.AdminUserId);
            if (adminUser == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg="管理员不存在" }) { StatusCode=400};
            }
            var permission = await PerSvc.GetByNameAsync(model.PermissionName);
            if (permission == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "权限名不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<bool> { Data=await PerSvc.CheckPermissionAsync(model.AdminUserId,model.PermissionName)}) ;
        }

        [HttpPost("UpdateRoleToPermisses")]
        public async Task<IActionResult> UpdateRoleToPermisses(UpdateRoleOrPermissionModel model)
        {
            await PerSvc.UpdateRoleToPermissesAsync(model.Id, model.Ids);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            await PerSvc.MarkDeleteAsync(id);
            return Ok();
        }
    }
}