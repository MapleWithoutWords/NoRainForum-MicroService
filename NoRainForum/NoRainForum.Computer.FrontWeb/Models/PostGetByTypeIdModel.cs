using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Models
{
    public class PostGetByTypeIdModel
    {
        [Required(ErrorMessage ="帖子类型Id不能为空")]
        public long PostTypeId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageDataCount { get; set; } = 10;
        public bool? IsKnot { get; set; } = null;
        public bool?  IsEssence { get; set; } = null;
    }
}
