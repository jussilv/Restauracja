using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restauracja.Data;
using Restauracja.Models;
using System.Collections.Generic;
using System.Linq;

[Authorize]
public class CreateOrderModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateOrderModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<MenuItem> MenuItems { get; set; }

    [BindProperty]
    public List<int> SelectedMenuItems { get; set; }

    public void OnGet()
    {
        MenuItems = _context.MenuItems.Where(m => m.IsAvailable).ToList();
    }

    public IActionResult OnPost()
    {
        if (SelectedMenuItems == null || !SelectedMenuItems.Any())
        {
            ModelState.AddModelError(string.Empty, "Wybierz przynajmniej jedn¹ pozycjê z menu.");
            return Page();
        }

        var userId = User.Identity.Name;

        var order = new Order
        {
            UserId = userId,
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
                    MenuItemId = menuItemId,
                    Quantity = 1,
                    Price = menuItem.Price
                });
                order.TotalPrice += menuItem.Price;
            }
        }

        _context.Orders.Add(order);
        _context.SaveChanges();

        return RedirectToPage("/Orders/MyOrders");
    }
}
