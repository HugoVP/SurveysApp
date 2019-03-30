using SurveysApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SurveysApp.Areas.Admin.Controllers
{
    //[Authorize]
    public class CategoryController : Controller
    {

        private readonly Models.AppDbContext _db = new AppDbContext();

        // GET: Admin/Category
        public async Task<ActionResult> Index()
        {
            var categories = await _db.Category.ToListAsync();

            return View(categories);
        }

        //[AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _db.Category.Add(category);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET - EDIT
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var category = await _db.Category.FindAsync(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var categoryFromDb = await _db.Category.FindAsync(category.Id);

            if (categoryFromDb == null)
            {
                return HttpNotFound();
            }

            categoryFromDb.Name = category.Name;

            //_db.Update(category);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET - DELETE
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var category = await _db.Category.FindAsync(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // DELETE - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            Category category = await _db.Category.FindAsync(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            _db.Category.Remove(category);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET - DETAILS
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var category = await _db.Category.FindAsync(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }
    }
}