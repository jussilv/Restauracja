using Microsoft.AspNetCore.Mvc;
using Restauracja.Data;
using Restauracja.Models;

namespace RestauracjaApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var menuItems = _context.MenuItems.Where(m => m.IsAvailable).ToList();
            return View(menuItems);
        }
    }
}