using BagShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace BagStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly BagContext context;

        public HomeController(BagContext ctx)
        {
            context = ctx;
        }

        // ===== HOME =====
        public IActionResult Index()
        {
            var L = context.Bags.OrderBy(b => b.BagName).ToList();
            return View(L);
        }

        public IActionResult Privacy() => View();

        // ===== LOGIN =====
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("ID") == null)
                return View();

            // إذا مسجل دخول
            if (HttpContext.Session.GetInt32("ID") == 1)
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            BagShop.Models.User u =
                context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (u != null)
            {
                HttpContext.Session.SetInt32("ID", u.UserId);

                if (HttpContext.Session.GetInt32("ID") == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }

            return View("LoginFailed");
        }

        // ===== SIGN UP =====
        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        public IActionResult SignUp(BagShop.Models.User u, string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value.Errors.Select(er => er.ErrorMessage).ToList()
                    });

                string errorDetails = "❌ النموذج غير صالح:<br><ul>";

                foreach (var error in errors)
                {
                    errorDetails += $"<li><strong>{error.Field}</strong>: {string.Join(", ", error.Errors)}</li>";
                }

                errorDetails += "</ul>";

                return Content(errorDetails, "text/html");
            }

            if (u.Password == confirmPassword)
            {
                context.Users.Add(u);
                context.SaveChanges();

                ViewBag.UserID = u.UserId;
                return View("Successful");
            }

            return View("UnsuccessfulSignUp");
        }

        // ===== ERROR =====
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
