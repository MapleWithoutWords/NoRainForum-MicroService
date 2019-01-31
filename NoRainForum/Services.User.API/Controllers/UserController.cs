using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.APICommon;
using Services.User.API.Models;
using Services.User.DTO;
using Services.User.IService;

namespace Services.User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserService UserSvc { get; set; }
        public UserController(IUserService UserSvc)
        {
            this.UserSvc = UserSvc;
        }

        [HttpGet("GetPageData")]
        public async Task<IActionResult> GetPageData(int pageIndex, int pageDataCount)
        {
            var users = await UserSvc.GetPageDataAsync(pageIndex, pageDataCount);
            var totalcount = await UserSvc.TotalCountAsync();
            return new JsonResult(new APIResult<ListModel<ListUserDTO>> {  Data=new ListModel<ListUserDTO> { Datas=users, TotalCount=totalcount } });
        }
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string nickName)
        {
            return new JsonResult(new APIResult<ListUserDTO> { Data = await UserSvc.GetByNickNameAsync(nickName) });
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            return new JsonResult(new APIResult<ListUserDTO> { Data = await UserSvc.GetByIdAsync(id) });
        }
        [HttpPut("Put")]
        public async Task<IActionResult> Put(AddUserModel model)
        {
            var user =await UserSvc.GetByEmailAsync(model.Email);
            if (user != null)
            {
                return new JsonResult(new APIResult<long>() { ErrorMsg = "邮箱已存在", Data = 400 }) { StatusCode = 400 };
            }
            var nameUser = await UserSvc.GetByNickNameAsync(model.NickName);
            if (nameUser!=null)
            {
                return new JsonResult(new APIResult<long>() { ErrorMsg = "昵称已存在", Data = 400 }) { StatusCode = 400 };
            }
            AddUserDTO dto = new AddUserDTO();
            dto.Email = model.Email;
            dto.Gender = model.Gender;
            dto.NickName = model.NickName;
            dto.Password = model.Password;
            return new JsonResult(new APIResult<long>() { Data = await UserSvc.AddNewAsync(dto) }) { };
        }
        [HttpPost("Post")]
        public async Task<IActionResult> Post(UpdateUserModel model)
        {
            var emailUser =await UserSvc.GetByEmailAsync(model.Email);
            if (emailUser != null)
            {
                if (emailUser.Id!= model.Id)
                {
                    return new JsonResult(new APIResult<long>() { ErrorMsg = "邮箱已存在", Data = 400 }) { StatusCode = 400 };
                }
            }
            UpdateUserDTO dto = new UpdateUserDTO();
            dto.Autograph = model.Autograph;
            dto.City = model.City;
            dto.Email = model.Email;
            dto.Gender = model.Gender;
            dto.Id = model.Id;
            dto.IsActive = model.IsActive;
            dto.NickName = model.NickName;
            await UserSvc.UpdateAsync(dto);
            return Ok();
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            await UserSvc.MarkDeleteAsync(id);
            return Ok();
        }
    }
}