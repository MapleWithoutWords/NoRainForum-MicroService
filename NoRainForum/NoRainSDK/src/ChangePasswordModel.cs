using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainSDK.src
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage ="id不能为空")]
        public long Id { get; set; }
        [Required(ErrorMessage = "旧密码不能为空")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "新密码不能为空")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "新密码长度为6-16个字符")]
        public string NewPassword { get; set; }
    }
}
