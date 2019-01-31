using Microsoft.EntityFrameworkCore;
using Services.Common.ServiceCommon;
using Services.Post.DTO;
using Services.Post.IService;
using Services.Post.Model;
using Services.Post.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Post.Service
{
    public class PostTypeService : IPostTypeService
    {
        public async Task<long> AddNewAsync(AddIdNameDTO dto)
        {
            PostTypeEntity entity = new PostTypeEntity();
            entity.Description = dto.Description;
            entity.Name = dto.Name;
            using (PostContext ctx = new PostContext())
            {
                await ctx.PostTypes.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return entity.Id;
            }
        }
        private IdNameDTO TODTO(PostTypeEntity entity)
        {
            IdNameDTO dto = new IdNameDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Description = entity.Description;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            return dto;
        }

        public async Task<IdNameDTO> GetByIdAsync(long id)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostTypeEntity> bs = new BaseService<PostTypeEntity>(ctx);
                var postType = await bs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
                return postType == null ? null : TODTO(postType);
            }
        }

        public async Task<IdNameDTO> GetByNameAsync(string name)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostTypeEntity> bs = new BaseService<PostTypeEntity>(ctx);
                var postType = await bs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Name == name);
                return postType == null ? null : TODTO(postType);
            }
        }

        public async Task<List<IdNameDTO>> GetPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostTypeEntity> bs = new BaseService<PostTypeEntity>(ctx);
                List<IdNameDTO> list = new List<IdNameDTO>();
                await bs.GetAll().AsNoTracking().OrderByDescending(e => e.CreateTime).Skip((pageIndex - 1) * pageDataCount).Take(pageDataCount).ForEachAsync(e =>
               {
                   list.Add(TODTO(e));
               });
                return list;
            }
        }

        public async Task MarkDeleteAsync(long id)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostTypeEntity> bs = new BaseService<PostTypeEntity>(ctx);
                await bs.MarkDeleteAsync(id);
            }
        }

        public async Task<long> TotalCountAsync()
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostTypeEntity> bs = new BaseService<PostTypeEntity>(ctx);
                return await bs.TotalCountAsync();
            }
        }

        public async Task UpdateAsync(UpdateIdNameDTO dto)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostTypeEntity> bs = new BaseService<PostTypeEntity>(ctx);
                var en = await bs.GetAll().SingleAsync(e => e.Id == dto.Id);
                en.Description = dto.Description;
                en.Name = dto.Name;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<List<IdNameDTO>> GetAll()
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostTypeEntity> bs = new BaseService<PostTypeEntity>(ctx);
                List<IdNameDTO> list = new List<IdNameDTO>();
                await bs.GetAll().AsNoTracking().ForEachAsync(e =>
                {
                    list.Add(TODTO(e));
                });
                return list;
            }
        }
    }
}
