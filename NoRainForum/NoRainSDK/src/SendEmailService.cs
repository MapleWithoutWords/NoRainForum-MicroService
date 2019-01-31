using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NoRainSDK.http;
using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoRainSDK.src
{
    public class SendEmailService
    {
        public SendEmailService(string appKey, string appSecret)
        {
            AppInfoSetting.Setting.AppKey = appKey;
            AppInfoSetting.Setting.AppSecret = appSecret;
            Client = new SDKClient(AppInfoSetting.Setting, "http://127.0.0.1:8888/OtherService/api/SendEmail/");
        }
        private SDKClient Client { get; set; }
        public string ErrorMsg { get; set; }
        public async Task<string> SendRegisterEmailAsync(SendEmailModel model)
        {
            var result = await Client.PostAsync("SendRegisterEmail", model);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return obj["data"]==null?null: obj["data"].ToString();
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
    }
}
