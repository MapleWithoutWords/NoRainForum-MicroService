using System;
using System.Collections.Generic;
using System.Text;

namespace NoRainSDK.Models
{
    public class ListPostDTO
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 发表人Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 帖子类型Id
        /// </summary>
        public long PostTypeId { get; set; }
        /// <summary>
        /// 帖子类型名称
        /// </summary>
        public string PostTypeName { get; set; }
        /// <summary>
        /// 帖子状态Id
        /// </summary>
        public long PostStatusId { get; set; }
        /// <summary>
        /// 帖子状态名称
        /// </summary>
        public string PostStatusName { get; set; }
        /// <summary>
        /// 是否结贴
        /// </summary>
        public bool IsKnot { get; set; } = false;
        /// <summary>
        /// 是否精华
        /// </summary>
        public bool IsEssence { get; set; } = false;
        /// <summary>
        /// 评论数量
        /// </summary>
        public long CommentCount { get; set; }
    }
}
