using First_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user, string Username, string Password, string Email)
        {
            var existingUser = _context.Profiles.FirstOrDefault(x => x.Username == Username);
            if (existingUser == null)
            {
                if (ModelState.IsValid)
                {
                    user.Password = Password;
                    user.Email = Email;
                    user.Roleid = 2;
                    _context.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Login"); // Redirect to login page after successful registration
                }
            }
            else
            {
                ModelState.AddModelError("", "Username already exists");
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
            var users = await _context.Users.
                Where(x => x.Username == user.Username && x.Password == user.Password).SingleOrDefaultAsync();

            if (users != null)
            {
                switch (users.Roleid)
                {
                    case 1:
                        HttpContext.Session.SetInt32("AdminId", (int)users.Userid);
                        return RedirectToAction("Index", "Admin");
                    case 2:
                        HttpContext.Session.SetInt32("Userid", (int)users.Userid);
                        return RedirectToAction("Index", "Users");
                    case 3:
                        HttpContext.Session.SetInt32("Chefid", (int)users.Userid);
                        return RedirectToAction("Index", "Chef");

                }
            }
            ModelState.AddModelError("", "UserName or Password are incorret");
            return View();

        }
        

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            //return RedirectToAction(" ", " ");
            return View("Login");
        }

    }
}
