using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.User.API.Models
{
    public class AddUserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(16,ErrorMessage ="昵称不能超过16个字符")]
        public string NickName { get; set; }
        [Required]
        public bool Gender { get; set; }
        [StringLength(16, ErrorMessage = "密码长度为：6-16个字符")]
        [Required]
        public string Password { get; set; }
    }
}
