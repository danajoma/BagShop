using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BagShop.Models;

namespace BagShop.Controllers
{
    public class UserController : Controller
    {
        private readonly BagContext context;

        public UserController(BagContext ctx)
        {
            context = ctx;
        }

        // 1. عرض الشناط المتاحة للزبون
        public IActionResult Index()
        {
            var bags = context.Bags.Include(b => b.User).ToList();
            return View(bags);
        }

        // 2. أكشن الشراء الفعلي 🛒
        public IActionResult Buy(int id)
        {
            // جلب رقم المستخدم من الجلسة (Session) لمعرفة من المشتري
            int? userId = HttpContext.Session.GetInt32("ID");

            if (userId == null)
            {
                // إذا مش مسجل دخول، بنرجعه على صفحة اللوجن
                return RedirectToAction("Login", "Home");
            }

            // البحث عن الشنطة المطلوبة في قاعدة البيانات
            var bag = context.Bags.Find(id);
            if (bag != null)
            {
                // ربط الشنطة برقم المستخدم اللي اشتراها (Foreign Key)
                bag.UserId = userId.Value;

                context.Bags.Update(bag);
                context.SaveChanges(); // حفظ العملية في الداتابيز
            }

            // بعد الشراء، بنرجعه على نفس الصفحة وبتظهر الشنطة محجوزة باسمه
            return RedirectToAction("Index");
        }
    }
}

