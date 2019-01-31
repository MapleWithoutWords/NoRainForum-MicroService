using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.Model.Entities
{
    public class AdminUserEntity:BaseEntity
    {
        public string PhoneNum { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public int LoginErrorCount { get; set; } = 0;
        public DateTime LoginErrorTime { get; set; } = DateTime.Now;
    }
}
