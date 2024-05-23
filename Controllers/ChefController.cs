using First_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace First_Project.Controllers
{
    public class ChefController : Controller
    {
        private readonly ModelContext _context;
        public ChefController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var rec = _context.Recipes.Where(c=> c.Userid == HttpContext.Session.GetInt32("Chefid") ).ToList();
            return View(rec);
        }

        public IActionResult AddRes()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddRes(Recipe recipe)
        {
            recipe.Userid = HttpContext.Session.GetInt32("Chefid");
            _context.Add(recipe);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
