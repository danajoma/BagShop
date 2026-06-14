using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BagStore.Models;

namespace BagStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly BagContext context;

        public AdminController(BagContext ctx)
        {
            context = ctx;
        }

        // حماية
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("ID") == null)
                return RedirectToAction("Login", "Home");

            return View();
        }

        // عرض كل الشنط
        public IActionResult BagsList()
        {
            var bags = context.Bags
                .Include(b => b.User)
                .ToList();

            return View(bags);
        }

        // ===== ADD =====
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Users = context.Users.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(Bag bag)
        {
            context.Bags.Add(bag);
            context.SaveChanges();

            return RedirectToAction("BagsList");
        }

        // ===== EDIT =====
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var bag = context.Bags.Find(id);
            return View(bag);
        }

        [HttpPost]
        public IActionResult Edit(Bag bag)
        {
            context.Bags.Update(bag);
            context.SaveChanges();

            return RedirectToAction("BagsList");
        }

        // ===== DELETE =====
        public IActionResult Delete(int id)
        {
            var bag = context.Bags.Find(id);
            context.Bags.Remove(bag);
            context.SaveChanges();

            return RedirectToAction("BagsList");
        }

        // بحث
        [HttpPost]
        public IActionResult Search(string key)
        {
            var result = context.Bags
                .Where(b => b.BagName.Contains(key))
                .ToList();

            return View("BagsList", result);
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
