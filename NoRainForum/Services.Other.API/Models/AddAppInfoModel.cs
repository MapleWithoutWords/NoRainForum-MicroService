using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Other.API.Models
{
    public class AddAppInfoModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
