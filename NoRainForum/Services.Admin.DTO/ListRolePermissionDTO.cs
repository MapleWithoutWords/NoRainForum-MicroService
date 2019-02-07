using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.DTO
{
    public class ListRolePermissionDTO
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
