using Microsoft.AspNetCore.Mvc.RazorPages;
using Restauracja.Data;
using Restauracja.Models;
using System.Threading.Tasks;

namespace Restauracja.Pages.Orders
{
    public class SuccessModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SuccessModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = PaymentStatus.Oplacone;
                await _context.SaveChangesAsync();
            }
        }
    }
}