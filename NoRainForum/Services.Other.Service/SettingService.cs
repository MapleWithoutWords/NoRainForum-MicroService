using Microsoft.EntityFrameworkCore;
using Services.Common.ServiceCommon;
using Services.Other.DTO;
using Services.Other.IService;
using Services.Other.Model;
using Services.Other.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Other.Service
{
    public class SettingService : ISettingService
    {
        public async Task<long> AddNewAsync(string keyPari, string key, string value)
        {
            SettingEntity entity = new SettingEntity();
            entity.Key = key;
            entity.KeyPari = keyPari;
            entity.Value = value;
            using (OtherContext ctx = new OtherContext())
            {
                await ctx.Settings.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return entity.Id;
            }

        }
        private SettingDTO TODTO(SettingEntity entity)
        {
            SettingDTO dto = new SettingDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.Key = entity.Key;
            dto.KeyPari = entity.KeyPari;
            dto.Value = entity.Value;
            return dto;
        }
        public async Task<SettingDTO> GetByIdAsync(long id)
        {
            using (OtherContext ctx = new OtherContext())
            {
                BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
                var setting = await bs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
                return setting == null ? null : TODTO(setting);
            }
        }

        public async Task<SettingDTO> GetByKeyAsync(string keyPari)
        {
            using (OtherContext ctx = new OtherContext())
            {
                BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
                var setting = await bs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.KeyPari == keyPari);
                return setting == null ? null : TODTO(setting);
            }
        }

        public async Task<List<SettingDTO>> GetPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            using (OtherContext ctx = new OtherContext())
            {
                BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
                List<SettingDTO> list = new List<SettingDTO>();
                await bs.GetAll().AsNoTracking()
                    .OrderByDescending(e => e.CreateTime)
                    .Skip((pageIndex - 1) * pageDataCount)
                    .Take(pageDataCount)
                    .ForEachAsync(e =>
                    {
                        list.Add(TODTO(e));
                    });
                return list;
            }
        }

        public async Task MarkDeleteAsync(long id)
        {
            using (OtherContext ctx = new OtherContext())
            {
                BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
                await bs.MarkDeleteAsync(id);
            }
        }

        public async Task<long> TotalCountAsync()
        {
            using (OtherContext ctx = new OtherContext())
            {
                BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
                return await bs.TotalCountAsync();
            }
        }

        public async Task UpdateAsync(long id, string keyPari, string key, string value)
        {
            using (OtherContext ctx = new OtherContext())
            {
                BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
                var en =await bs.GetAll().SingleAsync(e=>e.Id==id);
                en.KeyPari = keyPari;
                en.Key = key;
                en.Value = value;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
