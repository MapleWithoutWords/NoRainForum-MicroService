using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Models
{
    public class ListPostUserModel
    {
        /// <summary>
        /// 获取置顶的帖子
        /// </summary>
        public List<ListPostDTO> StickPosts { get; set; }
        /// <summary>
        /// 获取指定的帖子的用户
        /// </summary>
        public List<ListUserDTO> StickUsers { get; set; }
        /// <summary>
        /// 获取综合帖子
        /// </summary>
        public List<ListPostDTO> ColligatePosts { get; set; }
        /// <summary>
        /// 获取综合帖子的用户
        /// </summary>
        public List<ListUserDTO> ColligateUsers { get; set; }
    }
}
