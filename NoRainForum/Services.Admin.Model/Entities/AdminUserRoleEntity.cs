using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.Model.Entities
{
    public class AdminUserRoleEntity:BaseEntity
    {
        public long AdminUserId { get; set; }
        public AdminUserEntity AdminUser { get; set; }
        public long RoleId { get; set; }
        public RoleEntity Role { get; set; }
    }
}
