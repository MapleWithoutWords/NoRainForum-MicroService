using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Post.API.Models
{
    public class UpdatePostModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long PostStatusId { get; set; }
        [Required]
        public bool IsEssence { get; set; } = false;
    }
}
