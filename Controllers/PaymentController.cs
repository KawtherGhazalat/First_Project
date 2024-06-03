using First_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace First_Project.Controllers
{
    public class PaymentController : Controller
    {
        private ModelContext _context;

        public PaymentController(ModelContext context )
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (!CheckUser())
                return RedirectToAction(nameof(Index), nameof(LoginAndRegisterController));
            return View();
        }
        public async Task<IActionResult> Pay()
        {
            var userId = HttpContext.Session.GetInt32("LoggedUser").Value;
            var order = _context.Orders.FirstOrDefault(x => x.isPaid == false && x.UserId == userId);
            _context.Payments.Add(new Payment()
            {
                Amount = (decimal) order.TotalPrice,
                CreationDate = DateTime.UtcNow,
                UserId = userId

            });
            _context.SaveChanges();

            order.isPaid = true;
            _context.Orders.Update(order);
            _context.SaveChanges();

            return RedirectToAction(nameof(ThankYou));
        }
        [HttpGet]
        public ActionResult ThankYou()
        {
            return View();
        }
        private bool CheckUser()
        {
            if (HttpContext.Session.GetInt32("LoggedUser") != null)
                return true;

            return false;
        }
        public IActionResult Transactions()
        {
            return View(_context.Payments.Include(x=>x.User).ToList());
        }

    }
}
