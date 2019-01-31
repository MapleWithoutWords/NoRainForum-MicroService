using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainSDK.src
{
    public class UpdateUserModel
    {
        public long Id { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Autograph { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
