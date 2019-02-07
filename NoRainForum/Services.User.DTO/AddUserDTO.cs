using System;
using System.Collections.Generic;
using System.Text;

namespace Services.User.DTO
{
    public class AddUserDTO
    {
        public string Email { get; set; }
        public string NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; set; }
        public string Password { get; set; }
    }
}
