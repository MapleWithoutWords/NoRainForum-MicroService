using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Models
{
    public class PostDetailModel
    {
        public ListContentPostDTO Post { get; set; }
        public ListUserDTO User { get; set; }
        public long LookCount { get; set; }
    }
}
