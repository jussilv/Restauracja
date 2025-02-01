using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restauracja.Data;
using Restauracja.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var orderInDb = await _context.Orders.FindAsync(Order.Id);
            if (orderInDb == null)
            {
                return NotFound();
            }

            orderInDb.OrderDate = Order.OrderDate;
            orderInDb.TotalPrice = Order.TotalPrice;
            orderInDb.Status = Order.Status;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}