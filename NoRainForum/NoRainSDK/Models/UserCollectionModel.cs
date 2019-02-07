using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainSDK.Models
{
    public class UserCollectionModel
    {
        [Required]
        public long PostId { get; set; }
        public long UserId { get; set; }
    }
}
