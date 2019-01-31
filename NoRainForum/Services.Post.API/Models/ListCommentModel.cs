using Services.Post.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Post.API.Models
{
    public class ListCommentModel
    {
        public List<ListPostCommentDTO> Comments { get; set; }
        public long TotalCount { get; set; }
    }
}
