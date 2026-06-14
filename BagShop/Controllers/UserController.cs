using Microsoft.AspNetCore.Mvc;
using BagStore.Models;

namespace BagStore.Controllers
{
    public class UserController : Controller
    {
        private readonly BagContext context;

        public UserController(BagContext ctx)
        {
            context = ctx;
        }

        // حماية الصفحة
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("ID") == null)
                return RedirectToAction("Login", "Home");

            return View();
        }

        // عرض كل الشنط
        public IActionResult BagsList()
        {
            var bags = context.Bags.OrderBy(b => b.BagName).ToList();
            return View(bags);
        }

        // شنط المستخدم فقط
        public IActionResult MyBags()
        {
            int? id = HttpContext.Session.GetInt32("ID");

            if (id == null)
                return RedirectToAction("Login", "Home");

            var myBags = context.Bags
                .Where(b => b.UserId == id)
                .ToList();

            return View(myBags);
        }

        // تغيير كلمة السر
        [HttpPost]
        public IActionResult ChangePassword(string oldPass, string newPass, string confirmPass)
        {
            int? id = HttpContext.Session.GetInt32("ID");

            var user = context.Users.Find(id);

            if (user == null)
                return RedirectToAction("Login", "Home");

            if (newPass != confirmPass)
                return View("Wrong");

            if (oldPass == newPass)
                return View("Similar");

            user.Password = newPass;
            context.SaveChanges();

            return View("Success");
        }

        // تسجيل خروج
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
