using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoRainSDK.src
{
    public class PostTypeService : CommonAbstract<ListModel<IdNameDTO>>
    {
        public PostTypeService(string appKey, string apSecret) : base(appKey, apSecret, "http://127.0.0.1:8888/PostService/api/PostType/")
        {
        }

        public async Task<List<IdNameDTO>> GetAllAsync()
        {
            return await GetAllAsync<List<IdNameDTO>>();
        }
        public async Task<long> AddNewAsync(AddIdNameModel dto)
        {
            return await AddNewAsync<AddIdNameModel>(dto);
        }
        public async Task<bool> UpdateAsync(UpdateIdNameModel dto)
        {
            return await UpdateAsync<UpdateIdNameModel>(dto);
        }
        public async Task<IdNameDTO> GetByIdAsync(long id)
        {
            return await GetByIdAsync<IdNameDTO>(id);
        }
    }
}
