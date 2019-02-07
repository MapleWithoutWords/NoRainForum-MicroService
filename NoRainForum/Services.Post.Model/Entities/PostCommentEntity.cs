using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.Model.Entities
{
    public class PostCommentEntity : BaseEntity
    {
        public string Content { get; set; }
        public long PostId { get; set; }
        public PostEntity Post { get; set; }
        /// <summary>
        /// 评论用户Id
        /// </summary>
        public long CommonUserId { get; set; }
        /// <summary>
        /// 回复用户Id
        /// </summary>
        public long ReplyUserId { get; set; }
        /// <summary>
        /// 是否被采用
        /// </summary>
        public bool IsUse { get; set; } = false;
    }
}
