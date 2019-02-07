using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainSDK.Models
{
    public class AdoptPostModel
    {

        public long UserId { get; set; }
        [Required]
        public long  PostId { get; set; }
        [Required]
        public long CommentId { get; set; }
    }
}
