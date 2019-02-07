using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainSDK.Models
{
    public class AddCommentModel
    {
        [Required]
        public string Content { get; set; }
        [Required]
        /// <summary>
        /// 所属帖子ID
        /// </summary>
        public long PostId { get; set; }
        /// <summary>
        /// 评论人Id
        /// </summary>
        public long CommentUserId { get; set; }
        [Required]
        /// <summary>
        /// 收到回复的用户Id
        /// </summary>
        public long ReplyUserId { get; set; }
    }
}
