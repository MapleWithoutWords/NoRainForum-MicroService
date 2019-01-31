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
    public class AppInfoController : ControllerBase
    {
        public IAppInfoService AppInfoSvc { get; set; }
        public AppInfoController(IAppInfoService AppInfoSvc)
        {
            this.AppInfoSvc = AppInfoSvc;
        }
        [HttpPut("Put")]
        public async Task<IActionResult> Put(AddAppInfoModel model)
        {
            string appkey = Guid.NewGuid().ToString() + model.Email;
            string appsecret = Guid.NewGuid().ToString();
            if (await AppInfoSvc.GetByAppKeyAsync(appkey) != null)
            {
                return new JsonResult(new APIResult<int> { ErrorMsg = "appkey存在" }) { StatusCode = 400 };
            }
            AddAppInfoDTO dto = new AddAppInfoDTO();
            dto.AppKey = appkey;
            dto.AppSecret = appsecret;
            dto.Email = model.Email;
            await AppInfoSvc.AddNewAsync(dto);
            return new JsonResult(new APIResult<AddAppInfoDTO> { Data = dto });
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(string appKey)
        {
            return new JsonResult(new APIResult<AppInfoDTO> { Data = await AppInfoSvc.GetByAppKeyAsync(appKey) });
        }
    }
}