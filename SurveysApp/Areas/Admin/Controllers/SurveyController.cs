using SurveysApp.Models;
using SurveysApp.Forms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SurveysApp.Areas.Admin.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();


        
        // GET: Admin/Survey
        public async Task<ActionResult> Index()
        {
            var surveys = await _db.Survey.Include(m => m.Category).Where(m=>m.Admin.Email==User.Identity.Name).ToListAsync();

            return View(surveys);
        }

        public async Task<ActionResult> Create()
        {

            SurveyCategoryQuestionViewModel surveyCategoryQuestionVM = new SurveyCategoryQuestionViewModel()
            {
                Survey = new Survey(),
                CategoryList  = await _db.Category.ToListAsync(),
            };

            return View(surveyCategoryQuestionVM);
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SurveyCategoryQuestionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {

                viewModel.CategoryList = await _db.Category.ToListAsync();

                return View(viewModel);
            }

            TempData["StatusMessage"] = "Survey created successfully";

            var admin = _db.User.FirstOrDefault(m=>m.Email==User.Identity.Name);

            viewModel.Survey.AdminId = admin.Id;

            _db.Survey.Add(viewModel.Survey);
            await _db.SaveChangesAsync();

            //return RedirectToAction(nameof(Index));
            return RedirectToAction("AddQuestion", new { surveyId = viewModel.Survey.Id });
        }

        // GET - CREATE QUESTIONS
        public async Task<ActionResult> AddQuestion(int? surveyId)
        {
            if (surveyId == null)
            {
                return HttpNotFound();
            }

            var survey = await _db.Survey.Include(m => m.Questions).SingleOrDefaultAsync(m => m.Id == surveyId);

            if (survey == null)
            {
                return HttpNotFound();
            }

            var viewModel = new SurveyQuestionViewModel()
            {
                Survey = survey,
                Question = new Question()
                {
                    SurveyId = survey.Id
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("AddQuestion")]
        public async Task<ActionResult> AddQuestion(Question question)
        {
            if (ModelState.IsValid == false)
            {

                var viewModel = new SurveyQuestionViewModel()
                {
                    Survey = await _db.Survey.Include(m => m.Questions).SingleOrDefaultAsync(m => m.Id == question.SurveyId),
                    Question = question
                };

                return View(viewModel);
            }

            _db.Question.Add(question);
            await _db.SaveChangesAsync();
            TempData["StatusMessage"] = "Question created successfully";

            return RedirectToAction("AddQuestion", new { surveyId = question.SurveyId});
        }

        //[ActionName("Edit")]
        //public async Task<ActionResult> Edit(SurveyCategoryQuestionViewModel viewModel)
        //{
        //    return View(viewModel);
        //}

        //GET - EDIT
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var survey = await _db.Survey.Include(s => s.Questions).FirstAsync(s => s.Id == id);

            if (survey == null)
            {
                return HttpNotFound();
            }

            var viewModel = new SurveyCategoriesViewModel
            {
                Survey = survey,
                CategoryList = await _db.Category.ToListAsync(),
            };

            return View(viewModel);
        }
        
        //POST -- EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SurveyCategoriesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {                               
                /* Reload category list*/
                viewModel.CategoryList = await _db.Category.ToListAsync();
                return View(viewModel);

            }
            //_db.Entry(viewModel.Survey).State = EntityState.Modified; // Update survey

            var surveyFromDatabase = await _db.Survey.FindAsync(viewModel.Survey.Id);
            surveyFromDatabase.Name = viewModel.Survey.Name;
            surveyFromDatabase.Status = viewModel.Survey.Status;
            surveyFromDatabase.CategoryId = viewModel.Survey.CategoryId;

            // Update questions
            if (viewModel.Survey.Questions != null)
            {
                foreach (var question in viewModel.Survey.Questions)
                {

                    _db.Entry(question).State = EntityState.Modified;
                }
            }


            await _db.SaveChangesAsync();

            TempData["StatusMessage"] = "Survey updated successfully";

            return RedirectToAction(nameof(Index));
        }

        // GET - DELETE QUESTION
        public async Task<ActionResult> DeleteQuestion(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var question = await _db.Question.Include(m => m.Survey).SingleOrDefaultAsync(m => m.Id == id);

            if (question == null)
            {
                return HttpNotFound();
            }

            return View(question);
        }

        // POST - DELETE QUESTION
        [HttpPost]
        [ActionName("DeleteQuestion")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            if (id <= 0)
            {
                return HttpNotFound();
            }

            var question = await _db.Question.SingleOrDefaultAsync(m => m.Id == id);

            if (question == null)
            {
                return HttpNotFound();
            }

            _db.Question.Remove(question);
            await _db.SaveChangesAsync();
            TempData["StatusMessage"] = "Question deleted successfully";

            return RedirectToAction(nameof(Edit), new { id = question.SurveyId });
        }

        //GET - DELETE (SURVEY)
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteSurvey(int? id)
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
            return View(survey);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteSurveyConfirm(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            TempData["StatusMessage"] = "Survey deleted successfully";

            var survey = await _db.Survey.FindAsync(id);

            if (survey == null)
            {
                return HttpNotFound();
            }

            _db.Survey.Remove(survey);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Details(int id, int? userId)
        {
            var survey = await _db.Survey
                .Include(m => m.Questions).SingleOrDefaultAsync(m => m.Id == id);

            if (survey == null)
            {
                return HttpNotFound();
            }
            
            if (userId != null) {
                
                // retrieve questions and answer for the survey
                SurveyAndAnswerListViewModel surveyAndAnswerListViewModel = new SurveyAndAnswerListViewModel()
                {
                    Survey = survey,
                    AnswerList = await _db.Answer.Where(answer => answer.UserId == userId)
                        .Join(_db.Question.Where(m => m.SurveyId == id), a => a.QuestionId, q => q.Id, (a, q) => a)
                        .ToListAsync()
                    //AnswerList = await _db.Answer.Where(a=>a.Question.SurveyId==id && a.UserId==userId).Select(a=>a).ToListAsync()
                };


                return View("DetailsByUser", surveyAndAnswerListViewModel);
            }
            else
            {             
                
                // Retrive the users that has answered the survey
                var users = await
                    _db.Survey.Where(m => m.Id == id)
                    .Join(_db.Question, s => s.Id, q => q.SurveyId, (s, q) => new { QuestionId = q.Id })
                    .Join(_db.Answer, q => q.QuestionId, a => a.QuestionId, (q, a) => new { a.UserId })
                    .GroupBy(a => a.UserId)
                    .Join(_db.User, a => a.Key, u => u.Id, (a, u) => u)
                    .ToListAsync();

                return View(users);
            }

        }
    }
}