using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Other.Model.Entities
{
    public class SettingEntity:BaseEntity
    {
        public string KeyPari { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
