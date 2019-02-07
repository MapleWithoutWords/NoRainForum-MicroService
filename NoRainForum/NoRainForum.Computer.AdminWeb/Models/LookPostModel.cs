using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.AdminWeb.Models
{
    public class LookPostModel
    {
        public ListContentPostDTO Post { get; set; }
        public ListUserDTO User { get; set; }
        public List<IdNameDTO> PostStatuses { get; set; }
    }
}
