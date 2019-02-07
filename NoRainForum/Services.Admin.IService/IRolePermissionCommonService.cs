using Services.Admin.DTO;
using Services.Common.IServiceCommon;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Admin.IService
{
    public interface IRolePermissionCommonService : IBaseService<ListRolePermissionDTO>
    {
        Task<long> AddNewAsync(AddRolePermissionDTO dto);
        Task UpdateAsync(UpdateRolePermissionDTO dto);
        Task<ListRolePermissionDTO> GetByNameAsync(string name);
    }
}
