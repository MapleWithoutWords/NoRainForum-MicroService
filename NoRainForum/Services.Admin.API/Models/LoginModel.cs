using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Admin.API.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "电话号码不能为空")]
        [RegularExpression(@"^1(3|4|5|7|8)\d{9}$",ErrorMessage ="手机号码格式错误")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
    }
}
