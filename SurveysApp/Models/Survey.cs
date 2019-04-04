using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveysApp.Models
{
    public class Survey
    {
        public Survey()
        {
            DischargeTime = DateTime.Now;
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DischargeTime { get; set; }

        [Required]
        [DefaultValue(EStatus.Active)]
        public EStatus Status { get; set; }

        public enum EStatus { Active = 0, Inactive = 1, Archived = 2 }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        [Display(Name = "Admin")]
        public int AdminId { get; set; }

        [ForeignKey("AdminId")]
        public virtual User Admin { get; set; }

        [Display(Name = "Question")]
        public ICollection<Question> Questions { get; set; }
    }
}