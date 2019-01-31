using System;
using System.Collections.Generic;
using System.Text;

namespace NoRainSDK.Models
{
    public class IdNameDTO
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
