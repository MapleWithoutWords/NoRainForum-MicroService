using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Other.Model.Entities
{
    public class AppInfoEntity:BaseEntity
    {
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string Email { get; set; }
    }
}
