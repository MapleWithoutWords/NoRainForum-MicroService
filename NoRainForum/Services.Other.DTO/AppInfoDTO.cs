using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Other.DTO
{
    public class AppInfoDTO
    {
        public long Id { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
