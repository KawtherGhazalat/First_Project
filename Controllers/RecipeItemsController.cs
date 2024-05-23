using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project.Models;

namespace First_Project.Controllers
{
    public class RecipeItemsController : Controller
    {
        private readonly ModelContext _context;

        public RecipeItemsController(ModelContext context)
        {
            _context = context;
        }

        // GET: RecipeItems
        public async Task<IActionResult> Index()
        {
              return _context.RecipeItems != null ? 
                          View(await _context.RecipeItems.ToListAsync()) :
                          Problem("Entity set 'ModelContext.RecipeItems'  is null.");
        }

        // GET: RecipeItems/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.RecipeItems == null)
            {
                return NotFound();
            }

            var recipeItem = await _context.RecipeItems
                .FirstOrDefaultAsync(m => m.Recipeitemid == id);
            if (recipeItem == null)
            {
                return NotFound();
            }

            return View(recipeItem);
        }

        // GET: RecipeItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RecipeItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Recipeitemid,Recipeid,Ingredient,Quantity,Unit")] RecipeItem recipeItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipeItem);
        }

        // GET: RecipeItems/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.RecipeItems == null)
            {
                return NotFound();
            }

            var recipeItem = await _context.RecipeItems.FindAsync(id);
            if (recipeItem == null)
            {
                return NotFound();
            }
            return View(recipeItem);
        }

        // POST: RecipeItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Recipeitemid,Recipeid,Ingredient,Quantity,Unit")] RecipeItem recipeItem)
        {
            if (id != recipeItem.Recipeitemid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeItemExists(recipeItem.Recipeitemid))
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
            return View(recipeItem);
        }

        // GET: RecipeItems/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.RecipeItems == null)
            {
                return NotFound();
            }

            var recipeItem = await _context.RecipeItems
                .FirstOrDefaultAsync(m => m.Recipeitemid == id);
            if (recipeItem == null)
            {
                return NotFound();
            }

            return View(recipeItem);
        }

        // POST: RecipeItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.RecipeItems == null)
            {
                return Problem("Entity set 'ModelContext.RecipeItems'  is null.");
            }
            var recipeItem = await _context.RecipeItems.FindAsync(id);
            if (recipeItem != null)
            {
                _context.RecipeItems.Remove(recipeItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeItemExists(decimal id)
        {
          return (_context.RecipeItems?.Any(e => e.Recipeitemid == id)).GetValueOrDefault();
        }
    }
}
