using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurveysApp.Forms
{
    public class UserLoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 chars", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string StatusMessage { get; set; }
    }
}