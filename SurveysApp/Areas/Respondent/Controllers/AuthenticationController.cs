using SurveysApp.Forms;
using SurveysApp.Models;
using SurveysApp.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SurveysApp.Areas.Respondent.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly AppDbContext _db;

        public AuthenticationController()
        {
            _db = new AppDbContext();
        }


        // GET: Respondent/Authentication
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserSignInViewModel userSignInViewModel)
        {
          
            if(!ModelState.IsValid)
            {
                return View(userSignInViewModel);
            }
            
            var userExistsInDatabase = await _db.User.FirstOrDefaultAsync(m=>m.Email ==userSignInViewModel.Email);

            if (userExistsInDatabase != null)
            {
                TempData["StatusMessage"] = "Error - User already exists, use another email.";
                return View(userSignInViewModel);
            }

            User user = new User()
            {
                FirstName = userSignInViewModel.FirstName,
                LastName = userSignInViewModel.LastName,
                Gender = userSignInViewModel.Gender.ToString(),
                Email = userSignInViewModel.Email,
                Password = userSignInViewModel.Password,
                Role = "Admin"
            };


            _db.User.Add(user);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Survey", new { area = "Respondent" });

            /*return View(userSignInViewModel);*/
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userLoginViewModel);
            }

            if (!UserManager.Validate(userLoginViewModel, Response))
            {
                TempData["StatusMessage"] = "Error - Credential are wrong!";
                return View(userLoginViewModel);
            }

            return RedirectToAction("Index", "Survey", new { area = "Respondent" });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session.Abandon();


            FormsAuthentication.SignOut();
            HttpCookie myHttpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            myHttpCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(myHttpCookie);
            return RedirectToAction("Login");
        }
    }
}