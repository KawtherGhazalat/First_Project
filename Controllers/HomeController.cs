using First_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace First_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var Recipe = _context.Recipes.ToList();
            ViewBag.Recipes = Recipe;

            var Category = _context.Categories.ToList();
            ViewBag.Category = Category;

            return View();
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult recipe(decimal id)
        {
            var recipy = _context.Recipes.Where(r => r.Recipeid == id).FirstOrDefault();
            return View(recipy);
        }

        public IActionResult Category(decimal id)
        {
            var categories = _context.Categories.Where(r=> r.Categoryid == id).FirstOrDefault();
            return View(categories);
        }
    }
}
