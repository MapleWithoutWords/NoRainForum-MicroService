using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Models
{
    public class FrontAddPostModel
    {
        [Required]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "标题长度为:6-64个字符")]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        [Range(0, long.MaxValue)]
        public long PostTypeId { get; set; }
        [Required]
        public string ValidCode { get; set; }
    }
}
