using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.APICommon;
using Services.Other.API.Models;
using Services.Other.DTO;
using Services.Other.IService;

namespace Services.Other.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        public ISettingService SettingSvc { get; set; }
        public SettingController(ISettingService SettingSvc)
        {
            this.SettingSvc = SettingSvc;
        }
        [HttpGet("GetPageData")]
        public async Task<IActionResult> GetPageData(int pageIndex = 1, int pageDataCount = 10)
        {
            var settings = await SettingSvc.GetPageDataAsync(pageIndex, pageDataCount);
            var count = await SettingSvc.TotalCountAsync();
            return new JsonResult(new APIResult<ListModel<SettingDTO>> { Data = new ListModel<SettingDTO> { Datas = settings, TotalCount = count } });
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            return new JsonResult(new APIResult<SettingDTO> { Data = await SettingSvc.GetByIdAsync(id) });
        }
        [HttpPost("Post")]
        public async Task<IActionResult> Post(UpdateSettingModel model)
        {
            var en = await SettingSvc.GetByKeyAsync(model.KeyPari);
            if (en != null)
            {
                if (en.Id != model.Id)
                {
                    return new JsonResult(new APIResult<long> { ErrorMsg = "key存在" }) { StatusCode = 400 };
                }
            }
            await SettingSvc.UpdateAsync(model.Id, model.KeyPari, model.Key, model.Value);
            return Ok();
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put(AddSettingModel model)
        {
            var en = await SettingSvc.GetByKeyAsync(model.KeyPari);
            if (en != null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "key存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<long> { Data = await SettingSvc.AddNewAsync(model.KeyPari, model.Key, model.Value) });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            await SettingSvc.MarkDeleteAsync(id);
            return Ok();
        }
    }
}