using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Models
{
    public class PostDetailCommentModel
    {
        public List<ListPostCommentDTO> Comments { get; set; }
        public long  TotalCount { get; set; }
        public List<ListUserDTO> Users { get; set; }

    }
}
