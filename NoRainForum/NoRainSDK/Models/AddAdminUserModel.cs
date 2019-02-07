using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainSDK.Models
{
    public class AddAdminUserModel
    {
        [Required(ErrorMessage ="电话号码不能为空")]
        [RegularExpression(@"^1(3|4|5|7|8)\d{9}$", ErrorMessage = "手机号码格式错误")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(16,MinimumLength =6,ErrorMessage ="密码长度为：6-16个字符")]
        public string Password { get; set; }

        [Required(ErrorMessage = "名字不能为空")]
        [StringLength(16,ErrorMessage ="名字不能超过16个字符")]
        public string Name { get; set; }

        [Required(ErrorMessage = "性别不能为空")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "年龄不能为空")]
        [Range(1,256)]
        public int Age { get; set; }
    }
}
