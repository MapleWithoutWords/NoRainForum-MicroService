using Services.Common.IServiceCommon;
using Services.Post.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Post.IService
{
    public interface IPostCommontService : IBaseService<ListPostCommentDTO>
    {
        Task<long> AddNewAsync(AddPostCommentDTO dto);
        /// <summary>
        /// 获取用户一天的评论
        /// </summary>
        /// <param name="sendUserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        Task<List<ListPostCommentDTO>> GetByPageSendUserIdAsync(long commentUserId);
        /// <summary>
        /// 根据帖子Id获取该帖子的评论
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<List<ListPostCommentDTO>> GetPageByPostIdAsync(long postId, int pageIndex=1, int pageDataCount=10);
        /// <summary>
        /// 获取该帖子的评论总数量
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<long> GetTotalCountByPostIdAsync(long postId);
        /// <summary>
        /// 获取帖子的评论数量
        /// </summary>
        /// <param name="postIds"></param>
        /// <returns></returns>
        Task<List<long>> GetByPostIdCountAsync(IEnumerable<long> postIds);
        /// <summary>
        /// 获取回复我的消息
        /// </summary>
        /// <param name="replyUserId"></param>
        /// <returns></returns>
        Task<List<ListPostCommentDTO>> GetByPageReplyUserIdAsync(long replyUserId, int pageIndex, int pageDataCount);
    }
}
