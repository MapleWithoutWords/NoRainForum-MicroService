using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.APICommon;
using Services.Post.API.Models;
using Services.Post.DTO;
using Services.Post.IService;

namespace Services.Post.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTypeController : ControllerBase
    {
        public IPostTypeService PostTypeSvc { get; set; }
        public PostTypeController(IPostTypeService postTypeSvc)
        {
            PostTypeSvc = postTypeSvc;
        }
        /// <summary>
        /// 获取所有帖子类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(
                new APIResult<List<IdNameDTO>>()
                {
                    Data = await PostTypeSvc.GetAll()
                }
            );
        }
        /// <summary>
        /// 分页获取帖子类型
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        [HttpGet("GetPageData")]
        public async Task<IActionResult> GetPageData(int pageIndex = 1, int pageDataCount = 10)
        {
            var postStatus = await PostTypeSvc.GetPageDataAsync(pageIndex, pageDataCount);
            var totalCount = await PostTypeSvc.TotalCountAsync();
            return new JsonResult(
                new APIResult<ListModel<IdNameDTO>>()
                {
                    Data = new ListModel<IdNameDTO> { Datas = postStatus, TotalCount = totalCount }
                });
        }
        /// <summary>
        /// 根据Id获取帖子类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            var res = await PostTypeSvc.GetByIdAsync(id);
            if (res == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "id不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<IdNameDTO> { Data = res });
        }
        /// <summary>
        /// 添加帖子类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Put")]
        public async Task<IActionResult> Put(AddIdNameModel model)
        {
            var entity = await PostTypeSvc.GetByNameAsync(model.Name);
            if (entity != null)
            {
                return new JsonResult(
                   new APIResult<long>()
                   {
                       ErrorMsg = "该帖子类型已存在"
                   }
               )
                { StatusCode = 400 };
            }
            AddIdNameDTO dto = new AddIdNameDTO();
            dto.Description = model.Description;
            dto.Name = model.Name;
            return new JsonResult(
                new APIResult<long>()

                {
                    Data = await PostTypeSvc.AddNewAsync(dto)
                }
            );
        }
        /// <summary>
        /// 删除帖子类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            await PostTypeSvc.MarkDeleteAsync(id);
            return Ok();
        }
        /// <summary>
        /// 修改帖子类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> Post(UpdateIdNameModel model)
        {
            var entity = await PostTypeSvc.GetByNameAsync(model.Name);
            if (entity != null)
            {
                if (entity.Id != model.Id)
                {
                    return new JsonResult(
                       new APIResult<long>()
                       {
                           ErrorMsg = "该类型名称已存在"
                       }
                   )
                    { StatusCode = 400 };
                }
            }
            UpdateIdNameDTO dto = new UpdateIdNameDTO();
            dto.Id = model.Id;
            dto.Name = model.Name;
            dto.Description = model.Description;
            await PostTypeSvc.UpdateAsync(dto);
            return Ok();
        }
    }
}