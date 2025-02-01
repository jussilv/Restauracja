using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System.Threading.Tasks;

namespace Restauracja.Pages.Orders
{
    public class OrderConfirmedModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrderConfirmedModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            Order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}