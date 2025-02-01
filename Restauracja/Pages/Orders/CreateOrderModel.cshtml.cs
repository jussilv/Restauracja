using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restauracja.Data;
using Restauracja.Models;
using System.Collections.Generic;
using System.Linq;

namespace Restauracja.Pages.Orders
{
    public class CreateOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateOrderModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<int> SelectedMenuItems { get; set; } = new List<int>();

        public List<MenuItem> MenuItems { get; set; }

        public void OnGet()
        {
            MenuItems = _context.MenuItems.ToList();
        }

        public IActionResult OnPost()
        {
            if (SelectedMenuItems.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Musisz wybraæ co najmniej jedn¹ pozycjê z menu.");
                return Page();
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalPrice = 0,
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var menuItemId in SelectedMenuItems)
            {
                var menuItem = _context.MenuItems.FirstOrDefault(m => m.Id == menuItemId);
                if (menuItem != null)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        MenuItemId = menuItem.Id,
                        Quantity = 1
                    });
                    order.TotalPrice += menuItem.Price;
                }
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}