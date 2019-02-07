using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainSDK.Models
{
    public class UpdateSettingModel
    {
        [Required(ErrorMessage ="id不为空")]
        public long Id { get; set; }
        [Required(ErrorMessage = "键不为空")]
        public string KeyPari { get; set; }
        [Required(ErrorMessage = "值key不为空")]
        public string Key { get; set; }
        [Required(ErrorMessage = "值value不为空")]
        public string Value { get; set; }
    }
}
