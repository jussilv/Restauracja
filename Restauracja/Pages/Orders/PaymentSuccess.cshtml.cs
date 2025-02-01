using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restauracja.Data;
using Restauracja.Models;
using System.Threading.Tasks;

namespace Restauracja.Pages.Orders
{
    public class PaymentSuccessModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PaymentSuccessModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _context.Orders.FindAsync(id);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}