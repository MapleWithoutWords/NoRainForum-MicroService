using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.DTO
{
    public class ListPostCommentDTO
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 所属帖子ID
        /// </summary>
        public long PostId { get; set; }
        /// <summary>
        /// 帖子所属用户的ID
        /// </summary>
        public long PostUserId { get; set; }
        /// <summary>
        /// 所属帖子标题
        /// </summary>
        public string PostTitle { get; set; }
        /// <summary>
        /// 评论人Id
        /// </summary>
        public long CommentUserId { get; set; }
        /// <summary>
        /// 收到回复用户Id
        /// </summary>
        public long ReplyUserId { get; set; }
        /// <summary>
        /// 是否被采用
        /// </summary>
        public bool IsUse { get; set; }
        /// <summary>
        /// 是否结贴
        /// </summary>
        public bool IsKnot { get; set; }
    }
}
