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
            DashboardCounter counter = new DashboardCounter()
            {
                Users = _context.Users.Where(x => x.Roleid == 2).ToList().Count(),
                Chefs = _context.Users.Where(x => x.Roleid == 3).ToList().Count(),
                Recipes = _context.Recipes.ToList().Count()
            };
            return View(counter);
        }

        //public IActionResult JoinTables()
        //{
        //    var Categories = _context.Categories.ToList();
        //    var recipes = _context.Recipes.ToList();
        //    var users = _context.Users.ToList();
        //    var CategoryRecipe = _context.CategoryRecipe.ToList();

        //    var result = from u in users
        //                 join cr in CategoryRecipe on u.Id equals cr.CustomerId
        //                 join r in recipes on cr.ProductId equals r.Id
        //                 join cat in Categories on r.CategoryId equals cat.Id
        //                 select new JoinTables { users = u, Category = cat, Recipe = r, CategoryRecipe = cr };
        //    return View(result);
        //}
    }
}
