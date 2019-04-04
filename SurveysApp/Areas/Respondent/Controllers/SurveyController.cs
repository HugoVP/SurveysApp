using SurveysApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SurveysApp.Forms;

namespace SurveysApp.Areas.Respondent.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly AppDbContext _db;

        public SurveyController()
        {
            _db = new AppDbContext();
        }

        // GET: Respondent/Survey
        public async Task<ActionResult> Index()
        {
            // Retrieve user from coockie
            var user = await _db.User.FirstOrDefaultAsync(m => m.Email == User.Identity.Name);

            
            // Redirect to login if user does not exist
            if (user == null)
            {
                return RedirectToAction("Login", "Authentication", new { area = "Respondent" });
            }

            // Retrieve all answered surveys by the user
            var answeredSurveys = await _db.Answer.Where(a => a.UserId == user.Id)
                .Join(
                    _db.Question,
                    answer => answer.QuestionId,
                    question => question.Id,
                    (answer, question) => new
                    {
                        question.SurveyId
                    }
                )
                .GroupBy(m => m.SurveyId)
                .Join(
                    _db.Survey,
                    answer => answer.Key,
                    survey => survey.Id,
                    (answer, survey) => survey
                )
                .ToListAsync();


            // Retrive all active survyes
            IEnumerable<Survey> surveys = await _db.Survey.Where(m => m.Status == Survey.EStatus.Active).ToListAsync();

            // Extract answered surveys from active surveys
            var newSurveys = surveys.Except(answeredSurveys);

            return View(newSurveys);
        }

        // GET - Answeing
        public async Task<ActionResult> Answering(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            // Retrieve survey by id and include its questions
            var survey = await _db.Survey.Include(m=>m.Questions).SingleOrDefaultAsync(m=>m.Id == id);

            if (survey == null)
            {
                return HttpNotFound();
            }

            // Create viewModel
            var viewModel = new SurveyAndAnswerListViewModel()
            {
                Survey = survey,

            };

            return View(viewModel);
        }

        // POST - Answer
        [HttpPost]

        [ValidateAntiForgeryToken]
        [ActionName("Answering")]
        public async Task<ActionResult> Answer(int? id, SurveyAndAnswerListViewModel viewModel)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            // Retrieve survey with questions
            var survey = await _db.Survey.Include(m => m.Questions).SingleOrDefaultAsync(m => m.Id == id);


            if (survey == null)
            {
                return HttpNotFound();
            }

            // View Model validation
            if (!ModelState.IsValid)
            {
                // Set survey on viewModel
                viewModel.Survey = survey;

                return View("Answering", viewModel);
            }

            // Retrieve user from coockie
            var user = await _db.User.SingleOrDefaultAsync(m=>m.Email == User.Identity.Name);

            // Redirect to login if user does not exist
            if (user == null)
            {
                return RedirectToAction("Login", "Authentication", new { area = "Respondent" });
            }

            // Redirect to Index if there are not answers
            if (viewModel.AnswerList == null)
            {
                return RedirectToAction("Index");
            }

            // Save answers to database
            foreach (var answer in viewModel.AnswerList)
            {
                answer.UserId = user.Id;
                _db.Answer.Add(answer);
            }

            TempData["StatusMessage"] = "Survey answered.";

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //GET - Answered
        public async Task<ActionResult> Answered()
        {
            var user = await _db.User.FirstOrDefaultAsync(m => m.Email == User.Identity.Name);

            // Redirect to login if user does not exist
            if (user == null)
            {
                return RedirectToAction("Login", "Authentication", new { area = "Respondent" });
            }
            // Retrieve all answered surveys
            var answeredSurveys = await _db.Answer.Where(a => a.UserId == user.Id)
                .Join(
                    _db.Question,
                    answer => answer.QuestionId,
                    question => question.Id,
                    (answer, question) => new
                    {
                        question.SurveyId
                    }
                )
                .GroupBy(m => m.SurveyId)
                .Join(
                    _db.Survey,
                    answer => answer.Key,
                    survey => survey.Id,
                    (answer, survey) => survey
                )
                .ToListAsync();

            return View(answeredSurveys);
        }

        //GET - DETAILS
        [ActionName("Details")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var survey = await _db.Survey.Include(m => m.Questions).SingleOrDefaultAsync(m => m.Id == id);


            if (survey == null)
            {
                return HttpNotFound();
            }

            var user = await _db.User.FirstOrDefaultAsync(m => m.Email == User.Identity.Name);

            var viewModel = new SurveyAndAnswerListViewModel()
            {
                Survey = survey,
                AnswerList = await _db.Answer.Where(a => a.UserId == user.Id).Join(_db.Question.Where(q => q.SurveyId == survey.Id), a => a.QuestionId, q => q.Id, (a, q) => a).ToListAsync()
            };

            return View(viewModel);

        }
        
    }
}