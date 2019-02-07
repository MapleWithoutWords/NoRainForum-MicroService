using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainSDK.src
{
    public class UpdateUserModel
    {
        public long Id { get; set; }
        [Required]
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Autograph { get; set; }
        [Required]
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string NickName { get; set; }
        [Required]
        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
