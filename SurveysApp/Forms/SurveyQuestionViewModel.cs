using SurveysApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysApp.Forms
{
    public class SurveyQuestionViewModel
    {
        public Survey Survey { get; set; }
        public Question Question { get; set; }
    }
}