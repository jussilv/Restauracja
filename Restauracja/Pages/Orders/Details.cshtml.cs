using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restauracja.Data;
using Restauracja.Models;

namespace Restauracja.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public IActionResult OnGet(int id)
        {
            Order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
