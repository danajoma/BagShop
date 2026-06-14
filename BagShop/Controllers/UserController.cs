using Microsoft.AspNetCore.Mvc;
using BagStore.Models;

namespace BagStore.Controllers
{
    public class UserController : Controller
    {
        private readonly BagContext context;

        public UserController(BagContext ctx) => context = ctx;

        // ===== HOME USER =====
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("ID") == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // ===== SEARCH =====
        [HttpGet]
        public IActionResult Search()
        {
            if (HttpContext.Session.GetInt32("ID") == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Search(string key)
        {
            if (HttpContext.Session.GetInt32("ID") == null)
                return RedirectToAction("Index", "Home");

            List<Bag> bags = context.Bags
                .Where(b => b.BagName.Contains(key))
                .ToList();

            if (bags != null && bags.Count > 0)
                return View("BagSearchResult", bags);

            return View("Index");
        }

        // ===== LIST ALL BAGS =====
        [HttpGet]
        public IActionResult BagList()
        {
            List<Bag> L = context.Bags
                .OrderBy(b => b.BagName)
                .ToList();

            return View(L);
        }

        // ===== CHANGE PASSWORD =====
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetInt32("ID") == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string p1, string p2, string p3)
        {
            int? id = HttpContext.Session.GetInt32("ID");

            if (id == null)
                return RedirectToAction("Index", "Home");

            if (p2 == p3)
            {
                if (p1 == p2)
                    return View("Similar");

                BagStore.Models.User temp = context.Users.Find(id);

                if (temp != null)
                {
                    temp.Password = p2;
                    context.Users.Update(temp);
                    context.SaveChanges();
                }

                return View("Success");
            }

            return View("Wrong");
        }

        // ===== MY BAGS =====
        public IActionResult MyBags()
        {
            if (HttpContext.Session.GetInt32("ID") == null)
                return RedirectToAction("Index", "Home");

            int? ID = HttpContext.Session.GetInt32("ID");

            List<Bag> bags = context.Bags
                .Where(b => b.UserId == ID)
                .OrderBy(b => b.BagName)
                .ToList();

            return View("BagList", bags);
        }

        // ===== LOGOUT =====
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

