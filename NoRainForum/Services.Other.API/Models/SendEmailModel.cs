using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Other.API.Models
{
    public class SendEmailModel
    {
        [Required(ErrorMessage ="邮箱不能为空")]
        [EmailAddress(ErrorMessage = "email格式错误")]
        public string RecipientEmail { get; set; }

        public SendType SendType { get; set; }
    }
}
