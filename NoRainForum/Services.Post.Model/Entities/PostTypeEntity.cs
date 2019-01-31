using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.Model.Entities
{
    public class PostTypeEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
