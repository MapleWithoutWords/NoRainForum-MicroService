using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.AdminWeb.Models
{
    public class AdminWebListPostModel
    {
        public List<ListPostDTO> Posts { get; set; }
        public long TotalCount { get; set; }
        public List<ListUserDTO> Users { get; set; }
        public string  Page { get; set; }
    }
}
