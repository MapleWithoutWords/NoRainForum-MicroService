using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.AdminWeb.Models
{
    public class AddRoleModel
    {
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(32, ErrorMessage = "名称长度不能超过32个字符")]
        public string Name { get; set; }
        [Required(ErrorMessage = "描述信息不能为空")]
        [StringLength(64, ErrorMessage = "描述信息长度不能超过64个字符")]
        public string Description { get; set; }
        public long[] Ids { get; set; }
    }
}
