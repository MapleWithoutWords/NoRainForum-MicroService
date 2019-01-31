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
    public class RoleService : CommonAbstract<ListModel<ListRolePermissionDTO>>
    {
        public RoleService(string appKey, string appSecret) : base(appKey, appSecret, "http://127.0.0.1:8888/AdminService/api/Role/")
        {
        }
        public async Task<long> AddNewAsync(AddRolePermissionModel dto)
        {
            return await base.AddNewAsync<AddRolePermissionModel>(dto);
        }
        public async Task<List<ListRolePermissionDTO>> GetAllAsync()
        {
            return await GetAllAsync<List<ListRolePermissionDTO>>();
        }
        public async Task<bool> UpdateAsync(UpdateRolePermissionModel dto)
        {
            return await UpdateAsync<UpdateRolePermissionModel>(dto);
        }
        public async Task<ListRolePermissionDTO> GetByIdAsync(long id)
        {
            return await GetByIdAsync<ListRolePermissionDTO>(id);
        }
        public async Task<bool> UpdateAdminUserRolesAsynv(UpdateRoleOrPermissionModel model)
        {
            SDKResult result = await client.PostAsync("UpdateAdminUserRoles", model);
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
        public async Task<List<ListRolePermissionDTO>> GetByAdminUserId(long adminUserId)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["adminUserId"] = adminUserId.ToString();
            SDKResult result = await client.GetAsync("GetByAdminUserId", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<ListRolePermissionDTO>>(obj["data"] == null ? "" : obj["data"].ToString());
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
        
    }
}
