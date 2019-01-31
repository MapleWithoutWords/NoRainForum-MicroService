using Services.Common.IServiceCommon;
using Services.Other.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Other.IService
{
    public interface IAppInfoService:IBaseService<AppInfoDTO>
    {
        Task<long> AddNewAsync(AddAppInfoDTO dot);
        Task<AppInfoDTO> GetByAppKeyAsync(string appKey);
    }
}
