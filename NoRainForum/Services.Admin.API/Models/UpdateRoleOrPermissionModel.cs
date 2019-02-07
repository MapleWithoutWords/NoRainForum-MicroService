using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Admin.API.Models
{
    public class UpdateRoleOrPermissionModel
    {
        [Required(ErrorMessage = "id不能为空")]
        public long Id { get; set; }
        [Required(ErrorMessage = "ids不能为空")]
        public long[] Ids { get; set; }
    }
}
