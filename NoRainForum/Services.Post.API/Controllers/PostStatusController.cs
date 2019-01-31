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
    public class PostStatusController : ControllerBase
    {
        public IPostStatusService PostStatusSvc { get; set; }
        public PostStatusController(IPostStatusService PostStatusSvc)
        {
            this.PostStatusSvc = PostStatusSvc;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(
                new APIResult<List<IdNameDTO>>()
                {
                    Data = await PostStatusSvc.GetAll()
                }
            );
        }
        [HttpGet("GetPageData")]
        public async Task<IActionResult> GetPageData(int pageIndex = 1, int pageDataCount = 10)
        {
            var postStatus = await PostStatusSvc.GetPageDataAsync(pageIndex, pageDataCount);
            var totalCount = await PostStatusSvc.TotalCountAsync();
            return new JsonResult(
                new APIResult<ListModel<IdNameDTO>>()
                {
                    Data = new ListModel<IdNameDTO> { Datas= postStatus, TotalCount=totalCount }
                });
        }
            

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            var res = await PostStatusSvc.GetByIdAsync(id);
            if (res == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "id不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<IdNameDTO> { Data = res });
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put(AddIdNameModel model)
        {
            var entity = await PostStatusSvc.GetByNameAsync(model.Name);
            if (entity != null)
            {
                return new JsonResult(new APIResult<long>()
                {
                    ErrorMsg = "该帖子类型已存在"
                })
                { StatusCode = 400 };
            }
            AddIdNameDTO dto = new AddIdNameDTO();
            dto.Description = model.Description;
            dto.Name = model.Name;
            return new JsonResult(new APIResult<long>()
            {
                Data = await PostStatusSvc.AddNewAsync(dto)
            });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            await PostStatusSvc.MarkDeleteAsync(id);
            return Ok();
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(UpdateIdNameModel model)
        {
            var entity = await PostStatusSvc.GetByNameAsync(model.Name);
            if (entity != null)
            {
                if (entity.Id != model.Id)
                {
                    return new JsonResult(new APIResult<long>(){
                           ErrorMsg = "该类型名称已存在"
                       }){ StatusCode = 400 };
                }
            }
            UpdateIdNameDTO dto = new UpdateIdNameDTO();
            dto.Id = model.Id;
            dto.Name = model.Name;
            dto.Description = model.Description;
            await PostStatusSvc.UpdateAsync(dto);
            return Ok();
        }

    }
}