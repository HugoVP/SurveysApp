using SurveysApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysApp.Forms
{
    public class SurveyCategoryQuestionViewModel
    {
        public Survey Survey { get; set; }
        public IEnumerable<Question> QuestionList { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }
    }
}