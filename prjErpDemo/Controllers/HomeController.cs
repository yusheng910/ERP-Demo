using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjErpDemo.Models;
using prjErpDemo.ViewModels;
using System.Diagnostics;
using System.Text.Json;

namespace prjErpDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly db_ErpDemoContext _db;

        public HomeController(ILogger<HomeController> logger, db_ErpDemoContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("Login"))
            {
                List<OrderVM> orderList = _db.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderStatus)
                    .Include(o => o.OrderDetails)
                    .Select(o => new OrderVM
                    {
                        order = o,
                        orderDate = o.OrderDate.ToString("yyyy-MM-dd HH:mm:ssZ"),
                        shippedDate = o.ShippedDate != null ? o.ShippedDate.Value.ToString("yyyy-MM-dd HH:mm:ssZ") : null,
                        totalAmount = o.OrderDetails.Sum(od => od.Product.Price * od.Quantity)
                    })
                    .OrderByDescending(o => o.order.OrderDate)
                    .ToList();
                return View(orderList);
            }
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.Keys.Contains("Login"))
            {
                string? jsonStr = HttpContext.Session.GetString("Login");
                if (jsonStr != null)
                {
                    User? loginUser = JsonSerializer.Deserialize<User>(jsonStr);
                    return RedirectToAction("Index", "Home");
                    // user in session: redirect to home page
                }

            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM login)
        {
            if (String.IsNullOrEmpty(login.loginAccount) || String.IsNullOrEmpty(login.loginPassword))
            {
                return View();
            }

            try
            {
                User? user = _db.Users.FirstOrDefault(u => u.AccountName.Equals(login.loginAccount)
                                && u.Password.Equals(CommonFn.ComputeSHA256Hash(login.loginPassword)));
                if (user != null)
                {
                    string jsonStr = "";
                    jsonStr = JsonSerializer.Serialize(user);
                    HttpContext.Session.SetString("Login", jsonStr);

                    _logger.LogInformation(user.AccountName + " has logging in the service");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("Web server error: " + ex.Message);
            }

            return View();

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Login");
            return RedirectToAction("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}