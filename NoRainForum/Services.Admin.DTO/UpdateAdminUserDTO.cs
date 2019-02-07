using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.DTO
{
    public class UpdateAdminUserDTO
    {
        public string PhoneNum { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public long Id { get; set; }
    }
}
