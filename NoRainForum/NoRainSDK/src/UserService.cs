using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NoRainSDK.hash;
using NoRainSDK.http;
using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NoRainSDK.src
{
    public class UserService : CommonAbstract<ListModel<ListUserDTO>>
    {
        public UserService(string appKey, string appSecret) : base(appKey, appSecret, "http://127.0.0.1:8888/UserService/api/user/")
        {

        }
        public async Task<long> AddNewAsync(AddUserModel dto)
        {
            return await AddNewAsync<AddUserModel>(dto);
        }
        public async Task<bool> UpdateAsync(UpdateUserModel dto)
        {
            return await UpdateAsync<UpdateUserModel>(dto);
        }
        public async Task<ListUserDTO> GetByIdAsync(long id)
        {
            return await GetByIdAsync<ListUserDTO>(id);
        }

        public async Task<List<ListUserDTO>> GetByIdsAsync(List<long> userIds)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < userIds.Count; i++)
            {
                if (i == userIds.Count - 1)
                {
                    sb.Append($"ids={userIds.ElementAt(i)}");
                }
                else
                {
                    sb.Append($"ids={userIds.ElementAt(i)}&");
                }

            }
            string queryString = sb.ToString();
            using (HttpRequestMessage requestMessage = new HttpRequestMessage())
            {
                requestMessage.Method = HttpMethod.Get;
                Uri uri = new Uri(client.ServerRoot + "GetByIds?" + queryString);
                requestMessage.RequestUri = uri;
                string path = new Regex("^/.+(/api/.*)$").Match(uri.PathAndQuery).Groups[1].Value;
                string sign = MD5Helper.CalcMD5(path + client.Setting.AppSecret);
                requestMessage.Headers.Add("sign", sign);
                requestMessage.Headers.Add("appKey", client.Setting.AppKey);
                SDKResult result = await client.SendAsync(requestMessage);
                JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ErrorMsg = obj["errorMsg"].ToString();
                    return null;
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<List<ListUserDTO>>(obj["data"] == null ? "" : obj["data"].ToString());
                }
                else
                {
                    throw new ApplicationException("未知的错误");
                }
            }
        }

        public async Task<ListUserDTO> LoginAsync(UserLoginModel model)
        {

            SDKResult result = await client.PostAsync("Login", model);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ListUserDTO>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
        public async Task<bool> EditPasswordAsync(RePasswordModel model)
        {

            SDKResult result = await client.PostAsync("EditPassword", model);
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
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }


        public async Task<ListUserDTO> GetByNameAsync(string nickName)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs["nickName"] = nickName;
            SDKResult result = await client.GetAsync("GetByName", pairs);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ListUserDTO>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }

        }
        public async Task<bool> ActiveEmailAsync(long id)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs["id"] = id.ToString();
            SDKResult result = await client.GetAsync("ActiveEmail", pairs);
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
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }

        }
    }
}
