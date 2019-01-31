using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.AdminWeb.Models
{
    public class UpdateAdminUserEditModel
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string PhoneNum { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public long[] RoleIds { get; set; }
    }
}
