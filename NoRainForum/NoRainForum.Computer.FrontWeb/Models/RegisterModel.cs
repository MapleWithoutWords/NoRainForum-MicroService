using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Models
{
    public class RegisterModel
    {

        [Required(ErrorMessage ="邮箱不能为空")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "昵称不能为空")]
        [StringLength(16, ErrorMessage = "昵称不能超过16个字符")]
        public string NickName { get; set; }
        [Required(ErrorMessage = "性别不能为空")]
        public bool Gender { get; set; }
        [StringLength(16, ErrorMessage = "密码长度为：6-16个字符")]
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        [Required(ErrorMessage = "验证码不能为空")]
        public string EmailCode { get; set; }
    }
}
