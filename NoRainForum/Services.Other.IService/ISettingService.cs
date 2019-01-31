using Services.Common.IServiceCommon;
using Services.Other.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Other.IService
{
    public interface ISettingService : IBaseService<SettingDTO>
    {
        Task<SettingDTO> GetByKeyAsync(string keyPari);
        Task<long> AddNewAsync(string keyPari, string key, string value);

        Task UpdateAsync(long id, string keyPari, string key, string value);
    }
}
