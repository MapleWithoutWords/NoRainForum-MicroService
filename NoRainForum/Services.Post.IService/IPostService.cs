using Services.Common.IServiceCommon;
using Services.Post.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Post.IService
{
    public interface IPostService : IBaseService<ListPostDTO>
    {
        Task<long> AddNewAsync(AddPostDTO dto);
        Task UpdateAsync(UpdatePostDTO dto);
        /// <summary>
        /// 根据类型获取帖子
        /// </summary>
        /// <param name="postTypeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <param name="isKnot">是否结贴</param>
        /// <param name="isEssence">是否精华</param>
        /// <returns></returns>
        Task<List<ListPostDTO>> GetPageByTypeIdAsync(long postTypeId, int pageIndex = 1, int pageDataCount = 10, bool? isKnot = null, bool? isEssence = null);
        Task<long> GetByPostTypeIdCountAsync(long typeId, bool? isKnot = null, bool? isEssence = null);

        Task<List<ListPostDTO>> GetByUserIdAsync(long userId, int pageIndex = 1, int pageDataCount = 10);
        Task<long> GetByUserIdTotalCountAsync(long userId);

        Task<List<ListPostDTO>> GetPageCollectionPostByUserIdAsync(long userId, int pageIndex = 1, int pageDataCount = 10);
        Task<long> GetCollectionPostByUserIdTotalCountAsync(long userId);
        /// <summary>
        /// 获取评论数量最多的十条帖子
        /// </summary>
        /// <returns></returns>
        Task<List<ListPostDTO>> GetByCommentCountAsync();
        /// <summary>
        /// 获取一周内最热的帖子：评论数最多
        /// </summary>
        /// <returns></returns>
        Task<List<ListPostDTO>> GetByDayCommentAsync();


        Task<long> UserCollectionPostAsync(long userId,long postId);
    }
}
