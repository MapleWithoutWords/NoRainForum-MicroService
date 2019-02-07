using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Post.API.Models
{
    public class AdoptPostModel
    {
        public long UserId { get; set; }
        public long  PostId { get; set; }
        public long CommentId { get; set; }
    }
}
