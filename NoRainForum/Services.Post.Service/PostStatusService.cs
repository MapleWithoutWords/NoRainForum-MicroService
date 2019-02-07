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
    public class PostStatusService : IPostStatusService
    {
        public async Task<long> AddNewAsync(AddIdNameDTO dto)
        {
            PostStatusEntity entity = new PostStatusEntity();
            entity.Description = dto.Description;
            entity.Name = dto.Name;
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostStatusEntity> bs = new BaseService<PostStatusEntity>(ctx);
                var status = await bs.GetAll().SingleOrDefaultAsync(e => e.Name == dto.Name);
                if (status!=null)
                {
                    throw new Exception("帖子状态已存在");
                }
                await ctx.PostStatuses.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return entity.Id;
            }
        }
        private IdNameDTO TODTO(PostStatusEntity entity)
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
                BaseService<PostStatusEntity> bs = new BaseService<PostStatusEntity>(ctx);
                var postType = await bs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
                return postType == null ? null : TODTO(postType);
            }
        }

        public async Task<IdNameDTO> GetByNameAsync(string name)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostStatusEntity> bs = new BaseService<PostStatusEntity>(ctx);
                var postType = await bs.GetAll().AsNoTracking().SingleOrDefaultAsync(e => e.Name == name);
                return postType == null ? null : TODTO(postType);
            }
        }

        public async Task<List<IdNameDTO>> GetPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostStatusEntity> bs = new BaseService<PostStatusEntity>(ctx);
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
                BaseService<PostStatusEntity> bs = new BaseService<PostStatusEntity>(ctx);
                await bs.MarkDeleteAsync(id);
            }
        }

        public async Task<long> TotalCountAsync()
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostStatusEntity> bs = new BaseService<PostStatusEntity>(ctx);
                return await bs.TotalCountAsync();
            }
        }

        public async Task UpdateAsync(UpdateIdNameDTO dto)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostStatusEntity> bs = new BaseService<PostStatusEntity>(ctx);
                var nameEntity = await bs.GetAll().SingleOrDefaultAsync(e => e.Name == dto.Name);
                if (nameEntity!=null)
                {
                    if (nameEntity.Id!=dto.Id)
                    {
                        throw new Exception("帖子状态已存在");
                    }
                }
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
                BaseService<PostStatusEntity> bs = new BaseService<PostStatusEntity>(ctx);
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
