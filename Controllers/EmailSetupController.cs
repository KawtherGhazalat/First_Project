using Microsoft.AspNetCore.Mvc;
using First_Project.Models;
using System.Net;
using System.Net.Mail;

namespace First_Project.Controllers
{
    public class EmailSetupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(First_Project.Models.gmail model)
        {
            MailMessage mm = new MailMessage("kghazalat2000@gmail.com", model.To);
            mm.Subject = model.Subject;
            mm.Body = model.Body;
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;


            NetworkCredential nc = new NetworkCredential("kghazalat2000@gmail.com", "gmailpassword");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
            ViewBag.Message = "Mail has been sent successfully!";


            return View();
        }




    }
}
