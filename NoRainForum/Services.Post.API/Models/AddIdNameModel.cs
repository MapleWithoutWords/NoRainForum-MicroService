using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Post.API.Models
{
    public class AddIdNameModel
    {
        [Required]
        [StringLength(32,ErrorMessage ="名字不能超过32个字符")]
        public string Name { get; set; }
        [Required]
        [StringLength(64, ErrorMessage = "名字不能超过64个字符")]
        public string Description { get; set; }
    }
}
