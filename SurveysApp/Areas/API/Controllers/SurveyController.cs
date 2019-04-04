using SurveysApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SurveysApp.Areas.API
{
    public class SurveyController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();
        // GET: API/Survey
        public async Task<ActionResult> Index(string category ="")
        {
            var surveys = await _db.Survey.Where(m => m.Status == Survey.EStatus.Active).ToListAsync();

            if (category != "")
            {
                surveys = surveys.Where(m=>m.Category.Name == category).ToList();
            }

            return Json(surveys, JsonRequestBehavior.AllowGet);
        }
    }
}