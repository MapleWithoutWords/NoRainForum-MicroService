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
        /// <summary>
        /// 分页获取用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        [HttpGet("GetPageData")]
        public async Task<IActionResult> GetPageData(int pageIndex, int pageDataCount)
        {
            var users = await UserSvc.GetPageDataAsync(pageIndex, pageDataCount);
            var totalcount = await UserSvc.TotalCountAsync();
            return new JsonResult(new APIResult<ListModel<ListUserDTO>> { Data = new ListModel<ListUserDTO> { Datas = users, TotalCount = totalcount } });
        }
        /// <summary>
        /// 根据昵称获取用户
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string nickName)
        {
            return new JsonResult(new APIResult<ListUserDTO> { Data = await UserSvc.GetByNickNameAsync(nickName) });
        }
        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            return new JsonResult(new APIResult<ListUserDTO> { Data = await UserSvc.GetByIdAsync(id) });
        }
        /// <summary>
        /// 根据id集合获取多个用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("GetByIds")]
        public async Task<IActionResult> GetByIds(List<long> ids)
        {
            var users = await UserSvc.GetByIdsAsync(ids);
            if (users.Count != ids.Count)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "有用户不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<List<ListUserDTO>> { Data = users });
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Put")]
        public async Task<IActionResult> Put(AddUserModel model)
        {
            var user = await UserSvc.GetByEmailAsync(model.Email);
            if (user != null)
            {
                return new JsonResult(new APIResult<long>() { ErrorMsg = "邮箱已存在", Data = 400 }) { StatusCode = 400 };
            }
            var nameUser = await UserSvc.GetByNickNameAsync(model.NickName);
            if (nameUser != null)
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
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> Post(UpdateUserModel model)
        {
            var emailUser = await UserSvc.GetByEmailAsync(model.Email);
            if (emailUser != null)
            {
                if (emailUser.Id != model.Id)
                {
                    return new JsonResult(new APIResult<long>() { ErrorMsg = "邮箱已存在", Data = 400 }) { StatusCode = 400 };
                }
            }
            var nameUser = await UserSvc.GetByNickNameAsync(model.NickName);
            if (nameUser != null)
            {
                if (nameUser.Id != model.Id)
                {
                    return new JsonResult(new APIResult<long>() { ErrorMsg = "昵称已存在", Data = 400 }) { StatusCode = 400 };
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


        [HttpGet("ActiveEmail")]
        public async Task<IActionResult> ActiveEmail(long id)
        {
            var user = await UserSvc.GetByIdAsync(id);
            if (user == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "用户不存在" }) { StatusCode = 400 };
            }
            await UserSvc.ActiveEmailAsync(id);
            return Ok();
        }

        [HttpPost("EditPassword")]
        public async Task<IActionResult> EditPassword(RePasswordModel model)
        {
            var user = await UserSvc.GetByEmailAsync(model.Email);
            if (user == null)
            {
                return new JsonResult(new APIResult<long>() { ErrorMsg = "用户不存在", Data = 400 }) { StatusCode = 400 };
            }
            await UserSvc.EditorPasswordAsync(user.Id, model.NewPassword);
            return Ok();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var user = await UserSvc.GetByEmailAsync(model.Email);
            if (user == null)
            {
                return new JsonResult(new APIResult<int> { ErrorMsg = "用户名或密码错误" }) { StatusCode = 400 };
            }
            if (await UserSvc.IsLockAsync(user.Id))
            {
                var min = (DateTime.Now - user.LoginErrorTime).TotalMinutes;
                return new JsonResult(new APIResult<int> { ErrorMsg = $"用户已被锁定,{min}分钟后再试" }) { StatusCode = 400 };
            }
            if (!await UserSvc.LoginAsync(model.Email, model.Password))
            {
                await UserSvc.LockUserAsync(user.Id);
                return new JsonResult(new APIResult<int> { ErrorMsg = "用户名或密码错误" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<ListUserDTO> { Data = user });
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            await UserSvc.MarkDeleteAsync(id);
            return Ok();
        }
    }
}