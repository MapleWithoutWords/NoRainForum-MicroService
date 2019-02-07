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
    public class PostController : ControllerBase
    {
        public IPostService PostSvc { get; set; }
        public IPostTypeService TypeSvc { get; set; }
        public IPostCommontService CommentSvc { get; set; }
        public IPostStatusService StatuSvc { get; set; }
        public PostController(IPostService PostSvc, IPostTypeService TypeSvc, IPostStatusService StatuSvc, IPostCommontService CommentSvc)
        {
            this.PostSvc = PostSvc;
            this.TypeSvc = TypeSvc;
            this.StatuSvc = StatuSvc;
            this.CommentSvc = CommentSvc;
        }
        /// <summary>
        /// 分页获取所有帖子
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        [HttpGet("GetPageData")]
        public async Task<IActionResult> GetPageData(int pageIndex = 1, int pageDataCount = 10, bool? isKnot = null, bool? isEssence = null)
        {
            var Posts = await PostSvc.GetPageDataAsync(pageIndex, pageDataCount, isKnot, isEssence);
            var TotalCount = await PostSvc.TotalCountAsync();
            return new JsonResult(
                new APIResult<ListModel<ListPostDTO>>()
                {
                    Data = new ListModel<ListPostDTO> { Datas = Posts, TotalCount = TotalCount }
                });
        }

        [HttpGet("GetAdminWebPageData")]
        public  async Task<IActionResult> GetAdminWebPageData(int pageIndex = 1, int pageDataCount = 10)
        {
            var Posts = await PostSvc.GetAdminWebPageDataAsync(pageIndex, pageDataCount);
            var TotalCount = await PostSvc.GetAdminWebTotalCountAsync();
            return new JsonResult(
                new APIResult<ListModel<ListPostDTO>>()
                {
                    Data = new ListModel<ListPostDTO> { Datas = Posts, TotalCount = TotalCount }
                });
        }

        [HttpGet("GetQuestionPostByUserId")]
        public async Task<IActionResult> GetQuestionPostByUserId(long userId)
        {
            var Posts = await PostSvc.GetQuestionPostByUserIdAsync(userId);
            return new JsonResult(
                new APIResult<List<ListPostDTO>>()
                {
                    Data = Posts
                });
        }

        /// <summary>
        /// 根据帖子类型分页获取帖子
        /// </summary>
        /// <param name="postTypeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <param name="IsKnot">是否结贴</param>
        /// <param name="IsEssence">是否精华</param>
        /// <returns></returns>
        [HttpGet("GetByTypeId")]
        public async Task<IActionResult> GetByTypeId(long postTypeId, int pageIndex = 1, int pageDataCount = 10, bool? IsKnot = null, bool? IsEssence = null)
        {
            var postType = await TypeSvc.GetByIdAsync(postTypeId);
            if (postType == null)
            {
                return new JsonResult(
                    new APIResult<long>()
                    {
                        ErrorMsg = "帖子类型不存在"
                    }
                )
                { StatusCode = 400 };
            }
            return new JsonResult(
                 new APIResult<ListModel<ListPostDTO>>()
                 {
                     Data = new ListModel<ListPostDTO>
                     {
                         Datas = await PostSvc.GetPageByTypeIdAsync(postTypeId, pageIndex, pageDataCount, IsKnot, IsEssence),
                         TotalCount = await PostSvc.GetByPostTypeIdCountAsync(postTypeId, IsKnot, IsEssence)
                     }
                 }
            );
        }
        /// <summary>
        /// 获取该用户发的帖子
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId(long userId, int pageIndex = 1, int pageDataCount = 10)
        {
            return new JsonResult(
                   new APIResult<ListModel<ListPostDTO>>()
                   {
                       Data = new  ListModel<ListPostDTO>
                       {
                           Datas = await PostSvc.GetByUserIdAsync(userId, pageIndex, pageDataCount),
                           TotalCount = await PostSvc.GetByUserIdTotalCountAsync(userId)
                       }
                   }
               );
        }
        /// <summary>
        /// 获取用户收藏的帖子
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        [HttpGet("GetCollectionPostByUserId")]
        public async Task<IActionResult> GetCollectionPostByUserId(long userId, int pageIndex = 1, int pageDataCount = 10)
        {
            return new JsonResult(
                    new APIResult<ListModel<ListPostDTO>>()
                    {
                        Data = new ListModel<ListPostDTO>
                        {
                            Datas = await PostSvc.GetPageCollectionPostByUserIdAsync(userId, pageIndex, pageDataCount),
                            TotalCount = await PostSvc.GetCollectionPostByUserIdTotalCountAsync(userId)
                        }
                    }
               );
        }
        /// <summary>
        /// 获取置顶的帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStick")]
        public async Task<IActionResult> GetStick()
        {
            return new JsonResult(
              new APIResult<List<ListPostDTO>>()
              {
                  Data = await PostSvc.GetByCommentCountAsync()
              }
           );
        }
        /// <summary>
        /// 获取一周内最热的帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDayPost")]
        public async Task<IActionResult> GetDayPost()
        {
            return new JsonResult(new APIResult<List<ListPostDTO>> { Data = await PostSvc.GetByDayCommentAsync() });
        }
        /// <summary>
        /// 根据Id获取帖子
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            var post = await PostSvc.GetByIdAsync(id);
            if (post == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "帖子不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<ListPostDTO>()
            {
                Data = post
            });
        }

        /// <summary>
        /// 添加帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Put")]
        public async Task<IActionResult> Put(AddPostModel model)
        {
            var postType = await TypeSvc.GetByIdAsync(model.PostTypeId);
            if (postType == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "帖子类型不存在" }) { StatusCode = 400 };
            }

            AddPostDTO dto = new AddPostDTO();
            dto.Content = model.Content;

            dto.Title = model.Title;
            dto.UserId = model.UserId;
            dto.PostTypeId = model.PostTypeId;
            if (postType.Name == "分享")
            {
                dto.PostStatusId = 1;
            }
            else
            {
                dto.PostStatusId = 2;
            }
            return new JsonResult(new APIResult<long> { Data = await PostSvc.AddNewAsync(dto) });
        }
        /// <summary>
        /// 用户收藏帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("UserCollectionPost")]
        public async Task<IActionResult> UserCollectionPost(UserCollectionModel model)
        {
            var post = await PostSvc.GetByIdAsync(model.PostId);
            if (post == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "帖子不存在" }) { StatusCode = 400 };
            }
            return new JsonResult(new APIResult<long> { Data = await PostSvc.UserCollectionPostAsync(model.UserId, model.PostId) });
        }


        /// <summary>
        /// 修改帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> Post(UpdatePostModel model)
        {
            var post = await PostSvc.GetByIdAsync(model.Id);
            if (post == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "帖子不存在" }) { StatusCode = 400 };
            }
            var statu = await StatuSvc.GetByIdAsync(model.PostStatusId);
            if (statu == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "帖子状态不存在" }) { StatusCode = 400 };
            }
            UpdatePostDTO dto = new UpdatePostDTO();
            dto.Id = model.Id;
            dto.PostStatusId = model.PostStatusId;
            dto.IsEssence = model.IsEssence;
            await PostSvc.UpdateAsync(dto);
            return Ok();
        }



        /// <summary>
        /// 采纳帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AdoptPost")]
        public async Task<IActionResult> AdoptPost(AdoptPostModel model)
        {
            var post = await PostSvc.GetByIdAsync(model.PostId);
            if (post == null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "帖子不存在" }) { StatusCode = 400 };
            }
            if (post.UserId!=model.UserId)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "只有帖子拥有者才能采纳" }) { StatusCode = 400 };
            }
            var comment =await CommentSvc.GetByIdAsync(model.CommentId);
            if (comment==null)
            {
                return new JsonResult(new APIResult<long> { ErrorMsg = "评论Id不存在" }) { StatusCode = 400 };
            }
            await PostSvc.AdoptPostAsync(model.UserId,model.PostId,model.CommentId);
            return Ok();
        }
    }
}