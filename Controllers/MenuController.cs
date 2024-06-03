using First_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using static NuGet.Packaging.PackagingConstants;

namespace First_Project.Controllers
{
    public class MenuController : Controller
    {
        private ModelContext _context;
        public MenuController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!CheckUser())
                return RedirectToAction("Login", "LoginAndRegister");
            var categories = _context.Categories.ToList(); 
            return View(categories);
        }
        public IActionResult GetAllOrders()
        {
            if (!CheckUser())
                return RedirectToAction("Login", "LoginAndRegister");
            var orders = _context.Orders
                .Where(x => x.UserId == HttpContext.Session.GetInt32("LoggedUser").Value)
                .Include(x=>x.User)
                .ToList();

            orders.ForEach(o => o.MenuItems = JsonConvert.DeserializeObject<List<MenuItem>>(o.Items));

            return View(orders);
        }
        public ActionResult PlaceOrder([FromBody] List<MenuItem> selectedItemsJson)
        {
            var userId = HttpContext.Session.GetInt32("LoggedUser");

            var isHasOrder = _context.Orders.Where(x => x.UserId == userId && x.isPaid == false).FirstOrDefault();
            if (isHasOrder != null)
                return Json(new { message = $"User has already placed an order and has not paid yet! Order number is {isHasOrder.OrderNumber} ", type = 0 });

            if (selectedItemsJson.Count() > 0)
            {
                Order order = new Order
                {
                    Date = DateTime.UtcNow,
                    Items = JsonConvert.SerializeObject(selectedItemsJson),
                    OrderNumber = GenerateOrderId(),
                    OrderRequester = _context.Users
                    .FirstOrDefault(x => x.ID == userId).Username,
                    TotalPrice = selectedItemsJson.Sum(item => item.Price),
                    UserId = userId.Value
                };
                _context.Orders.Add(order);
                _context.SaveChanges();

                List<int> recipeIds = new List<int>();
                selectedItemsJson.ForEach(x => 
                {
                    recipeIds.Add(x.RecipeId);
                });


                foreach(var id in recipeIds)
                {
                    var recpie = _context.Recipes.FirstOrDefault(x => x.ID == id);
                    if (recpie is not null)
                    {
                        recpie.isSold = true;
                        _context.Recipes.Update(recpie);
                        _context.SaveChanges();

                    }
                }

            }
            else 
                return Json(new { message = $"You have not placed any items please check your cart", type = 0 });

            return Json(new { message = $"Order is placed! You can carry on to checkout!", type = 1 });

        }

        public JsonResult GetRecipesByCategory(int categoryId)
        {
            var menuItems = _context.Recipes
                            .Include(r => r.Category)
                            .Where(r => r.CategoryId == categoryId)
                            .Select(r => new
                            {
                                r.ID,
                                r.Title,
                                r.Description,
                                r.Price,
                                CategoryName = r.Category.CategoryName,
                                CategoryId = r.CategoryId,
                                RecipeId = r.ID,
                                Image = r.Image,

                            })
                            .ToList();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Use camelCase for property names
                WriteIndented = true, // Pretty print the JSON
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull // Ignore null values
            };

            return Json(menuItems, jsonOptions);
        }

        string GenerateOrderId()
        {
            string orderId = (100 - _context.Orders.Count()).ToString();
            return '#' + orderId;
        }
        private bool CheckUser()
        {
            if (HttpContext.Session.GetInt32("LoggedUser") != null)
                return true;

            return false;
        }


    }
}
