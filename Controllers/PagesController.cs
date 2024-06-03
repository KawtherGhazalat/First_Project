using Microsoft.AspNetCore.Mvc;

namespace First_Project.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult AboutUs(string? layout)
        {
            ViewBag.Layout = layout;
            return View();
        }
        public IActionResult ContactUs(string? layout)
        {
            ViewBag.Layout = layout;
            return View();
        }

    }
}
