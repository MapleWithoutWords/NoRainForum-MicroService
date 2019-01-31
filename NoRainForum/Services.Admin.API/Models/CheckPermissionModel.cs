using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Admin.API.Models
{
    public class CheckPermissionModel
    {
        [Required(ErrorMessage ="管理员id不能为空")]
        public long AdminUserId { get; set; }
        [Required(ErrorMessage = "权限名不能为空")]
        [StringLength(32,ErrorMessage ="权限名最多32个字符")]
        public string PermissionName { get; set; }
    }
}
