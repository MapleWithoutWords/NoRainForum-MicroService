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

    }
}
