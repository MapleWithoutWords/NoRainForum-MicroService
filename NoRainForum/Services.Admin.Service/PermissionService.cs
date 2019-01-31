using Microsoft.EntityFrameworkCore;
using Services.Admin.DTO;
using Services.Admin.IService;
using Services.Admin.Model;
using Services.Admin.Model.Entities;
using Services.Common.ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Admin.Service
{
    public class PermissionService : IPermissionService
    {
        public async Task<long> AddNewAsync(AddRolePermissionDTO dto)
        {
            PermissionEntity entity = new PermissionEntity();
            entity.Description = dto.Description;
            entity.Name = dto.Name;
            using (AdminUserContext ctx = new AdminUserContext())
            {
                await ctx.Permissions.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return entity.Id;
            }
        }
        private ListRolePermissionDTO TODTO(PermissionEntity entity)
        {
            ListRolePermissionDTO dto = new ListRolePermissionDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Description = entity.Description;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            return dto;
        }

        public async Task<bool> CheckPermissionAsync(long adminUserId, string permissionName)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<RolePermissionEntity> RPBs = new BaseService<RolePermissionEntity>(ctx);
                BaseService<AdminUserRoleEntity> AdminRoleBs = new BaseService<AdminUserRoleEntity>(ctx);
                List<long> roleIds = new List<long>();
                await AdminRoleBs.GetAll().ForEachAsync(e =>
                 {
                     if (e.AdminUserId == adminUserId)
                     {
                         roleIds.Add(e.RoleId);
                     }
                 });
                bool res = false;
                await RPBs.GetAll().ForEachAsync(e =>
                {
                    if (roleIds.Any(x => x == e.RoleId))
                    {
                        res = true;
                        return;
                    }
                });
                return res;
            }
        }

        public async Task<List<ListRolePermissionDTO>> GetAllAsync()
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<PermissionEntity> roleBs = new BaseService<PermissionEntity>(ctx);
                List<ListRolePermissionDTO> list = new List<ListRolePermissionDTO>();
                await roleBs.GetAll().AsNoTracking().ForEachAsync(e =>
                {
                    list.Add(TODTO(e));
                });
                return list;
            }
        }

        public async Task<ListRolePermissionDTO> GetByIdAsync(long id)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<PermissionEntity> roleBs = new BaseService<PermissionEntity>(ctx);
                var role = await roleBs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
                return role == null ? null : TODTO(role);
            }
        }

        public async Task<ListRolePermissionDTO> GetByNameAsync(string name)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<PermissionEntity> roleBs = new BaseService<PermissionEntity>(ctx);
                var role = await roleBs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Name == name);
                return role == null ? null : TODTO(role);
            }
        }

        public async Task<List<ListRolePermissionDTO>> GetByRoleIdAsync(long roleId)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<RolePermissionEntity> rolePermissionBs = new BaseService<RolePermissionEntity>(ctx);
                List<ListRolePermissionDTO> list = new List<ListRolePermissionDTO>();
                await rolePermissionBs.GetAll()
                    .Include(e => e.Permission)
                    .AsNoTracking()
                    .ForEachAsync(e =>
                    {
                        if (e.RoleId == roleId)
                        {
                            list.Add(TODTO(e.Permission));
                        }
                    });
                return list;
            }
        }

        public async Task<List<ListRolePermissionDTO>> GetPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<PermissionEntity> roleBs = new BaseService<PermissionEntity>(ctx);
                List<ListRolePermissionDTO> list = new List<ListRolePermissionDTO>();
                await roleBs.GetAll()
                    .OrderByDescending(e => e.CreateTime)
                    .Skip((pageIndex - 1) * pageDataCount)
                    .Take(pageDataCount).AsNoTracking().ForEachAsync(e =>
                    {
                        list.Add(TODTO(e));
                    });
                return list;
            }
        }

        public async Task MarkDeleteAsync(long id)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<PermissionEntity> roleBs = new BaseService<PermissionEntity>(ctx);
                await roleBs.MarkDeleteAsync(id);
            }
        }

        public async Task<long> TotalCountAsync()
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<PermissionEntity> roleBs = new BaseService<PermissionEntity>(ctx);
                return await roleBs.TotalCountAsync();
            }
        }

        public async Task UpdateAsync(UpdateRolePermissionDTO dto)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<PermissionEntity> roleBs = new BaseService<PermissionEntity>(ctx);
                var entity = await roleBs.GetAll().SingleAsync(e => e.Id == dto.Id);
                entity.Description = dto.Description;
                entity.Name = dto.Name;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task UpdateRoleToPermissesAsync(long roleId, long[] PermissionIds)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                var role = await roleBs.GetAll().SingleAsync(e => e.Id == roleId);

                BaseService<RolePermissionEntity> rolePermissionBs = new BaseService<RolePermissionEntity>(ctx);
                await rolePermissionBs.GetAll().ForEachAsync(e =>
                {
                    if (e.RoleId == roleId)
                    {
                        ctx.RolePermission.Remove(e);
                    }
                });
                BaseService<PermissionEntity> permissionBs = new BaseService<PermissionEntity>(ctx);
                await permissionBs.GetAll().ForEachAsync(e =>
                {
                    if (PermissionIds.Any(x => x == e.Id))
                    {
                        RolePermissionEntity en = new RolePermissionEntity();
                        en.Role = role;
                        en.Permission = e;
                        ctx.RolePermission.Add(en);
                    }
                });
                await ctx.SaveChangesAsync();
            }
        }
    }
}
