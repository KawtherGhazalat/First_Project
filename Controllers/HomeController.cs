using First_Project.Enums;
using First_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index(string? recipeKeyword, string? categoryKeyword)
        {
            var recipesQuery = _context.Recipes.AsQueryable();
            var categoriesQuery = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(recipeKeyword))
            {
                recipesQuery = recipesQuery.Where(r => r.Title.ToLower().Contains(recipeKeyword.ToLower()) || r.Description.ToLower().Contains(recipeKeyword.ToLower()));
            }

            if (!string.IsNullOrEmpty(categoryKeyword))
            {
                categoriesQuery = categoriesQuery.Where(c => c.CategoryName.ToLower().Contains(categoryKeyword.ToLower()));
            }

            var recipes = recipesQuery.Where(x => x.Status != RecipeEnum.Pending.ToString()).ToList();
            var categories = categoriesQuery.ToList();

            ViewBag.Recipes = recipes;
            ViewBag.Categories = categories;

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

        public IActionResult AddToCart(int id)
        {
            if (CheckUser())
                return RedirectToAction("Index", "Menu", new { reipcyId = id });

            return RedirectToAction("Login", "LoginAndRegister");
        }

        public IActionResult ViewCategoryRecipes(int id)
        {
            if (CheckUser())
            {
                var recipes = _context.Recipes.Where(r => r.CategoryId == id)
                    .Include(x=>x.User)
                    .Include(x=>x.Category).ToList();
                return View(recipes);
            }
            return RedirectToAction("Login", "LoginAndRegister");

        }


        private bool CheckUser()
        {
            if (HttpContext.Session.GetInt32("LoggedUser") != null )
                return true;

            return false;
        }

    }
}
