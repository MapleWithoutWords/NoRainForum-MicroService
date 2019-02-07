using Services.Admin.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Admin.IService
{
    public interface IRoleService : IRolePermissionCommonService
    {
        Task<List<ListRolePermissionDTO>> GetByAdminUserIdAsync(long AdminuserId);
        Task UpdateAdminUserToRolesAsync(long adminUserId, long[] roleIds);
        Task<List<ListRolePermissionDTO>> GetAllAsync();
    }
}
