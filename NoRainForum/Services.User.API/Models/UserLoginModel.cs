using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.User.API.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage ="邮箱不能为空")]
        [EmailAddress(ErrorMessage ="邮箱格式错误")]
        public string Email { get; set; }
        [Required(ErrorMessage ="密码不能为空")]
        public string Password { get; set; }
    }
}
