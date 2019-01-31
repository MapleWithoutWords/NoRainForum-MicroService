using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Post.API.Models
{
    public class UpdatePostModel
    {
        public long Id { get; set; }
        public long PostStatusId { get; set; }
    }
}
