using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.DTO
{
    public class AddPostCommentDTO
    {
        public string Content { get; set; }
        /// <summary>
        /// 所属帖子ID
        /// </summary>
        public long PostId { get; set; }
        /// <summary>
        /// 评论人Id
        /// </summary>
        public long CommentUserId { get; set; }
        /// <summary>
        /// 收到回复的用户Id
        /// </summary>
        public long ReplyUserId { get; set; }
    }
}
