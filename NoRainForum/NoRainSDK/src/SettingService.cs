using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoRainSDK.src
{
   public class SettingService:CommonAbstract<ListModel<SettingDTO>>
    {
        public SettingService(string appKey,string appSecret):base(appKey,appSecret,"http://127.0.0.1:8888/OtherService/api/Setting/")
        {

        }
        public async Task<SettingDTO> GetByIdAsync(long id)
        {
            return await GetByIdAsync<SettingDTO>(id);
        }
        public async Task<bool> UpdateAsync(UpdateSettingModel model)
        {
            return await UpdateAsync<UpdateSettingModel>(model);
        }
        public async Task<long> AddNewAsync(AddSettingModel model)
        {
            return await AddNewAsync<AddSettingModel>(model);
        }
    }
}
