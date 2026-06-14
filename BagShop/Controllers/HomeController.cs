using Microsoft.AspNetCore.Mvc;
using BagStore.Models;

namespace BagStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly BagContext context;

        public HomeController(BagContext ctx)
        {
            context = ctx;
        }

        // الصفحة الرئيسية
        public IActionResult Index()
        {
            var bags = context.Bags.OrderBy(b => b.BagName).ToList();
            return View(bags);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // ===== LOGIN =====
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("ID") != null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("ID", user.UserId);

                if (user.UserId == 1)
                    return RedirectToAction("Index", "Admin");

                return RedirectToAction("Index", "User");
            }

            return View("LoginFailed");
        }

        // ===== SIGN UP =====
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User u)
        {
            if (!ModelState.IsValid)
                return View(u);

            context.Users.Add(u);
            context.SaveChanges();

            return View("Success");
        }
    }
}