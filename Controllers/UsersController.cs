using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project.Models;
using First_Project.DTOs;
using System.Text.Json;
using First_Project.Enums;

namespace First_Project.Controllers
{
    public class UsersController : Controller
    {
        private readonly ModelContext _context;

        public UsersController(ModelContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string? recipeKeyword, string? categoryKeyword)
        {
            if (!CheckUser())
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

            var categories = categoriesQuery.ToList();

            var recipes = recipesQuery.Where(x => x.Status != RecipeEnum.Pending.ToString()).ToList();
            var testamonials = _context.Testimonials.Where(x => x.isActive == true).ToList();

            List<RecipeWithTestimonialsDTO>? recipeWithTestimonials = recipes.Select(recipe => new RecipeWithTestimonialsDTO
            {
                Recipe = recipe,
                Testimonials = testamonials.Where(t => t.RecipeId == recipe.ID).ToList()
            }).ToList();

            ViewBag.Recipes = recipeWithTestimonials;
            ViewBag.Categories = categories;

            return View();
        }
        public IActionResult Users(string? keyword)
        {
            if (CheckUser() || CheckAdmin())
                return View(_context.Users.Include(x => x.Role).ToList());

            return RedirectToAction("Login", nameof(LoginAndRegisterController));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public IActionResult Create()
        {
            if (CheckUser() || CheckAdmin())
            {
                ViewData["Roles"] = new SelectList(_context.Roles, "ID", "RoleName");
                return View();
            }
            return RedirectToAction("Login", nameof(LoginAndRegisterController));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Username,Email,Password,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                if(CheckUser())
                return RedirectToAction(nameof(Index));
                else return RedirectToAction(nameof(Users));

            }
            ViewData["Roles"] = new SelectList(_context.Roles, "ID", "RoleName");

            return RedirectToAction(nameof(Users));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Roles"] = new SelectList(_context.Roles, "ID", "RoleName");
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Username,Email,Password,RoleId")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Users));
            }
            ViewData["Roles"] = new SelectList(_context.Roles, "ID", "RoleName");
            return RedirectToAction(nameof(Users));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

          
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ModelContext.Users'  is null.");
            }
            var profile = _context.Profiles.FirstOrDefault(x => x.UserId == id);
            if (profile != null)
            {
                _context.Profiles.Remove(profile);
                await _context.SaveChangesAsync();
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }
        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.ID == id)).GetValueOrDefault();
        }
        public IActionResult GetTestimonials(int recipeId)
        {
            var testimonials = _context.Testimonials
                .Where(t => t.RecipeId == recipeId && t.isActive)
                .Select(t => new {
                    t.Content,
                    t.Dateposted
                }).ToList();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Use camelCase for property names
                WriteIndented = true, // Pretty print the JSON
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull // Ignore null values
            };

            return Json(testimonials, jsonOptions);
        }

        private bool CheckUser()
        {
            if (HttpContext.Session.GetInt32("LoggedUser") != null)
                return true;

            return false;
        }
        private bool CheckAdmin()
        {
            if (HttpContext.Session.GetInt32("LoggedAdmin") != null)
                return true;

            return false;
        }


    }
}
