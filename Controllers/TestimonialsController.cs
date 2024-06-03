using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project.Models;
using First_Project.Enums;

namespace First_Project.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Testimonials.Where(x=> x.isActive == false).Include(t => t.User).ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }


        public IActionResult Create()
        {
            ViewData["Users"] = new SelectList(_context.Users, "ID", "Username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Testimonialid,UserId,Content,Dateposted")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Users"] = new SelectList(_context.Users, "ID", "Username");
            return View(testimonial);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }

            ViewData["Users"] = new SelectList(_context.Users, "ID", "Username");
            return View(testimonial);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Testimonialid,UserId,Content,Dateposted")] Testimonial testimonial)
        {
            if (id != testimonial.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.ID))
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
            ViewData["Users"] = new SelectList(_context.Users, "ID", "Username");
            return View(testimonial);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .FirstOrDefaultAsync(m => m.ID == id);

            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }
        public ActionResult AcceptTestimonual(int testimonialId)
        {
            var testimonial = _context.Testimonials.FirstOrDefault(x => x.ID == testimonialId);
            testimonial.isActive = true;
            _context.Testimonials.Update(testimonial);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public ActionResult RejectTestimonual(int testimonialId)
        {
            var testimonial = _context.Testimonials.FirstOrDefault(x => x.ID == testimonialId);
            _context.Testimonials.Remove(testimonial);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }

            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        private bool TestimonialExists(int id)
        {
            return (_context.Testimonials?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
