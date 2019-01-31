using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.DTO
{
    public class AddPostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 发表人Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 帖子类型Id
        /// </summary>
        public long PostTypeId { get; set; }
        /// <summary>
        /// 帖子状态Id
        /// </summary>
        public long PostStatusId { get; set; }
    }
}
