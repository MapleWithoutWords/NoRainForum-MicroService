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
    public class PostCommentService : IPostCommontService
    {
        public async Task<long> AddNewAsync(AddPostCommentDTO dto)
        {
            PostCommentEntity entity = new PostCommentEntity();
            entity.CommonUserId = dto.CommentUserId;
            entity.Content = dto.Content;
            entity.PostId = dto.PostId;
            entity.ReplyUserId = dto.ReplyUserId;
            using (PostContext ctx = new PostContext())
            {
                await ctx.PostCommonts.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return entity.Id;
            }
        }
        private ListPostCommentDTO TODTO(PostCommentEntity entity)
        {
            ListPostCommentDTO dto = new ListPostCommentDTO();
            dto.CommentUserId = entity.CommonUserId;
            dto.Content = entity.Content;
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.IsUse = entity.IsUse;
            dto.PostId = entity.PostId;
            dto.PostTitle = entity.Post.Title;
            dto.ReplyUserId = entity.ReplyUserId;
            dto.PostUserId = entity.Post.UserId;
            dto.IsKnot = entity.Post.IsKnot;
            return dto;
        }
        public async Task<ListPostCommentDTO> GetByIdAsync(long id)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostCommentEntity> bs = new BaseService<PostCommentEntity>(ctx);
                var en = await bs.GetAll().AsNoTracking().Include(e => e.Post).SingleOrDefaultAsync(e => e.Id == id);
                return en == null ? null : TODTO(en);
            }
        }

        public async Task<List<ListPostCommentDTO>> GetByPageReplyUserIdAsync(long replyPostUserId, int pageIndex, int pageDataCount)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostCommentEntity> bs = new BaseService<PostCommentEntity>(ctx);
                List<ListPostCommentDTO> list = new List<ListPostCommentDTO>();
                await bs.GetAll()
                    .AsNoTracking()
                    .Include(e => e.Post)
                    .Where(e => e.ReplyUserId == replyPostUserId)
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

        public async Task<List<ListPostCommentDTO>> GetByPageSendUserIdAsync(long commentUserId)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostCommentEntity> bs = new BaseService<PostCommentEntity>(ctx);
                List<ListPostCommentDTO> list = new List<ListPostCommentDTO>();
                await bs.GetAll()
                    .AsNoTracking()
                    .Include(e => e.Post)
                    .Where(e => e.CommonUserId == commentUserId&&e.CreateTime>DateTime.Now.AddDays(-2))
                    .OrderByDescending(e => e.CreateTime)
                    .ForEachAsync(e =>
                    {
                        list.Add(TODTO(e));
                    });
                return list;
            }
        }

        public async Task<List<long>> GetByPostIdCountAsync(IEnumerable<long> postIds)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostCommentEntity> bs = new BaseService<PostCommentEntity>(ctx);
                List<long> list = await bs.GetAll()
                    .GroupBy(e => e.PostId)
                    .Where(e => postIds.Contains(e.Key))
                    .Select(e => e.LongCount())
                    .ToListAsync();
                return list;
            }
        }

        public async Task<List<ListPostCommentDTO>> GetPageByPostIdAsync(long postId, int pageIndex, int pageDataCount)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostCommentEntity> bs = new BaseService<PostCommentEntity>(ctx);
                List<ListPostCommentDTO> list = new List<ListPostCommentDTO>();
                await bs.GetAll()
                .AsNoTracking()
                .Include(e => e.Post)
                .Where(e => e.PostId == postId)
                .OrderByDescending(e=>e.IsUse)
                .OrderByDescending(e => e.CreateTime)
                .Skip((pageIndex - 1) * pageDataCount).Take(pageDataCount)
                .ForEachAsync(e =>
                {
                    list.Add(TODTO(e));
                });
                return list;
            }
        }

        public Task<List<ListPostCommentDTO>> GetPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            throw new NotImplementedException();
        }

        public async Task MarkDeleteAsync(long id)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostCommentEntity> bs = new BaseService<PostCommentEntity>(ctx);
                await bs.MarkDeleteAsync(id);
            }
        }

        public Task<long> TotalCountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<long> GetTotalCountByPostIdAsync(long postId)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostCommentEntity> bs = new BaseService<PostCommentEntity>(ctx);
                return await bs.GetAll().LongCountAsync(e => e.PostId == postId);
            }
        }
    }
}
