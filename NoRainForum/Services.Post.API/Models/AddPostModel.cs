using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Post.API.Models
{
    public class AddPostModel
    {
        [Required(ErrorMessage ="标题不能为空")]
        [StringLength(64,MinimumLength =6,ErrorMessage ="标题长度为:6-64个字符")]
        public string Title { get; set; }
        [Required(ErrorMessage = "内容不能为空")]
        public string Content { get; set; }
        [Required(ErrorMessage = "用户不能为空")]
        [Range(0,long.MaxValue,ErrorMessage ="用户id必须大于0")]
        public long UserId { get; set; }
        [Required(ErrorMessage = "帖子类型不能为空")]
        [Range(0, long.MaxValue, ErrorMessage = "帖子类型id必须大于0")]
        public long PostTypeId { get; set; }
    }
}
