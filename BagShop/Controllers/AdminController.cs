using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BagShop.Models;

namespace BagShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly BagContext context;

        public AdminController(BagContext ctx)
        {
            context = ctx;
        }


        public IActionResult Index()
        {
            // بدلاً من فتح صفحة اليوزر الكروت، نخليه يفتح صفحة الـ BagsList اللي أرسلتيها هس
            return RedirectToAction("BagsList");
        }

        // عرض كل الشنط
        public IActionResult BagsList()
        {
            var bags = context.Bags
                .Include(b => b.User)
                .ToList();

            return View(bags);
        }

        // ===== Create =====
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Users = context.Users.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Bag bag)
        {
            if (ModelState.IsValid)
            {
                context.Bags.Add(bag);
                context.SaveChanges();

                // إعادة التوجيه الصريح للأكشن الرئيسي
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Users = context.Users.ToList();
            return View("Create", bag);
        }

        // ===== EDIT =====
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var bag = context.Bags.Find(id);

            if (bag == null)
                return NotFound();

            return View(bag);
        }

        [HttpPost]
        public IActionResult Edit(Bag bag)
        {
            // 1. تحديث البيانات في قاعدة البيانات
            context.Bags.Update(bag);
            context.SaveChanges();

            // 2. التعديل السحري: بعد نجاح التعديل، يرجع لصفحة الجدول اللي بمجلد User
            
            return RedirectToAction("Index", "User");
        }

        // ===== DELETE =====
        public IActionResult Delete(int id)
        {
            var bag = context.Bags.Find(id);

            if (bag != null)
            {
                // 1. حذف الشنطة من قاعدة البيانات
                context.Bags.Remove(bag);
                context.SaveChanges();
            }

            // 2. التعديل السحري: بعد الحذف بنجاح، يرجع فوراً لصفحة الجدول اللي بمجلد User
            return RedirectToAction("Index", "User");
        }

     
       

        
    }
}
