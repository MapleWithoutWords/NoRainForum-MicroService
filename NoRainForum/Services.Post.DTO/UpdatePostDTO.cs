using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.DTO
{
    public class UpdatePostDTO
    {
        public long Id { get; set; }
        public long PostStatusId { get; set; }
        public bool IsEssence { get; set; } = false;
    }
}
