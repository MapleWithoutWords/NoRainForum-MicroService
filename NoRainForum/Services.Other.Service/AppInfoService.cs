using Microsoft.EntityFrameworkCore;
using Services.Common.ServiceCommon;
using Services.Other.DTO;
using Services.Other.IService;
using Services.Other.Model;
using Services.Other.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Other.Service
{
    public class AppInfoService : IAppInfoService
    {
        public async Task<long> AddNewAsync(AddAppInfoDTO dot)
        {
            AppInfoEntity entity = new AppInfoEntity();
            entity.AppKey = dot.AppKey;
            entity.AppSecret = dot.AppSecret;
            entity.Email = dot.Email;
            using (OtherContext ctx = new OtherContext())
            {
                BaseService<AppInfoEntity> bs = new BaseService<AppInfoEntity>(ctx);
                var appinfo = await bs.GetAll().SingleOrDefaultAsync(e=>e.Email==dot.Email);
                if (appinfo!=null)
                {
                    throw new Exception("邮箱已存在");
                }
                await ctx.AppInfos.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return entity.Id;
            }
        }
        private AppInfoDTO TODTO(AppInfoEntity entity)
        {
            AppInfoDTO dto = new AppInfoDTO();
            dto.Id = entity.Id;
            dto.AppKey = entity.AppKey;
            dto.AppSecret = entity.AppSecret;
            return dto;
        }
        public async Task<AppInfoDTO> GetByAppKeyAsync(string appKey)
        {
            using (OtherContext ctx = new OtherContext())
            {
                BaseService<AppInfoEntity> bs = new BaseService<AppInfoEntity>(ctx);
                var res =await bs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.AppKey == appKey);
                return res == null ? null : TODTO(res);
            }
        }

        public  Task<AppInfoDTO> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public  Task<List<AppInfoDTO>> GetPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            throw new NotImplementedException();
        }

        public  Task MarkDeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public  Task<long> TotalCountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
