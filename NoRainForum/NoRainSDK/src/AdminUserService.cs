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
    public class AdminUserService : CommonAbstract<ListModel<ListAdminUserDTO>>
    {
        public AdminUserService(string appKey, string appSecret) : base(appKey, appSecret, "http://127.0.0.1:8888/AdminService/api/AdminUser/")
        {

        }
        public async Task<long> AddNewAsync(AddAdminUserModel dto)
        {
            return await AddNewAsync<AddAdminUserModel>(dto);
        }
        public async Task<bool> UpdateAsync(UpdateAdminUserModel dto)
        {
            return await UpdateAsync<UpdateAdminUserModel>(dto);
        }
        public async Task<ListAdminUserDTO> GetByIdAsync(long id)
        {
            return await GetByIdAsync<ListAdminUserDTO>(id);
        }
        public async Task<ListAdminUserDTO> GetByPhoneNum(string phoneNum)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["phoneNum"] = phoneNum;
            SDKResult result = await client.GetAsync("GetByPhoneNum", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return obj["data"] == null ? null : JsonConvert.DeserializeObject<ListAdminUserDTO>(obj["data"].ToString());
            }
            else
            {
                throw new ApplicationException(" 错误");
            }
        }
      
        public async Task<ListAdminUserDTO> LoginAsync(AdminUserLoginModel model)
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
                return JsonConvert.DeserializeObject<ListAdminUserDTO>(obj["data"]==null?"": obj["data"].ToString());
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
        public async Task<bool> ChangePasswordAsync(ChangePasswordModel model)
        {

            SDKResult result = await client.PostAsync("ChangePassword", model);
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
