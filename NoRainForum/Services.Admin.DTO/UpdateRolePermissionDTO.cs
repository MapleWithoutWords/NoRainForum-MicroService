using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.DTO
{
    public class UpdateRolePermissionDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long Id { get; set; }
    }
}
