using Services.Admin.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Admin.IService
{
    public interface IPermissionService:IRolePermissionCommonService
    {
        Task<List<ListRolePermissionDTO>> GetByRoleIdAsync(long roleId);
        Task UpdateRoleToPermissesAsync(long roleId, long[] PermissionIds);
        Task<bool> CheckPermissionAsync(long adminUserId, string permissionName);
        Task<List<ListRolePermissionDTO>> GetAllAsync();
    }
}
