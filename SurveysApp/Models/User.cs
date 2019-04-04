using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SurveysApp.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        public enum EGender { Male = 0, Female = 1 }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        [Index(IsUnique=true)]
        public string Email { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 chars", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public enum ERole { Admin = 0, Respondent = 1 }
    }
}