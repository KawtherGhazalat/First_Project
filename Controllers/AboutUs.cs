using Microsoft.AspNetCore.Mvc;

namespace First_Project.Controllers
{
    public class AboutUs : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
