using Microsoft.EntityFrameworkCore;
using Services.Common;
using Services.Common.ServiceCommon;
using Services.User.DTO;
using Services.User.IService;
using Services.User.Model;
using Services.User.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.User.Service
{
    public class UserService : IUserService
    {
        public async Task<long> AddNewAsync(AddUserDTO dto)
        {
            UserEntity en = new UserEntity();
            en.Email = dto.Email;
            en.Gender = dto.Gender;
            en.NickName = dto.NickName;
            en.Salt = MD5Helper.GetSalt(10);
            en.PasswordHash = MD5Helper.CalcMD5(dto.Password + en.Salt);
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var emailEntity = await bs.GetAll().SingleOrDefaultAsync(e=>e.Email==dto.Email||e.NickName==dto.NickName);
                if (emailEntity!=null)
                {
                    throw new Exception("用户邮箱或者昵称已存在");
                }
                await ctx.Users.AddAsync(en);
                await ctx.SaveChangesAsync();
                return en.Id;
            }
        }
        private ListUserDTO TODTO(UserEntity en)
        {
            ListUserDTO dto = new ListUserDTO();
            dto.Autograph = en.Autograph;
            dto.City = en.City;
            dto.CreateTime = en.CreateTime;
            dto.Email = en.Email;
            dto.Gender = en.Gender;
            dto.HeadImgSrc = en.HeadImgSrc;
            dto.Id = en.Id;
            dto.NickName = en.NickName;
            dto.LoginErrorTime = en.LoginErrorTime;
            dto.IsActive = en.IsActive;
            dto.LoginErrorCount = en.LoginErrorCount;
            return dto;
        }
        public async Task EditorPasswordAsync(long id, string newPassword)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                var user = await userBs.GetAll().SingleAsync(e => e.Id == id);

                user.PasswordHash = MD5Helper.CalcMD5(newPassword + user.Salt);
                await ctx.SaveChangesAsync();
            }
        }
        public async Task<ListUserDTO> GetByEmailAsync(string email)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                var user = await userBs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Email == email);
                return user == null ? null : TODTO(user);
            }
        }
        public async Task<ListUserDTO> GetByIdAsync(long id)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                var user = await userBs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
                return user == null ? null : TODTO(user);
            }
        }
        public async Task<ListUserDTO> GetByNickNameAsync(string nickName)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                var user = await userBs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.NickName == nickName);
                return user == null ? null : TODTO(user);
            }
        }
        public async Task<List<ListUserDTO>> GetPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                List<ListUserDTO> list = new List<ListUserDTO>();
                await userBs.GetAll().AsNoTracking().OrderByDescending(e => e.CreateTime)
                    .Skip((pageIndex - 1) * pageDataCount)
                    .Take(pageDataCount).ForEachAsync(e =>
                    {
                        list.Add(TODTO(e));
                    });
                return list;
            }
        }
        public async Task<bool> IsLockAsync(long id)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                var user = await userBs.GetAll()
                                .AsNoTracking()
                                .SingleAsync(e => e.Id == id);
                if (user.LoginErrorCount <= 5)
                {
                    return false;
                }
                double min = (DateTime.Now - user.LoginErrorTime).TotalMinutes;
                if (min >= 30)
                {
                    return false;
                }
                return true;
            }
        }
        public async Task LockUserAsync(long id)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                var user = await userBs.GetAll().SingleOrDefaultAsync(e => e.Id == id);
                if (user == null)
                {
                    throw new ApplicationException($"id={id}不存在");
                }
                user.LoginErrorCount = user.LoginErrorCount + 1;
                user.LoginErrorTime = DateTime.Now;
                await ctx.SaveChangesAsync();
            }
        }
        public async Task<bool> LoginAsync(string email, string password)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                var user = await userBs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Email == email);
                if (user == null)
                {
                    return false;
                }
                if (user.PasswordHash != MD5Helper.CalcMD5(password + user.Salt))
                {
                    return false;
                }
                return true;
            }
        }
        public async Task MarkDeleteAsync(long id)
        {
            using (UserContext ctx = new UserContext())
            {

                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                await userBs.MarkDeleteAsync(id);
            }
        }
        public async Task<long> TotalCountAsync()
        {
            using (UserContext ctx = new UserContext())
            {

                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                return await userBs.GetAll().LongCountAsync();
            }
        }
        public async Task UnLockUserAsync(long id)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                var user = await userBs.GetAll().SingleOrDefaultAsync(e => e.Id == id);
                if (user == null)
                {
                    throw new ArgumentException($"找不到id={id}的数据");
                }
                user.LoginErrorCount = 0;
                await ctx.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(UpdateUserDTO dto)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                var emailEntity = await userBs.GetAll().SingleOrDefaultAsync(e => e.Email == dto.Email || e.NickName == dto.NickName);
                if (emailEntity != null)
                {
                    if (emailEntity.Id!=dto.Id)
                    {
                        throw new Exception("用户邮箱或者昵称已存在");
                    }
                }
                var user = await userBs.GetAll().SingleOrDefaultAsync(e => e.Id == dto.Id);
                if (user == null)
                {
                    return;
                }
                user.Autograph = dto.Autograph;
                user.City = dto.City;
                user.Email = dto.Email;
                user.Gender = dto.Gender;
                if (user.Email != dto.Email)
                {
                    user.IsActive = false;
                }
                user.NickName = dto.NickName;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<List<ListUserDTO>> GetByIdsAsync(List<long> ids)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
                List<ListUserDTO> list = new List<ListUserDTO>();
                foreach (var item in ids)
                {
                    var user = await userBs.GetAll().AsNoTracking().SingleAsync(e => e.Id == item);
                    list.Add(TODTO(user));
                }

                return list;
            }
        }

        public async Task ActiveEmailAsync(long id)
        {
            using (UserContext ctx = new UserContext())
            {
                BaseService<UserEntity> userBs = new BaseService<UserEntity>(ctx);
               
                var user =await userBs.GetAll().SingleAsync(e=>e.Id==id);
                user.IsActive = true;
               await ctx.SaveChangesAsync();
            }
        }
    }
}
