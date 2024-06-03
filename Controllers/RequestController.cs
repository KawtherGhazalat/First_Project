using First_Project.Enums;
using First_Project.Models;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace First_Project.Controllers
{
    public class RequestController : Controller
    {
        private readonly ModelContext _context;

        public RequestController(ModelContext _context)
        {
            this._context = _context;
        }
        public ActionResult Index(string? keyword)
        {
            return View(_context.Recipes
                .Include(x=>x.User)
                .Include(x=>x.Category)
                .Where(x => x.Status == RecipeEnum.Pending.ToString()));
        }

        public ActionResult AcceptRecipeRequest(int recipeId)
        {
            var recipe = _context.Recipes.FirstOrDefault(x => x.ID == recipeId);
            recipe.Status = RecipeEnum.Accepted.ToString();
            _context.Recipes.Update(recipe);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public ActionResult RejectRecipeRequest(int recipeId)
        {
            var recipe = _context.Recipes.FirstOrDefault(x => x.ID == recipeId);
            recipe.Status = RecipeEnum.Rejected.ToString();
            _context.Recipes.Update(recipe);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
