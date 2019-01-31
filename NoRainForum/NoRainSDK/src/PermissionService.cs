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
    public class PermissionService : CommonAbstract<ListModel<ListRolePermissionDTO>>
    {
        public PermissionService(string appKey, string appSecret) : base(appKey, appSecret, "http://127.0.0.1:8888/AdminService/api/Permission/") { }
        public async Task<long> AddNewAsync(AddRolePermissionModel dTO)
        {
            return await AddNewAsync<AddRolePermissionModel>(dTO);
        }
        public async Task<bool> UpdateAsync(UpdateRolePermissionModel dTO)
        {
            return await UpdateAsync<UpdateRolePermissionModel>(dTO);
        }
        public async Task<bool> UpdateRoleToPermissesAsync(UpdateRoleOrPermissionModel model)
        {
            SDKResult result = await client.PostAsync("UpdateRoleToPermisses", model);
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

        public async Task<List<ListRolePermissionDTO>> GetByRoleIdAsync(long roleId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["roleId"] = roleId.ToString();
            SDKResult result = await client.GetAsync("GetByRoleId", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<ListRolePermissionDTO>>(obj["data"]==null?"":obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return default(List<ListRolePermissionDTO>);
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
        
        public async Task<ListRolePermissionDTO> GetByIdAsync(long id)
        {
            return await GetByIdAsync<ListRolePermissionDTO>(id);
        }

        public async Task<bool> CheckPermissionAsync(CheckPermissionModel model)
        {
            SDKResult result = await client.PostAsync("CheckPermission", model);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return false;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
    }
}
