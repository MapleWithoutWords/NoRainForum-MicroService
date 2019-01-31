using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Admin.API.Models
{
    public class UpdateRolePermissionModel
    {
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(32, ErrorMessage = "名称的长度不能超过32个字符")]
        public string Name { get; set; }
        [Required(ErrorMessage = "描述不能为空")]
        [StringLength(64, ErrorMessage = "描述的长度不能超过64个字符")]
        public string Description { get; set; }
        [Required(ErrorMessage = "id不能为空")]
        [Range(1, long.MaxValue)]
        public long Id { get; set; }
    }
}
