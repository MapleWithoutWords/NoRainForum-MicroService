using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSRedis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoRainForum.Computer.FrontWeb.Filters;
using NoRainForum.Computer.FrontWeb.Models;
using NoRainForumCommon;
using NoRainSDK.Models;
using NoRainSDK.src;

namespace NoRainForum.Computer.FrontWeb.Controllers
{
    public class PostController : Controller
    {
        public PostService PostSvc { get; set; }
        public UserService UserSvc { get; set; }
        public PostCommentService CommentSvc { get; set; }
        public PostTypeService TypeSvc { get; set; }
        public PostController(PostService PostSvc, UserService UserSvc, PostCommentService CommentSvc, PostTypeService TypeSvc)
        {
            this.PostSvc = PostSvc;
            this.UserSvc = UserSvc;
            this.CommentSvc = CommentSvc;
            this.TypeSvc = TypeSvc;
        }
        /// <summary>
        /// 帖子详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Detail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Detail(long postId)
        {
            ListContentPostDTO post;
            ListUserDTO user;
            string postCache = await RedisHelper.GetAsync($"postDetail_{postId}");
            string userCache = await RedisHelper.GetAsync($"user_{postId}");
            if (!string.IsNullOrEmpty(postCache) && !string.IsNullOrEmpty(userCache))
            {
                post = JsonConvert.DeserializeObject<ListContentPostDTO>(postCache);
                user = JsonConvert.DeserializeObject<ListUserDTO>(userCache);
            }
            else
            {
                post = await PostSvc.GetByIdAsync(postId);
                user = await UserSvc.GetByIdAsync(post.UserId);

                if (post == null)
                {
                    return Json(new AjaxResult { Status = "redirect", Data = "/Error/Error404" });
                }
                if (user == null)
                {
                    return Json(new AjaxResult { Status = "error", Data = UserSvc.ErrorMsg });
                }
                await RedisHelper.SetAsync($"postDetail_{postId}", post);
                await RedisHelper.SetAsync($"user_{postId}", user);
            }
            long count = await RedisHelper.IncrByAsync("post_" + postId);
            PostDetailModel model = new PostDetailModel();
            model.Post = post;
            model.User = user;
            model.LookCount = count;

            return Json(new AjaxResult { Status = "ok", Data = model });
        }
        /// <summary>
        /// 分页获取帖子的评论
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetCommentByPostId(long postId, int pageIndex = 1, int pageDataCount = 5)
        {
            ListModel<ListPostCommentDTO> Comments = await CommentSvc.GetByPostIdAsync(postId, pageIndex, pageDataCount);
            if (Comments == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = CommentSvc.ErrorMsg });
            }
            List<long> userIds = Comments.Datas.Select(e => e.CommentUserId).ToList();
            var users = await UserSvc.GetByIdsAsync(userIds);
            if (users == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = UserSvc.ErrorMsg });
            }
            PostDetailCommentModel model = new PostDetailCommentModel();
            model.Comments = Comments.Datas;
            model.TotalCount = Comments.TotalCount;
            model.Users = users;
            return Json(new AjaxResult { Status = "ok", Data = model });
        }
        /// <summary>
        /// 根据帖子类型获取帖子
        /// </summary>
        /// <param name="postTypeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageDataCount"></param>
        /// <param name="isKnot"></param>
        /// <param name="isEssence"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByTypeId()
        {
            return View();
        }
        /// <summary>
        /// 根据帖子类型获取帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetByTypeId(PostGetByTypeIdModel model)
        {
            var posts = await PostSvc.GetByPostTypeIdAsync(model.PostTypeId, model.PageIndex, model.PageDataCount, model.IsKnot,
                model.IsEssence);
            if (posts == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PostSvc.ErrorMsg });
            }
            List<long> userIds = posts.Datas.Select(e => e.UserId).ToList();
            var users = await UserSvc.GetByIdsAsync(userIds);
            if (users == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = UserSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok", Data = new { Posts = posts, Users = users } });
        }

        /// <summary>
        /// 综合帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageData()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetPageData(int pageIndex = 1, int pageDataCount = 10, bool? isKnot = null, bool? isEssence = null)
        {
            var posts = await PostSvc.GetColligatePostAsync(pageIndex, pageDataCount, isKnot, isEssence);
            if (posts == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PostSvc.ErrorMsg });
            }
            List<long> userIds = posts.Datas.Select(e => e.UserId).ToList();
            var users = await UserSvc.GetByIdsAsync(userIds);
            if (users == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = UserSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok", Data = new { Posts = posts, Users = users } });
        }
        /// <summary>
        /// 评论帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckLogin]
        public async Task<IActionResult> Comment(AddCommentModel model)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            model.CommentUserId = userId;
            var replyUserId = await UserSvc.GetByIdAsync(model.ReplyUserId);
            if (replyUserId == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "您所回复的用户不存在" });
            }
            if (await CommentSvc.AddNewAsync(model) < 1)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = CommentSvc.ErrorMsg });
            }
            ListUserDTO commentUser = await UserSvc.GetByIdAsync(userId);
            ListContentPostDTO post;
            if (RedisHelper.Exists($"postDetail_{model.PostId}"))
            {
                post = JsonConvert.DeserializeObject<ListContentPostDTO>(await RedisHelper.GetAsync($"postDetail_{model.PostId}"));
            }
            else
            {
                post = await PostSvc.GetByIdAsync(model.PostId);
                await RedisHelper.SetAsync($"postDetail_{model.PostId}", post);
            }
            MessageModel msg = new MessageModel();
            msg.CommentUserName = commentUser.NickName;
            msg.PostId = model.PostId;
            msg.ReplyUserId = model.ReplyUserId;
            msg.PostTitle = post.Title;
            await RedisHelper.SAddAsync($"msg_{model.ReplyUserId}", msg);
            return Json(new AjaxResult { Status = "ok" });
        }
        /// <summary>
        /// 采纳评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckLogin]
        public async Task<IActionResult> Adopt(AdoptPostModel model)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            model.UserId = userId;
            if (!await PostSvc.AdoptPostAsync(model))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PostSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }
        /// <summary>
        /// 发表帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CheckLogin]
        public async Task<IActionResult> Add()
        {
            var postTypes = await TypeSvc.GetAllAsync();
            return View(postTypes);
        }
        [HttpPost]
        [CheckLogin]
        public async Task<IActionResult> Add(FrontAddPostModel model)
        {
            if (TempData[ConstList.VALIDCODE] == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码过期" });
            }
            string code = (string)TempData[ConstList.VALIDCODE];
            TempData[ConstList.VALIDCODE] = null;
            if (!code.Equals(model.ValidCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            var user = await UserSvc.GetByIdAsync(userId);
            if (!user.IsActive)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "邮箱未激活" });
            }
            AddPostModel addPost = new AddPostModel();
            addPost.Content = model.Content;
            addPost.PostTypeId = model.PostTypeId;
            addPost.Title = model.Title;
            addPost.UserId = userId;
            if (await PostSvc.AddNewAsync(addPost) < 1)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PostSvc.ErrorMsg });
            }
            return Json(new AjaxResult { Status = "ok" });
        }


        /// <summary>
        /// 收藏帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckLogin]
        public async Task<IActionResult> Collection(UserCollectionModel model)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString(ConstList.USERID));
            if (await RedisHelper.SIsMemberAsync($"collection_{userId}",model.PostId))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "不能重复收藏帖子" });
            }
            model.UserId = userId;
            if (!await PostSvc.UserCollectionPostAsync(model))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = PostSvc.ErrorMsg });
            }
            await RedisHelper.SAddAsync($"collection_{userId}", model.PostId);
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}