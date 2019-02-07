using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Models
{
    public class EmailValidCodeModel
    {
        [Required(ErrorMessage = "邮箱不能为空")]
        [EmailAddress(ErrorMessage = "email格式错误")]
        public string RecipientEmail { get; set; }
        [Required(ErrorMessage = "验证码不能为空")]
        public string ValidCode { get; set; }
    }
}
