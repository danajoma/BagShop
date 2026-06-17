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
                // نأخذه فوراً "مشي" لجدول الإدارة والشنط
                return RedirectToAction("Index", "Home");
            }

            // 3. إذا لم نجده (بياناته خطأ أو الحساب غير موجود)
            // نضع رسالة تنبيه تظهر بالصفحة
            ViewBag.ErrorMessage = "الإيميل أو كلمة المرور غير صحيحة! ❌";

            // ونعيد عرض صفحة اللوجن نفسها ليعيد المحاولة بدلاً من فتح صفحة فشل مستقلة
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
