using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.Model.Entities
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
