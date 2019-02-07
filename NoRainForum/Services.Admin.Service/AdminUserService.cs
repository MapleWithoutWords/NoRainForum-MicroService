using Microsoft.EntityFrameworkCore;
using Services.Admin.DTO;
using Services.Admin.IService;
using Services.Admin.Model;
using Services.Admin.Model.Entities;
using Services.Common;
using Services.Common.ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Admin.Service
{
    public class AdminUserService : IAdminUserService
    {
        public async Task<long> AddNewAsync(AddAdminUserDTO dto)
        {
            AdminUserEntity entity = new AdminUserEntity();
            entity.Age = dto.Age;
            entity.Gender = dto.Gender;
            entity.Name = dto.Name;
            entity.PhoneNum = dto.PhoneNum;
            entity.Salt = MD5Helper.GetSalt(10);
            entity.PasswordHash = MD5Helper.CalcMD5(dto.Password + entity.Salt);
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var phoneAdminUser = await bs.GetAll().SingleOrDefaultAsync(e=>e.PhoneNum==dto.PhoneNum);
                if (phoneAdminUser!=null)
                {
                    throw new Exception("电弧号码已存在");
                }
                await ctx.AdminUsers.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task EditorPasswordAsync(long id, string newPassword)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var entity = await bs.GetAll().SingleAsync(e => e.Id == id);
                entity.PasswordHash = MD5Helper.CalcMD5(newPassword + entity.Salt);
                await ctx.SaveChangesAsync();
            }
        }
        private ListAdminUserDTO TODTO(AdminUserEntity entity)
        {
            ListAdminUserDTO dto = new ListAdminUserDTO();
            dto.Age = entity.Age;
            dto.CreateTime = entity.CreateTime;
            dto.Gender = entity.Gender;
            dto.Id = entity.Id;
            dto.LoginErrorTime = entity.LoginErrorTime;
            dto.Name = entity.Name;
            dto.PhoneNum = entity.PhoneNum;
            return dto;
        }
        public async Task<ListAdminUserDTO> GetByIdAsync(long id)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var entity = await bs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
                return entity == null ? null : TODTO(entity);
            }
        }

        public async Task<ListAdminUserDTO> GetByPhoneNumAsync(string phoneNum)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var entity = await bs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.PhoneNum == phoneNum);
                return entity == null ? null : TODTO(entity);
            }
        }

        public async Task<List<ListAdminUserDTO>> GetPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                List<ListAdminUserDTO> list = new List<ListAdminUserDTO>();
                await bs.GetAll().AsNoTracking().OrderByDescending(e => e.CreateTime).Skip((pageIndex - 1) * pageDataCount).Take(pageDataCount).ForEachAsync(e =>
               {
                   list.Add(TODTO(e));
               });
                return list;
            }
        }

        public async Task<bool> IsLockAsync(long id)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                List<ListAdminUserDTO> list = new List<ListAdminUserDTO>();
                var entity = await bs.GetAll().AsNoTracking().SingleAsync(e => e.Id == id);
                if (entity.LoginErrorCount <= 5)
                {
                    return false;
                }
                double min = (DateTime.Now - entity.LoginErrorTime).TotalMinutes;
                if (min >= 30)
                {
                    return false;
                }
                return true;
            }
        }

        public async Task LockAdminUserAsync(long id)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> userBs = new BaseService<AdminUserEntity>(ctx);
                var user = await userBs.GetAll().SingleAsync(e => e.Id == id);
                user.LoginErrorCount = user.LoginErrorCount + 1;
                user.LoginErrorTime = DateTime.Now;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<bool> LoginAsync(string phoneNum, string password)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> userBs = new BaseService<AdminUserEntity>(ctx);
                var user = await userBs.GetAll()
                    .AsNoTracking()
                    .SingleOrDefaultAsync(e => e.PhoneNum == phoneNum);
                if (user == null)
                {
                    return false;
                }
                if (!user.PasswordHash.Equals(MD5Helper.CalcMD5(password + user.Salt), StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
                return true;
            }
        }

        public async Task MarkDeleteAsync(long id)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> userBs = new BaseService<AdminUserEntity>(ctx);
                await userBs.MarkDeleteAsync(id);
            }
        }

        public async Task<long> TotalCountAsync()
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> userBs = new BaseService<AdminUserEntity>(ctx);
                return await userBs.TotalCountAsync();
            }
        }

        public async Task UnLockAdminUserAsync(long id)
        {
            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> userBs = new BaseService<AdminUserEntity>(ctx);
                var user = await userBs.GetAll().SingleAsync(e => e.Id == id);
                user.LoginErrorCount = 0;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(UpdateAdminUserDTO dto)
        {

            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var phoneAdminUser = await bs.GetAll().SingleOrDefaultAsync(e => e.PhoneNum == dto.PhoneNum);
                if (phoneAdminUser!=null)
                {
                    if (phoneAdminUser.Id!=dto.Id)
                    {
                        throw new Exception("电话号码已存在");
                    }
                }

                AdminUserEntity entity = await bs.GetAll().SingleOrDefaultAsync(e => e.Id == dto.Id);
                if (entity == null)
                {
                    throw new ArgumentException($"管理员{dto.Id}不存在");
                }
                entity.Age = dto.Age;
                entity.Gender = dto.Gender;
                entity.Name = dto.Name;
                entity.PhoneNum = dto.PhoneNum;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task ChangePasswordAsync(long id, string newPassword)
        {

            using (AdminUserContext ctx = new AdminUserContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                AdminUserEntity entity = await bs.GetAll().SingleOrDefaultAsync(e => e.Id == id);
                if (entity == null)
                {
                    throw new ArgumentException($"管理员{id}不存在");
                }
                entity.PasswordHash = MD5Helper.CalcMD5(newPassword+entity.Salt);
                await ctx.SaveChangesAsync();
            }
        }
    }
}
