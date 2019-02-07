using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainSDK.Models
{
    public class UpdatePostModel
    {
        public long Id { get; set; }
        public long PostStatusId { get; set; }
        public bool IsEssence { get; set; } = false;
    }
}
