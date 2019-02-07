using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.User.API.Models
{
    public class RePasswordModel
    {
        [Required(ErrorMessage = "邮箱不能为空")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "新密码不能为空")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "新密码长度为6-16个字符")]
        public string NewPassword { get; set; }
    }
}
