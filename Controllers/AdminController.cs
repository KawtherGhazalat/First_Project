using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using First_Project.Models;
using First_Project.DTOs;

namespace First_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
         public AdminController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!CheckAdmin())
                return RedirectToAction("Login", "LoginAndRegister");

            DashboardCounter counter = new DashboardCounter()
            {
                Users = _context.Users.Where(x => x.RoleId == 2).ToList().Count(),
                Chefs = _context.Users.Where(x => x.RoleId == 3).ToList().Count(),
                Recipes = _context.Recipes.ToList().Count(),
                SoldRecipes = _context.Recipes.Where(x=>x.isSold == true).ToList().Count()
            };

            ViewBag.Recipes = _context.Recipes.Include(x=>x.User).Include(x=>x.Category).Where(x => x.isSold == true).ToList(); 

            return View(counter);

        }

        private bool CheckAdmin ()
        {
            if (HttpContext.Session.GetInt32("LoggedAdmin") != null)
                return true;

            return false;

        }
    }
}
