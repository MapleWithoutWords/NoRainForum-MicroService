using Microsoft.EntityFrameworkCore;
using Services.Common.IServiceCommon;
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
    public class PostService : IPostService
    {
        public async Task<long> AddNewAsync(AddPostDTO dto)
        {
            PostEntity entity = new PostEntity();
            entity.Content = dto.Content;
            entity.PostStatusId = dto.PostStatusId;
            entity.PostTypeId = dto.PostTypeId;
            entity.Title = dto.Title;
            entity.UserId = dto.UserId;
            using (PostContext ctx = new PostContext())
            {
                await ctx.Posts.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return entity.Id;
            }
        }
        private ListPostDTO TODTO(PostEntity entity, long commentCount)
        {
            ListPostDTO dto = new ListPostDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.PostStatusId = entity.PostStatusId;
            dto.PostStatusName = entity.PostStatus.Name;
            dto.PostTypeId = entity.PostTypeId;
            dto.PostTypeName = entity.PostType.Name;
            dto.Title = entity.Title;
            dto.UserId = entity.UserId;
            dto.IsEssence = entity.IsEssence;
            dto.IsKnot = entity.IsKnot;
            dto.CommentCount = commentCount;
            return dto;
        }
        /// <summary>
        /// 获取评论数量最多的十条帖子
        /// </summary>
        /// <returns></returns>
        public async Task<List<ListPostDTO>> GetByCommentCountAsync()
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> postBs = new BaseService<PostEntity>(ctx);
                BaseService<PostCommentEntity> commentBs = new BaseService<PostCommentEntity>(ctx);
                Dictionary<long, long> postIds = new Dictionary<long, long>();
                await commentBs.GetAll()
                       .AsNoTracking()
                       .Include(e => e.Post)
                       .Include(e => e.Post.PostStatus)
                      .Where(e => e.Post.PostStatus.Name != "审核中" && e.CreateTime > DateTime.Now.AddMonths(-1))
                      .GroupBy(e => e.PostId)
                      .OrderByDescending(e => e.LongCount())
                      .Take(10)
                      .ForEachAsync(e =>
                      {
                          postIds[e.Key] = e.LongCount();
                      });
                List<ListPostDTO> list = new List<ListPostDTO>();
                await postBs.GetAll()
                      .AsNoTracking()
                      .Include(e => e.PostType)
                      .Include(e => e.PostStatus)
                      .OrderBy(e => e.CreateTime)
                      .ForEachAsync(e =>
                      {
                          if (postIds.Keys.Any(x => x == e.Id))
                          {
                              list.Add(TODTO(e, postIds[e.Id]));
                          }
                      });
                return list;
            }
        }
        /// <summary>
        /// 获取一周内评论数最多的帖子
        /// </summary>
        /// <returns></returns>
        public async Task<List<ListPostDTO>> GetByDayCommentAsync()
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> postBs = new BaseService<PostEntity>(ctx);
                BaseService<PostCommentEntity> commentBs = new BaseService<PostCommentEntity>(ctx);
                Dictionary<long, long> postIds = new Dictionary<long, long>();
                await commentBs.GetAll()
                       .AsNoTracking()
                       .Include(e => e.Post)
                       .Include(e => e.Post.PostStatus)
                      .Where(e => e.Post.PostStatus.Name != "审核中")
                      .GroupBy(e => e.PostId)
                      .OrderByDescending(e => e.LongCount())
                      .Take(10)
                      .ForEachAsync(e =>
                      {
                          postIds[e.Key] = e.LongCount();
                      });
                List<ListPostDTO> list = new List<ListPostDTO>();
                await postBs.GetAll()
                      .AsNoTracking()
                      .Include(e => e.PostType)
                      .Include(e => e.PostStatus)
                      .OrderBy(e => e.CreateTime)
                      .ForEachAsync(e =>
                      {
                          if (e.CreateTime > DateTime.Now.AddDays(-7) && postIds.Keys.Any(p => p == e.Id))
                          {
                              list.Add(TODTO(e, postIds[e.Id]));
                          }
                      });
                return list;
            }
        }

        public async Task<ListContentPostDTO> GetByIdAsync(long id)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                var res = await bs.GetAll()
                       .AsNoTracking()
                      .Include(e => e.PostType)
                      .Include(e => e.PostStatus)
                      .SingleOrDefaultAsync(e => e.Id == id );
                BaseService<PostCommentEntity> commentBs = new BaseService<PostCommentEntity>(ctx);
                if (res == null)
                {
                    return null;
                }

                return new ListContentPostDTO
                {
                    Content = res.Content,
                    CreateTime = res.CreateTime,
                    IsEssence = res.IsEssence,
                    Id = res.Id,
                    IsKnot = res.IsKnot,
                    PostStatusId = res.PostStatusId,
                    PostStatusName = res.PostStatus.Name,
                    PostTypeId = res.PostTypeId,
                    PostTypeName = res.PostType.Name,
                    Title = res.Title,
                    UserId = res.UserId,
                    CommentCount = await commentBs.GetAll().LongCountAsync(e => e.PostId == id)
                };
            }
        }

        /// <summary>
        /// 获取该类型的帖子总数
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<long> GetByPostTypeIdCountAsync(long typeId, bool? isKnot = null, bool? isEssence = null)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                var posts = bs.GetAll();

                if (isKnot != null)
                {
                    posts = posts.Where(e => (e.IsKnot == isKnot));

                }
                else if (isEssence != null)
                {
                    posts = posts.Where(e => (e.IsEssence == isEssence));
                }
                return await posts
                      .AsNoTracking().LongCountAsync(e => e.PostTypeId == typeId && e.PostStatus.Name != "审核中");
            }
        }
        /// <summary>
        /// 获取该类型的帖子
        /// </summary>
        /// <param name="postTypeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        public async Task<List<ListPostDTO>> GetPageByTypeIdAsync(long postTypeId, int pageIndex = 1, int pageDataCount = 10, bool? isKnot = null, bool? isEssence = null)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> postBs = new BaseService<PostEntity>(ctx);
                List<ListPostDTO> list = new List<ListPostDTO>();
                BaseService<PostCommentEntity> commentBs = new BaseService<PostCommentEntity>(ctx);
                var posts = postBs.GetAll();

                if (isKnot != null)
                {
                    posts = posts.Where(e => (e.IsKnot == isKnot));

                }
                else if (isEssence != null)
                {
                    posts = posts.Where(e => (e.IsEssence == isEssence));
                }

                await posts.Where(e => e.PostTypeId == postTypeId && e.PostStatus.Name != "审核中")
                      .AsNoTracking()
                      .Include(e => e.PostType)
                      .Include(e => e.PostStatus)
                      .OrderBy(e => e.CreateTime)
                      .Skip((pageIndex - 1) * pageDataCount)
                      .Take(pageDataCount)
                      .ForEachAsync(e =>
                      {
                          list.Add(TODTO(e, commentBs.GetAll().LongCount(x => x.PostId == e.Id)));
                      });
                return list;
            }
        }

        /// <summary>
        /// 获取该用户发的帖子
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        public async Task<List<ListPostDTO>> GetByUserIdAsync(long userId, int pageIndex = 1, int pageDataCount = 10)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> postBs = new BaseService<PostEntity>(ctx);
                BaseService<PostCommentEntity> commentBs = new BaseService<PostCommentEntity>(ctx);
                List<ListPostDTO> list = new List<ListPostDTO>();
                await postBs.GetAll()
                      .Where(e => e.UserId == userId)
                      .AsNoTracking()
                      .Include(e => e.PostType)
                      .Include(e => e.PostStatus)
                      .OrderBy(e => e.CreateTime)
                      .Skip((pageIndex - 1) * pageDataCount)
                      .Take(pageDataCount)
                      .ForEachAsync(e =>
                      {
                          list.Add(TODTO(e, commentBs.GetAll().LongCount(x => x.PostId == e.Id)));
                      });
                return list;
            }
        }
        /// <summary>
        /// 获取用户发的帖子总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<long> GetByUserIdTotalCountAsync(long userId)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                return await bs.GetAll().LongCountAsync(e => e.UserId == userId);
            }
        }

        /// <summary>
        /// 获取该用户收藏的帖子总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<long> GetCollectionPostByUserIdTotalCountAsync(long userId)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> postBs = new BaseService<PostEntity>(ctx);
                return await postBs.GetAll().LongCountAsync(e => e.UserId == userId);
            }
        }
        /// <summary>
        /// 获取用户收藏的帖子
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        public async Task<List<ListPostDTO>> GetPageCollectionPostByUserIdAsync(long userId, int pageIndex = 1, int pageDataCount = 10)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostCollectionEntity> bs = new BaseService<PostCollectionEntity>(ctx);
                BaseService<PostCommentEntity> commentBs = new BaseService<PostCommentEntity>(ctx);
                List<ListPostDTO> list = new List<ListPostDTO>();
                await bs.GetAll()
                      .Where(e => e.UserId == userId)
                      .AsNoTracking()
                      .Include(e => e.Post)
                      .Include(e => e.Post.PostStatus)
                      .Include(e => e.Post.PostType)
                      .OrderBy(e => e.CreateTime)
                      .Skip((pageIndex - 1) * pageDataCount)
                      .Take(pageDataCount)
                      .ForEachAsync(e =>
                      {
                          list.Add(TODTO(e.Post, commentBs.GetAll().LongCount(x => x.PostId == e.PostId)));
                      });
                return list;
            }
        }

        public async Task<List<ListPostDTO>> GetPageDataAsync(int pageIndex = 1, int pageDataCount = 10, bool? isKnot = null, bool? isEssence = null)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                List<ListPostDTO> list = new List<ListPostDTO>();
                BaseService<PostCommentEntity> commentBs = new BaseService<PostCommentEntity>(ctx);

                var posts = bs.GetAll();

                if (isKnot != null)
                {
                    posts = posts.Where(e => e.IsKnot == isKnot);
                }
                if (isEssence != null)
                {
                    posts = posts.Where(e => e.IsEssence == isEssence);
                }
                await posts.Where(e => e.PostStatus.Name != "审核中")
                      .AsNoTracking()
                      .Include(e => e.PostStatus)
                      .Include(e => e.PostType)
                      .OrderBy(e => e.CreateTime)
                      .Skip((pageIndex - 1) * pageDataCount)
                      .Take(pageDataCount)
                      .ForEachAsync(e =>
                      {
                          list.Add(TODTO(e, commentBs.GetAll().LongCount(x => x.PostId == e.Id)));
                      });
                return list;
            }
        }

        public async Task MarkDeleteAsync(long id)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                await bs.MarkDeleteAsync(id);
            }
        }

        public async Task<long> TotalCountAsync()
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                return await bs.GetAll().LongCountAsync(e => e.PostStatus.Name != "审核中");
            }
        }

        public async Task UpdateAsync(UpdatePostDTO dto)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                var post = await bs.GetAll().SingleAsync(e => e.Id == dto.Id);
                post.PostStatusId = dto.PostStatusId;
                post.IsEssence = dto.IsEssence;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<long> UserCollectionPostAsync(long userId, long postId)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostCollectionEntity> bs = new BaseService<PostCollectionEntity>(ctx);
                PostCollectionEntity entity = new PostCollectionEntity();
                entity.UserId = userId;
                entity.PostId = postId;
                await ctx.PostCollections.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<List<ListPostDTO>> GetAdminWebPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                List<ListPostDTO> list = new List<ListPostDTO>();
                BaseService<PostCommentEntity> commentBs = new BaseService<PostCommentEntity>(ctx);
                
                await bs.GetAll()
                      .AsNoTracking()
                      .Include(e => e.PostStatus)
                      .Include(e => e.PostType)
                      .OrderBy(e => e.CreateTime)
                      .Skip((pageIndex - 1) * pageDataCount)
                      .Take(pageDataCount)
                      .ForEachAsync(e =>
                      {
                          list.Add(TODTO(e, commentBs.GetAll().LongCount(x => x.PostId == e.Id)));
                      });
                return list;
            }
        }

        public async Task<long> GetAdminWebTotalCountAsync()
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                return await bs.TotalCountAsync();
            }
        }

        public async Task<List<ListPostDTO>> GetQuestionPostByUserIdAsync(long userId)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                List<ListPostDTO> list = new List<ListPostDTO>();
                BaseService<PostCommentEntity> commentBs = new BaseService<PostCommentEntity>(ctx);

                await bs.GetAll()
                      .AsNoTracking()
                      .Include(e => e.PostStatus)
                      .Include(e => e.PostType)
                      .OrderBy(e => e.CreateTime)
                      .Where(e=>e.UserId==userId&&e.PostTypeId==1&&e.CreateTime>DateTime.Now.AddDays(-2))
                      .ForEachAsync(e =>
                      {
                          list.Add(TODTO(e, commentBs.GetAll().LongCount(x => x.PostId == e.Id)));
                      });
                return list;
            }
        }

        public async Task AdoptPostAsync(long userId, long postId, long commentId)
        {
            using (PostContext ctx = new PostContext())
            {
                BaseService<PostEntity> bs = new BaseService<PostEntity>(ctx);
                var post =await bs.GetAll().SingleAsync(e=>e.Id==postId);
                post.IsKnot = true;
                BaseService<PostCommentEntity> commentBs = new BaseService<PostCommentEntity>(ctx);
                var comment = await commentBs.GetAll().SingleAsync(e=>e.Id==commentId);
                comment.IsUse = true;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
