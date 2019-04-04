using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveysApp.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Survey")]
        public int SurveyId { get; set; }

        [ForeignKey("SurveyId")]
        public Survey Survey { get; set; }

        [Required]
        public string Content { get; set; }
    }
}