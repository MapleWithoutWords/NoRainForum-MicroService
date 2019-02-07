using NoRainForum.Computer.AdminWeb.common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.AdminWeb.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="电话号码不能为空")]
        [Phone(ErrorMessage ="电话号码格式错误")]
        public string PhoneNum { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        [Required(ErrorMessage = "验证码不能为空")]
        public string ValidCode { get; set; }
    }
}
