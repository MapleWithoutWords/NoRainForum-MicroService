using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.APICommon.appinfo;
using Services.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services.APICommon
{
    public class APIAuthorizationFilter : IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ApiAuthorAttribute[] attris = (ApiAuthorAttribute[])((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(ApiAuthorAttribute), false);
            if (attris.Length>0)
            {
                return;
            }


            //从报文头中获取Appkey和sign
            StringValues appkeys;
            StringValues signs;
            if (!context.HttpContext.Request.Headers.TryGetValue("appKey", out appkeys))
            {
                context.Result = new JsonResult(new APIResult<int> { ErrorMsg = "appkey不存在" }) { StatusCode = 400 };
                return;
            }
            if (!context.HttpContext.Request.Headers.TryGetValue("sign", out signs))
            {
                context.Result = new JsonResult(new APIResult<int> { ErrorMsg = "sign不存在" }) { StatusCode = 400 };
                return;
            }
            string sign = signs.First();
            string key = appkeys.First();

            //获取AppInfo
            //var task = GetDTO(key);
            //task.Wait();
            AppInfoDTO dto = new AppInfoDTO { AppKey = "ea92959a-d2af-4ef0-ab6f-3b30f5d4eced1028139084@qq.com", AppSecret = "0cc8e711-81e3-43ba-aeff-8fab8212efd2" };
            if (dto == null)
            {
                context.Result = new JsonResult(new APIResult<int> { ErrorMsg = "appkey错误" }) { StatusCode = 401 };
                return;
            }
            if (key != dto.AppKey)
            {
                context.Result = new JsonResult(new APIResult<int> { ErrorMsg = "appkey错误" }) { StatusCode = 401 };
                return;
            }
            //计算Sign
            string thSign = CalceSign(context, dto.AppSecret);

            if (!sign.Equals(thSign, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Result = new JsonResult(new APIResult<int> { ErrorMsg = "sign错误" }) { StatusCode = 401 };
                return;
            }
        }

        private string CalceSign(AuthorizationFilterContext context, string AppSecret)
        {
            string method = context.HttpContext.Request.Method.ToLower();
            string path = context.HttpContext.Request.Path.Value;
            string thSign;
            if (method == "get" || method == "delete")
            {
                IQueryCollection reqUrls = context.HttpContext.Request.Query;
                StringBuilder sb = new StringBuilder();
                var querys = reqUrls.OrderBy(k => k.Key).Select(e => e.Key + "=" + e.Value);
                string query = string.Join("&", querys);
                thSign = MD5Helper.CalcMD5(path + "?" + query + AppSecret);
            }
            else
            {
                //读取报文体
                var requestBodyStream = new MemoryStream();
                context.HttpContext.Request.Body.CopyTo(requestBodyStream);
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                string body = new StreamReader(requestBodyStream, Encoding.UTF8).ReadToEnd();
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                context.HttpContext.Request.Body = requestBodyStream;
                thSign = MD5Helper.CalcMD5(path + AppSecret + body);
            }
            return thSign;
        }
        /// <summary>
        /// 获取AppInfo信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<AppInfoDTO> GetDTO(string key)
        {
            AppInfoDTO dto = new AppInfoDTO();
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"http://localhost:8000/api/AppInfo/Get?appKey={key}");
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                var json = await res.Content.ReadAsStringAsync();
                JObject obj = JsonConvert.DeserializeObject<JObject>(json);
                if (obj["data"] == null)
                {
                    return null;
                }
                dto = JsonConvert.DeserializeObject<AppInfoDTO>(obj["data"].ToString());
                return dto;
            }
        }
    }
}
