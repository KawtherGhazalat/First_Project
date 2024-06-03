using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project.Models;



namespace First_Project.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfilesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.Profiles.Include(x => x.User).ToListAsync();
            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        public IActionResult Create()
        {
            ViewData["Users"] = new SelectList(_context.Users.Where(x=>x.ProfileId == 0 ), "ID", "Username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,RoleName,Image,Bio,ImageFile,CreationDate")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                if (profile.ImageFile != null && profile.ImageFile.Length > 0)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(profile.ImageFile.FileName);
                    string fullPath = Path.Combine(wwwrootPath, "Images", imageName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await profile.ImageFile.CopyToAsync(fileStream);
                    }

                    profile.Image = imageName;
                }

                var createdProfile =  _context.Add(profile);
                await _context.SaveChangesAsync();

                var user = _context.Users.Find(profile.UserId);
                user.ProfileId = createdProfile.Entity.ID;
                 _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Users"] = new SelectList(_context.Users.Where(x => x.ProfileId == 0), "ID", "Username");
            return View(profile);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            ViewData["Users"] = new SelectList(_context.Users.Where(x => x.ProfileId == 0 || x.ProfileId == id), "ID", "Username");
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserId,Username,RoleName,Image,Bio,ImageFile,CreationDate,Image")] Profile profile)
        {
            if (id != profile.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (profile.ImageFile != null && profile.ImageFile.Length > 0)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string imageName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(profile.ImageFile.FileName);
                        string fullPath = Path.Combine(wwwrootPath, "Images", imageName);

                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await profile.ImageFile.CopyToAsync(fileStream);
                        }

                        profile.Image = imageName;
                    }

                    _context.Update(profile);
                    await _context.SaveChangesAsync();

                    var user = _context.Users.Find(profile.UserId);
                    user.ProfileId = profile.ID;

                    _context.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.ID))
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

            ViewData["Users"] = new SelectList(_context.Users.Where(x => x.ProfileId == 0 || x.ProfileId == id), "ID", "Username");
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfileModal(int id, [Bind("ID,UserId,Username,RoleName,Image,Bio,ImageFile,CreationDate,Image")] Profile profile)
        {
            if (id != profile.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (profile.ImageFile != null && profile.ImageFile.Length > 0)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string imageName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(profile.ImageFile.FileName);
                        string fullPath = Path.Combine(wwwrootPath, "Images", imageName);

                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await profile.ImageFile.CopyToAsync(fileStream);
                        }

                        profile.Image = imageName;
                    }

                    _context.Update(profile);
                    await _context.SaveChangesAsync();

                    var user = _context.Users.Find(profile.UserId);
                    user.ProfileId = profile.ID;

                    _context.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (HttpContext.Session.GetInt32("LoggedChef").HasValue )
                {
                    return RedirectToAction(nameof(GetProfileChef));
                }
                else if (HttpContext.Session.GetInt32("LoggedUser").HasValue)
                {
                    return RedirectToAction(nameof(GetProfileUser));
                }
                else 
                {
                    return RedirectToAction(nameof(Index));

                }
            }

            ViewData["Users"] = new SelectList(_context.Users.Where(x => x.ProfileId == 0 || x.ProfileId == id), "ID", "Username");
            return View(profile);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            var user = _context.Users.FirstOrDefault(x => x.ProfileId == id);
            user.ProfileId = 0;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult GetProfileChef()
        {
            int chefId = HttpContext.Session.GetInt32("LoggedChef").Value;
            var profile = _context.Profiles.Include(x => x.User).FirstOrDefault(x => x.UserId == chefId);
            ViewData["Users"] = new SelectList(_context.Users.Where(x => x.ProfileId == 0 || x.ProfileId == profile.ID), "ID", "Username");

            return View(profile);
        }
        public ActionResult GetProfileUser()
        {
            int userId = HttpContext.Session.GetInt32("LoggedUser").Value;
            var profile = _context.Profiles.Include(x=>x.User).FirstOrDefault(x => x.UserId == userId);
            var list = _context.Users.Where(x => x.ProfileId == 0 || x.ProfileId == profile.ID).ToList();
            ViewData["Users"] = new SelectList(list, "ID", "Username");

            return View(profile);
        }
        public ActionResult GetProfileAdmin()
        {
            int adminId = HttpContext.Session.GetInt32("LoggedAdmin").Value; 
            var profile = _context.Profiles.Include(x => x.User).FirstOrDefault(x => x.UserId == adminId);
            ViewData["Users"] = new SelectList(_context.Users.Where(x => x.ProfileId == 0 || x.ProfileId == profile.ID), "ID", "Username");

            return View(profile);
        }

        private bool ProfileExists(int id)
        {
            return (_context.Profiles?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
