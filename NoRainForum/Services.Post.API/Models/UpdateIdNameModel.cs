using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Post.API.Models
{
    public class UpdateIdNameModel
    {
        [Required(ErrorMessage = "id不能为空")]
        [Range(0, long.MaxValue)]
        public long Id { get; set; }
        [Required(ErrorMessage = "名字不能为空")]
        [StringLength(32,ErrorMessage ="名字最长为32个字符")]
        public string Name { get; set; }
        [Required(ErrorMessage = "描述不能为空")]
        [StringLength(64, ErrorMessage = "描述最长为64个字符")]
        public string Description { get; set; }
    }
}
