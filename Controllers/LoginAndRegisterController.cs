using First_Project.Enums;
using First_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace First_Project.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        private readonly ModelContext _context;
        public LoginAndRegisterController(ModelContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            ViewBag.UserType = new SelectList(_context.Roles.ToList(), "ID", "RoleName");

            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.Username == user.Username);
            if (existingUser == null)
            {
                if (ModelState.IsValid)
                {
                    user.Password = user.Password;
                    user.Email = user.Email;
                    user.RoleId = user.RoleId;
                    var createdUser = _context.Add(user);
                    _context.SaveChanges();

                    var profile = new Profile()
                    {
                        Image = "F:\\Downloads\\First_Project\\wwwroot\\Images\\chef2.jpeg",
                        UserId = createdUser.Entity.ID,
                        CreationDate = DateTime.UtcNow,
                        Bio = string.Empty
                    };
                    _context.Profiles.Add(profile);
                    _context.SaveChanges();

                    var modifiedUser = _context.Users.Find(createdUser.Entity.ID);
                    modifiedUser.ProfileId = profile.ID;
                    _context.Users.Update(modifiedUser);
                    _context.SaveChanges();


                    return RedirectToAction("Login"); // Redirect to login page after successful registration
                }
                else
                {
                    ViewBag.UserType = new SelectList(_context.Roles.ToList(), "ID", "RoleName");
                    ModelState.AddModelError("", "Username already exists");
                }
            }
           
            return View(user); // Return the user object to the view if registration fails
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var loggedInUser = await _context.Users
                .Where(x => x.Username == user.Username && x.Password == user.Password)
                .SingleOrDefaultAsync();

            if (loggedInUser != null)
            {
                if (loggedInUser.RoleId == 1)
                {
                    HttpContext.Session.SetInt32("LoggedAdmin", loggedInUser.ID);
                    return RedirectToAction("Index", "Admin");
                }
                if (loggedInUser.RoleId == 2)
                {
                    HttpContext.Session.SetInt32("LoggedUser", loggedInUser.ID);
                    return RedirectToAction("Index", "Users");
                }
                else if (loggedInUser.RoleId == 3)
                {
                    HttpContext.Session.SetInt32("LoggedChef", loggedInUser.ID);
                    return RedirectToAction("Index", "Chef");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(user);
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        public IActionResult LogoutUser()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }


    }
}
