using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.DTO
{
    public class AddAdminUserDTO
    {
        public string PhoneNum { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
    }
}
