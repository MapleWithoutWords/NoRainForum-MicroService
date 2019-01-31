using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.Model.Entities
{
    public class PostCollectionEntity:BaseEntity
    {
        public long PostId { get; set; }
        public long UserId { get; set; }
        public PostEntity Post { get; set; }
    }
}
