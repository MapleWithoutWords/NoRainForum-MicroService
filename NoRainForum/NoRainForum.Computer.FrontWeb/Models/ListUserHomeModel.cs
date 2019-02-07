using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Models
{
    public class ListUserHomeModel
    {
        public List<ListPostDTO> Posts { get; set; }
        public List<ListPostCommentDTO> Comments { get; set; }
        public List<long> LookCount { get; set; } = new List<long>();
    }
}
