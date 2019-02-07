using System;
using System.Collections.Generic;
using System.Text;

namespace NoRainSDK.Models
{
    public class ListAdminUserDTO
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string PhoneNum { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public DateTime LoginErrorTime { get; set; }
    }
}
