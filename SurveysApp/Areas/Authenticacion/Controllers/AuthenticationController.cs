using SurveysApp.Models;
using SurveysApp.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace SurveysApp.Areas.Authentication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();
        // GET: Respondent/Authentication
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }


        public ActionResult Login()
        {
            return View();
        }

        // POST - LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel userLoginViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(userLoginViewModel);
            }

            // Check user in database

            return RedirectToAction(nameof(Index));
        }

        // GET - REGISTER
        public ActionResult Register()
        {
            return View();
        }        


        [HttpPost]
        public async Task<ActionResult> Register(UserSignInViewModel userSignInViewModel)
        {

            if (!ModelState.IsValid)
            {

                return View(userSignInViewModel);
            }

            //var user = new User()
            //{
            //    Email = userSignInViewModel.Email,
            //    Gender = userSignInViewModel.Gender,

            //    FirstName = userSignInViewModel.FirstName,
            //    LastName = userSignInViewModel.LastName,
            //    Role = "Respondent",
            //    Password = userSignInViewModel.Password,
            //};


            _db.User.Add(userSignInViewModel.User);

            await _db.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult Logout()
        {
            return RedirectToAction(nameof(Index));
        }
    }
}