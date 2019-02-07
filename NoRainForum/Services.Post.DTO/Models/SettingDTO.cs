using System;
using System.Collections.Generic;
using System.Text;

namespace NoRainSDK.Models
{
    public class SettingDTO
    {
        public long Id { get; set; }
        public string KeyPari { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
