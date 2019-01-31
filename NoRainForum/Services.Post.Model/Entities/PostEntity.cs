using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.Model.Entities
{
    public class PostEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public long PostTypeId { get; set; }
        public long PostStatusId { get; set; }
        /// <summary>
        /// 是否结贴
        /// </summary>
        public bool IsKnot { get; set; } = false;
        /// <summary>
        /// 是否精华
        /// </summary>
        public bool IsEssence { get; set; } = false;
        public long UserId { get; set; }
        public PostTypeEntity PostType { get; set; }
        public PostStatusEntity PostStatus { get; set; }
    }
}
