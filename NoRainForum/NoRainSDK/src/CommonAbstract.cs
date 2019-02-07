using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NoRainSDK.http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoRainSDK.src
{
    public abstract class CommonAbstract<T> where T : class
    {
        public CommonAbstract(string appKey, string apSecret, string serverRoot)
        {
            AppInfoSetting.Setting.AppKey = appKey;
            AppInfoSetting.Setting.AppSecret = apSecret;
            client = new SDKClient(AppInfoSetting.Setting,serverRoot);
        }
        protected SDKClient client;
        public string ErrorMsg { get; set; }

        public async Task<TObj> GetAllAsync<TObj>()where TObj:class
        {
            IDictionary<string, string> pairs = new Dictionary<string, string>();
            SDKResult result = await client.GetAsync("GetAll", pairs);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<TObj>(obj["data"]==null?"": obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return default(TObj);
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
        public async Task<T> GetPageDataAsync(int pageIndex, int pageDataCount)
        {
            IDictionary<string, string> pairs = new Dictionary<string, string>();
            pairs["pageIndex"] = pageIndex.ToString();
            pairs["pageDataCount"] = pageDataCount.ToString();

            SDKResult result = await client.GetAsync("GetPageData", pairs);
            
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return default(T);
            }
            else
            {
                throw new ApplicationException($"未知的错误,错误状态码：{result.StatusCode}");
            }
        }
        public async Task<bool> DeleteAsync(long id)
        {
            IDictionary<string, string> pairs = new Dictionary<string, string>();
            pairs[nameof(id)] = id.ToString();
            SDKResult result = await client.DeleteAsync("Delete", pairs);

            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return false;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
        protected async Task<long> AddNewAsync<TObj>(TObj dto) where TObj : class
        {
            SDKResult result = await client.PutAsync("Put", dto);

            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return -1;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<long>(obj["data"].ToString());
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }

        protected async Task<bool> UpdateAsync<TObj>(TObj dto) where TObj : class
        {
            SDKResult result = await client.PostAsync("Post", dto);

            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return false;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }

        protected async Task<TObj> GetByIdAsync<TObj>(long id) where TObj : class
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs["id"] = id.ToString();
            SDKResult result = await client.GetAsync("GetById", pairs);

            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest|| result.StatusCode ==  System.Net.HttpStatusCode.Unauthorized)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<TObj>(obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return default(TObj);
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }

    }
}

