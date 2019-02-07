using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Models
{
    public class MessageModel
    {
        public long PostId { get; set; }
        public string PostTitle { get; set; }
        public long ReplyUserId { get; set; }
        public string CommentUserName { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now.Date;
    }
}
