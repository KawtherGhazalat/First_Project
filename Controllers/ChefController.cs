using First_Project.Enums;
using First_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace First_Project.Controllers
{
    public class ChefController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ChefController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(string? recipeKeyword, string? categoryKeyword)
        {
            if (!CheckChef())
                return RedirectToAction("Login", nameof(LoginAndRegisterController));


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

            var recipes = recipesQuery.Where(x=>x.Status != RecipeEnum.Pending.ToString()).ToList();
            var categories = categoriesQuery.ToList();

            ViewBag.Recipes = recipes;
            ViewBag.Categories = categories;

            return View();
        }

        public IActionResult MyRecipes()
        {
            if (CheckChef())
                return View(_context.Recipes.Where(x => x.UserId == HttpContext.Session.GetInt32("LoggedChef").Value && x.Status != RecipeEnum.Pending.ToString()).Include(x => x.User).Include(x => x.Category).ToList());
            else return RedirectToAction("Login", nameof(LoginAndRegisterController));
        }
        public IActionResult ChefsRecipes()
        {
            if (CheckChef())
                return View(_context.Recipes.Where(x => x.UserId != HttpContext.Session.GetInt32("LoggedChef").Value && x.Status != RecipeEnum.Pending.ToString()).Include(x => x.User).Include(x => x.Category).ToList());
            else return RedirectToAction("Login", nameof(LoginAndRegisterController));

        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "ID", "CategoryName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CreationDate,Title,Description,Instructions,Price,CategoryId,Status,ImageFile")] Recipe recipe)
        {
            if (recipe.ImageFile != null && recipe.ImageFile.Length > 0)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string imageName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(recipe.ImageFile.FileName);
                string fullPath = Path.Combine(wwwrootPath, "Images", imageName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await recipe.ImageFile.CopyToAsync(fileStream);
                }

                recipe.Image = imageName;
            }

            if (ModelState.IsValid)
            {
                recipe.UserId = HttpContext.Session.GetInt32("LoggedChef").Value;
                recipe.Status = RecipeEnum.Pending.ToString();
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categories"] = new SelectList(_context.Categories, "ID", "CategoryName");
            ViewData["StatusEnums"] = Enum.GetNames(typeof(RecipeEnum)).Select(e => new SelectListItem()
            {
                Text = e,
                Value = e
            }); return View(recipe);
        }

        private bool CheckChef()
        {
            if (HttpContext.Session.GetInt32("LoggedChef") != null)
                return true;

            return false;
        }
    }
}
