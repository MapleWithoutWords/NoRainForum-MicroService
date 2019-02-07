using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common.ModelCommon
{
   public abstract class BaseEntity
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
