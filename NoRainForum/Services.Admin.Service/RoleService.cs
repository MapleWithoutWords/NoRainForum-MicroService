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
    public class RoleService : IRoleService
    {
        public async Task<long> AddNewAsync(AddRolePermissionDTO dto)
        {
            RoleEntity entity = new RoleEntity();
            entity.Description = dto.Description;
            entity.Name = dto.Name;
            using (AdminUserContext ctx = new AdminUserContext())
            {
                await ctx.Roles.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return entity.Id;
            }
        }
        private ListRolePermissionDTO TODTO(RoleEntity entity)
        {
            ListRolePermissionDTO dto = new ListRolePermissionDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Description = entity.Description;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            return dto;
        }
        public async Task<List<ListRolePermissionDTO>> GetAllAsync()
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                List<ListRolePermissionDTO> list = new List<ListRolePermissionDTO>();
                await roleBs.GetAll().AsNoTracking().ForEachAsync(e =>
                {
                    list.Add(TODTO(e));
                });
                return list;
            }
        }

        public async Task<List<ListRolePermissionDTO>> GetByAdminUserIdAsync(long AdminuserId)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserRoleEntity> adminuserRoleBs = new BaseService<AdminUserRoleEntity>(ctx);
                List<ListRolePermissionDTO> list = new List<ListRolePermissionDTO>();
                await adminuserRoleBs.GetAll()
                    .Include(e => e.Role)
                    .AsNoTracking()
                    .ForEachAsync(e =>
                {
                    if (e.AdminUserId == AdminuserId)
                    {
                        list.Add(TODTO(e.Role));
                    }
                });
                return list;
            }
        }

        public async Task<ListRolePermissionDTO> GetByIdAsync(long id)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                var role = await roleBs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
                return role == null ? null : TODTO(role);
            }
        }

        public async Task<ListRolePermissionDTO> GetByNameAsync(string name)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                var role = await roleBs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Name == name);
                return role == null ? null : TODTO(role);
            }
        }

        public async Task<List<ListRolePermissionDTO>> GetPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
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
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                await roleBs.MarkDeleteAsync(id);
            }
        }

        public async Task<long> TotalCountAsync()
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                return await roleBs.TotalCountAsync();
            }
        }

        public async Task UpdateAdminUserToRolesAsync(long adminUserId, long[] roleIds)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> adminuserBs = new BaseService<AdminUserEntity>(ctx);
                var adminuser =await adminuserBs.GetAll().SingleAsync(e=>e.Id==adminUserId);

                BaseService<AdminUserRoleEntity> adminuserRoleBs = new BaseService<AdminUserRoleEntity>(ctx);
                await adminuserRoleBs.GetAll().ForEachAsync(e =>
                {
                    if (e.AdminUserId == adminUserId)
                    {
                        ctx.AdminUserRoles.Remove(e);
                    }
                });

                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                await roleBs.GetAll().ForEachAsync( e =>
                {
                    if (roleIds.Any(x => x == e.Id))
                    {
                        AdminUserRoleEntity en = new AdminUserRoleEntity();
                        en.Role = e;
                        en.AdminUser = adminuser;
                        ctx.AdminUserRoles.Add(en);
                    }
                });
               await ctx.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(UpdateRolePermissionDTO dto)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<RoleEntity> roleBs = new BaseService<RoleEntity>(ctx);
                var entity = await roleBs.GetAll().SingleAsync(e => e.Id == dto.Id);
                entity.Description = dto.Description;
                entity.Name = dto.Name;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
