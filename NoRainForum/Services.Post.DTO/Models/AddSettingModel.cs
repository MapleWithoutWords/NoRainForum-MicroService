using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainSDK.Models
{
    public class AddSettingModel
    {
        [Required(ErrorMessage = "key不能为空")]
        [StringLength(256,ErrorMessage ="长度为256个字符")]
        public string KeyPari { get; set; }
        [Required(ErrorMessage = "值key不能为空")]
        [StringLength(256, ErrorMessage = "长度为256个字符")]
        public string Key { get; set; }
        [Required(ErrorMessage = "值不能为空")]
        [StringLength(256, ErrorMessage = "长度为256个字符")]
        public string Value { get; set; }
    }
}
