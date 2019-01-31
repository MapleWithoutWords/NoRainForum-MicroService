using Newtonsoft.Json;
using NoRainSDK.hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NoRainSDK.http
{
    public class SDKClient
    {
        private AppInfoSetting Setting;
        private string ServerRoot { get; set; }
        public SDKClient(AppInfoSetting setting, string serverRoot)
        {
            if (!ValidateCommon.IsValid(setting, out string errorMsg))
            {
                throw new ArgumentException(errorMsg);
            }
            Setting = setting;
            ServerRoot = serverRoot;
        }
        public async Task<SDKResult> GetAsync<T>(string url, T data) where T : class
        {
            return await RequestAsync(HttpMethod.Get, url, data);
        }
        public async Task<SDKResult> PostAsync<T>(string url, T data) where T : class
        {
            return await RequestAsync(HttpMethod.Post, url, data);
        }
        public async Task<SDKResult> DeleteAsync<T>(string url, T data) where T : class
        {
            return await RequestAsync(HttpMethod.Delete, url, data);
        }
        public async Task<SDKResult> PutAsync<T>(string url, T data) where T : class
        {
            return await RequestAsync(HttpMethod.Put, url, data);
        }

        private async Task<SDKResult> RequestAsync<T>(HttpMethod method, string url, T data) where T : class
        {


            if (string.IsNullOrEmpty(url) || data == null)
            {
                throw new ArgumentException($"url或者queryStringData不能为空");
            }

            using (HttpRequestMessage requestMessage = new HttpRequestMessage())
            {
                requestMessage.Method = method;
                string sign;
                if (method == HttpMethod.Get || method == HttpMethod.Delete)
                {
                    IDictionary<string, string> queryStringData;
                    if (data is IDictionary<string, string>)
                    {
                        queryStringData = (IDictionary<string, string>)data;
                    }
                    else
                    {
                        queryStringData = GetDict(data);
                    }
                    var queries = queryStringData.OrderBy(e => e.Key)
                                        .Select(e => $"{e.Key}={e.Value}");
                    string queryString = string.Join("&", queries);
                    Uri uri = new Uri(ServerRoot + url + "?" + queryString);
                    requestMessage.RequestUri = uri;
                    string path = new Regex("^/.+(/api/.*)$").Match(uri.PathAndQuery).Groups[1].Value;
                    sign = MD5Helper.CalcMD5(path + Setting.AppSecret);
                    requestMessage.Headers.Add("sign", sign);
                    requestMessage.Headers.Add("appKey", Setting.AppKey);
                    return await SendAsync(requestMessage);
                }
                else if (method == HttpMethod.Put || method == HttpMethod.Post)
                {
                    string json = JsonConvert.SerializeObject(data);
                    using (StringContent content = new StringContent(json))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        requestMessage.Content = content;
                        Uri uri = new Uri(ServerRoot + url);
                        requestMessage.RequestUri = uri;
                        string path = new Regex("^/.+(/api/.*)$").Match(uri.PathAndQuery).Groups[1].Value;
                        sign = MD5Helper.CalcMD5(path + Setting.AppSecret + json);
                        requestMessage.Headers.Add("sign", sign);
                        requestMessage.Headers.Add("appKey", Setting.AppKey);
                        return await SendAsync(requestMessage);
                    }
                }
                else
                {
                    throw new ApplicationException("未支持的方法");
                }
            }

        }
        private async Task<SDKResult> SendAsync(HttpRequestMessage requestMessage)
        {
            using (HttpClient client = new HttpClient())
            {
                var responseMsg = await client.SendAsync(requestMessage);
                if (responseMsg.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new ApplicationException($"Url：{requestMessage.RequestUri.ToString()}不存在");
                }
                SDKResult result = new SDKResult();
                result.StatusCode = responseMsg.StatusCode;
                result.Result = await responseMsg.Content.ReadAsStringAsync();
                return result;
            }
        }

        private IDictionary<string, string> GetDict<TObj>(TObj obj) where TObj : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException($"{obj.GetType().Name}类型不能为null");
            }
            Type objType = obj.GetType();
            IDictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var item in objType.GetProperties())
            {
                dict[item.Name] = item.GetValue(obj) == null ? "" : item.GetValue(obj).ToString();
            }
            return dict;
        }
    }
}
