using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja.Pages.Orders
{
    public class MyOrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyOrdersModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Order> Orders { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            IQueryable<Order> query = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.MenuItem)
                .Where(o => o.UserId == userId);

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(o => o.OrderItems.Any(oi => oi.MenuItem.Name.Contains(SearchTerm)) ||
                                         o.OrderDetails.Any(od => od.MenuItem.Name.Contains(SearchTerm)));
            }

            
            switch (SortBy)
            {
                case "price_desc":
                    query = query.OrderByDescending(o => (double)o.TotalPrice);
                    break;
                case "price_asc":
                    query = query.OrderBy(o => (double)o.TotalPrice);
                    break;
                case "date_desc":
                    query = query.OrderByDescending(o => o.OrderDate);
                    break;
                case "date_asc":
                    query = query.OrderBy(o => o.OrderDate);
                    break;
            }

            Orders = await query.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostPayAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = PaymentStatus.Oplacone;
                _context.Update(order);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Orders/PaymentSuccess", new { id = order.Id });
            }
            return RedirectToPage();
        }

    }
}