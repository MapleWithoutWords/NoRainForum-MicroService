using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Admin.API.Filters;
using Services.Admin.API.Models;
using Services.Admin.DTO;
using Services.Admin.IService;
using Services.APICommon;
using Services.Common;

namespace Services.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        public IAdminUserService AdminUserSvc { get; set; }
        public AdminUserController(IAdminUserService AdminUserSvc)
        {
            this.AdminUserSvc = AdminUserSvc;
        }
        [HttpGet("GetPageData")]
        public async Task<IActionResult> GetPageData(int pageIndex = 1, int pageDataCount = 10)
        {
            var adminusers = await AdminUserSvc.GetPageDataAsync(pageIndex, pageDataCount);
            long count = await AdminUserSvc.TotalCountAsync();
            var data = new APIResult<ListModel<ListAdminUserDTO>> { Data = new ListModel<ListAdminUserDTO> { Datas=adminusers, TotalCount=count } };
            return new JsonResult(data);
        }
        [HttpGet("GetByPhoneNum")]
        public async Task<IActionResult> GetByPhoneNum(string phoneNum)
        {
            var model = await AdminUserSvc.GetByPhoneNumAsync(phoneNum);
            
            return new JsonResult(new APIResult<ListAdminUserDTO> { Data = model });
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            var model = await AdminUserSvc.GetByIdAsync(id);
            if (model == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "id不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<ListAdminUserDTO> { Data = model });
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put([FromBody]AddAdminUserModel model)
        { 
            if (await AdminUserSvc.GetByPhoneNumAsync(model.PhoneNum) != null)
            {
                return new JsonResult(new APIResult<int> { ErrorMsg = "电话号码已存在" }) { StatusCode = 400 };
            }
            AddAdminUserDTO dto = new AddAdminUserDTO();
            dto.Age = model.Age;
            dto.Gender = model.Gender;
            dto.Name = model.Name;
            dto.Password = model.Password;
            dto.PhoneNum = model.PhoneNum;
            return new JsonResult(new APIResult<long> { Data = await AdminUserSvc.AddNewAsync(dto) });
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromBody]UpdateAdminUserModel model)
        {
            var idEntity= await AdminUserSvc.GetByIdAsync(model.Id);
            if (idEntity==null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg="管理员不存在" }) {StatusCode=400 };
            }
            var phoneEntity = await AdminUserSvc.GetByPhoneNumAsync(model.PhoneNum);
            if (phoneEntity != null)
            {
                if (phoneEntity.Id!=idEntity.Id)
                {
                    return new JsonResult(new APIResult<long> { ErrorMsg = "电话号码已存在" }) {StatusCode=400 };
                }
            }
            UpdateAdminUserDTO dto = new UpdateAdminUserDTO();
            dto.Age = model.Age;
            dto.Id = model.Id;
            dto.Gender = model.Gender;
            dto.Name = model.Name;
            dto.PhoneNum = model.PhoneNum;
            await AdminUserSvc.UpdateAsync(dto);
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            var adminUser = await AdminUserSvc.GetByPhoneNumAsync(model.PhoneNum);
            if (adminUser == null)
            {
                return new JsonResult(new APIResult<int> { ErrorMsg = "用户名或密码错误" }) { StatusCode = 400 };
            }
            if (await AdminUserSvc.IsLockAsync(adminUser.Id))
            {
                var min = (DateTime.Now - adminUser.LoginErrorTime).TotalMinutes;
                return new JsonResult(new APIResult<int> { ErrorMsg = $"用户已被锁定,{min}分钟后再试" }) { StatusCode = 400 };
            }
            if (!await AdminUserSvc.LoginAsync(model.PhoneNum, model.Password))
            {
                await AdminUserSvc.LockAdminUserAsync(adminUser.Id);
                return new JsonResult(new APIResult<int> { ErrorMsg = "用户名或密码错误" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<ListAdminUserDTO> { Data = adminUser });
        }
        
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            var adminUser = await AdminUserSvc.GetByIdAsync(model.Id);
            if (adminUser==null)
            {
                return new JsonResult(new APIResult<int> { ErrorMsg = "管理员不存咋" }) { StatusCode = 400 };
            }
            if (!await AdminUserSvc.LoginAsync(adminUser.PhoneNum, model.OldPassword))
            {
                return new JsonResult(new APIResult<int> { ErrorMsg = "旧密码错误" }) { StatusCode = 400 };
            }
            await AdminUserSvc.ChangePasswordAsync(model.Id,model.NewPassword);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            await AdminUserSvc.MarkDeleteAsync(id);
            return Ok();
        }
    }
}