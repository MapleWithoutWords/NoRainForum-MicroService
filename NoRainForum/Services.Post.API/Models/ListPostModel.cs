using Services.Post.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Post.API.Models
{
    public class ListPostModel
    {
        public List<ListPostDTO> Posts { get; set; }
        public long TotalCount { get; set; }
    }
}
