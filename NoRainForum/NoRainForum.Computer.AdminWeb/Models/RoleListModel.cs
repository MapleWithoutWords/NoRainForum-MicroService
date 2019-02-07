using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.AdminWeb.Models
{
    public class RoleListModel
    {
        public ListRolePermissionDTO Role { get; set; }
        public List<ListRolePermissionDTO> RolePers { get; set; }
        public List<ListRolePermissionDTO> Pers { get; set; }
    }
}
