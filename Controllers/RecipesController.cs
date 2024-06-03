using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project.Models;
using First_Project.Enums;
using System.Text;
using First_Project.DTOs;

namespace First_Project.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecipesController(ModelContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _context = context;
            this._webHostEnvironment = _webHostEnvironment;
        }

        public async Task<IActionResult> Index(string keyword, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Recipe> query = _context.Recipes;

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(r => r.Title.Contains(keyword) || r.Description.Contains(keyword));
            }

            if (startDate.HasValue)
            {
                query = query.Where(r => r.CreationDate >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.CreationDate <= endDate.Value.Date);
            }

            var recipes = await query.Where(y=>y.Status != RecipeEnum.Pending.ToString()).ToListAsync();
            return View(recipes);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "ID", "CategoryName");
            ViewData["Users"] = new SelectList(_context.Users, "ID", "Username");
            ViewData["StatusEnums"] = Enum.GetNames(typeof(RecipeEnum)).Select(e => new SelectListItem()
            {
                Text = e,
                Value = e
            });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CreationDate,Title,Description,Instructions,Price,UserId,CategoryId,Status,ImageFile")] Recipe recipe)
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
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categories"] = new SelectList(_context.Categories, "ID", "CategoryName");
            ViewData["Users"] = new SelectList(_context.Users, "ID", "Username");
            ViewData["StatusEnums"] = Enum.GetNames(typeof(RecipeEnum)).Select(e => new SelectListItem()
            {
                Text = e,
                Value = e
            }); return View(recipe);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = new SelectList(_context.Categories, "ID", "CategoryName");
            ViewData["Users"] = new SelectList(_context.Users, "ID", "Username");
            ViewData["StatusEnums"] = Enum.GetNames(typeof(RecipeEnum)).Select(e => new SelectListItem()
            {
                Text = e,
                Value = e
            });
            return View(recipe);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CreationDate,ID,Title,Description,Instructions,Price,UserId,CategoryId,Status,ImageFile")] Recipe recipe)
        {
            if (id != recipe.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (recipe.ImageFile != null && recipe.ImageFile.Length > 0)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string imageName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(recipe.ImageFile.FileName);
                        string fullPath = Path.Combine(wwwrootPath, "Images", imageName);

                        recipe.Image = imageName;

                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await recipe.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categories"] = new SelectList(_context.Categories, "ID", "CategoryName");
            ViewData["Users"] = new SelectList(_context.Users, "ID", "Username");
            ViewData["StatusEnums"] = Enum.GetNames(typeof(RecipeEnum)).Select(e => new SelectListItem()
            {
                Text = e,
                Value = e
            }); return View(recipe);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportToCSV()
        {
            var recipes = await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.Category)
                .Select(r => new RecipeReportDto
                {
                    Title = r.Title,
                    Description = r.Description,
                    Instructions = r.Instructions,
                    Price = r.Price,
                    Username = r.User.Username,
                    CategoryName = r.Category.CategoryName
                }).ToListAsync();

            var csvContent = new StringBuilder();
            csvContent.AppendLine("Title,Description,Instructions,Price,Username,CategoryName");
            foreach (var recipe in recipes)
            {
                csvContent.AppendLine($"{recipe.Title},{recipe.Description},{recipe.Instructions},{recipe.Price},{recipe.Username},{recipe.CategoryName}");
            }
            return File(Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", "recipes.csv");
        }

        public async Task<IActionResult> ExportToCSVSoldRecipes()
        {
            var recipes = await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.Category)
                .Where(x=> x.isSold == true)
                .Select(r => new RecipeReportDto
                {
                    Title = r.Title,
                    Description = r.Description,
                    Instructions = r.Instructions,
                    Price = r.Price,
                    Username = r.User.Username,
                    CategoryName = r.Category.CategoryName
                }).ToListAsync();

            var csvContent = new StringBuilder();
            csvContent.AppendLine("Title,Description,Instructions,Price,Username,CategoryName");
            foreach (var recipe in recipes)
            {
                csvContent.AppendLine($"{recipe.Title},{recipe.Description},{recipe.Instructions},{recipe.Price},{recipe.Username},{recipe.CategoryName}");
            }
            return File(Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", "recipes.csv");
        }

        public IActionResult GetAllRecipeisForUser()
        {
            if (!CheckUser())
                return RedirectToAction("Login", "LoginAndRegister");

            return View(_context.Recipes.Include(x=>x.User).Include(x=>x.Category).ToList());
        }

        [HttpPost]
        public IActionResult SubmitTestimonial(string content, int recipeId)
        {
            _context.Testimonials.Add(new Testimonial() 
            {
                Content = content,
                Dateposted = DateTime.UtcNow,
                RecipeId = recipeId,
                UserId = HttpContext.Session.GetInt32("LoggedUser").Value,
                isActive = false
            });
            _context.SaveChanges();
            return RedirectToAction("Index", "Users"); 
        }

        private bool RecipeExists(int id)
        {
            return (_context.Recipes?.Any(e => e.RecipeId == id)).GetValueOrDefault();
        }

        private bool CheckUser()
        {
            if (HttpContext.Session.GetInt32("LoggedUser") != null)
                return true;

            return false;
        }
    }
}
