using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.Model.Entities
{
    public class RolePermissionEntity:BaseEntity
    {
        public long RoleId { get; set; }
        public RoleEntity Role { get; set; }
        public long PermissionId { get; set; }
        public PermissionEntity Permission { get; set; }
    }
}
