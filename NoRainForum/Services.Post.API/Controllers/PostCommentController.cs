using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.APICommon;
using Services.Post.API.Models;
using Services.Post.DTO;
using Services.Post.IService;

namespace Services.Post.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCommentController : ControllerBase
    {
        public IPostService PostSvc { get; set; }
        public IPostCommontService CommentSvc { get; set; }
        public PostCommentController(IPostService PostSvc, IPostCommontService CommentSvc)
        {
            this.PostSvc = PostSvc;
            this.CommentSvc = CommentSvc;
        }
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Put")]
        public async Task<IActionResult> Put(AddCommentModel model)
        {
            var post = await PostSvc.GetByIdAsync(model.PostId);
            if (post == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "帖子不存在" }) { StatusCode = 400 };
            }
            AddPostCommentDTO dto = new AddPostCommentDTO();
            dto.CommentUserId = model.CommentUserId;
            dto.Content = model.Content;
            dto.PostId = model.PostId;
            dto.ReplyUserId = model.ReplyUserId;
            return new JsonResult(new APIResult<long> { Data = await CommentSvc.AddNewAsync(dto) });
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Put")]
        public async Task<IActionResult> Delete(long id)
        {
            await CommentSvc.MarkDeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// 获取帖子的评论
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByPostId")]
        public async Task<IActionResult> GetByPostId(long postId, int pageIndex=1, int pageDataCount=10)
        {
            var post = await PostSvc.GetByIdAsync(postId);
            if (post == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "帖子不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<ListCommentModel>
            {
                Data = new ListCommentModel
                {
                    Comments = await CommentSvc.GetPageByPostIdAsync(postId, pageIndex=1, pageDataCount=10),
                    TotalCount = await CommentSvc.GetTotalCountByPostIdAsync(postId)
                }
            });
        }
        /// <summary>
        /// 获取该用户今天的评论
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByCommentUserId")]
        public async Task<IActionResult> GetByCommentUserId(long commentUserId)
        {
            return new JsonResult(new APIResult<List<ListPostCommentDTO>> { Data = await CommentSvc.GetByPageSendUserIdAsync(commentUserId) });
        }

        [HttpGet("GetByReplyUserId")]
        public async Task<IActionResult> GetByReplyUserId(long replyUserId, int pageIndex=1, int pageDataCount=10)
        {
            return new JsonResult(new APIResult<List<ListPostCommentDTO>> { Data = await CommentSvc.GetByPageReplyUserIdAsync(replyUserId, pageIndex=1, pageDataCount=10) });
        }
    }
}