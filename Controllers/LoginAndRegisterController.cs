using First_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace First_Project.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        private readonly ModelContext context;
        public LoginAndRegisterController(ModelContext context)
        {
            this.context = context;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([Bind("Userid,Username,Email,Password,Roleid")] User user )
        {
            user.Roleid = 2;
            context.Users.Add(user);
        }
    }
}
