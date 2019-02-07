using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.AdminWeb.Models
{
    public class UpdateAdminUserListModel
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string PhoneNum { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public List<ListRolePermissionDTO> Roles { get; set; }
        public List<ListRolePermissionDTO> AdminUserRoles { get; set; }
    }
}
