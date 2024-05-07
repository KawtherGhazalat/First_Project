using Microsoft.AspNetCore.Mvc;

namespace First_Project.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
