using Services.Common.ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.User.Model.Entities
{
    public class UserEntity:BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadImgSrc { get; set; } = "https://avatars1.githubusercontent.com/u/38368335?s=40&v=4";
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Autograph { get; set; } = "这个人很懒，什么都没有留下";
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City { get; set; } = "未知";
        public int LoginErrorCount { get; set; } = 0;
        public DateTime LoginErrorTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 邮箱是否被激活
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
