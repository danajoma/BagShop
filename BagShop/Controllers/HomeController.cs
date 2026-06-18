using BagShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace BagShop.Controllers
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
            // 1. البحث عن المستخدم في قاعدة البيانات
            BagShop.Models.User u = context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            // 2. إذا وجدنا المستخدم (بياناته صحيحة)
            if (u != null)
            {
                // ===: حفظ بيانات المستخدم في السيشن ===
                HttpContext.Session.SetInt32("ID", u.UserId);

                // إذا كان نظامك يعتمد على أن المستخدم رقم 1 هو الأدمن:
                if (u.UserId == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }

            // 3. إذا لم نجده (بياناته خطأ)
            ViewBag.ErrorMessage = "الإيميل أو كلمة المرور غير صحيحة! ❌";
            return View();
        }

        // ===== SIGN UP =====
        [HttpGet]
        public IActionResult SignUp() => View();

      
        [HttpPost]
        public IActionResult SignUp(User u, string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(u);
            }

           

            context.Users.Add(u);
            context.SaveChanges();

            ViewBag.UserID = u.UserId;

            return View("Successful");
        }

        

    }
}
