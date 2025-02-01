using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System.Threading.Tasks;

namespace Restauracja.Pages.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // Dodanie UserManager

        public DeleteModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Order Order { get; set; } // Mo¿emy u¿yæ bez pe³nej kwalifikacji

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = _userManager.GetUserId(User);

            // Pobieramy zamówienie i sprawdzamy czy nale¿y do u¿ytkownika
            Order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userId = _userManager.GetUserId(User);

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(); // U¿ywamy asynchronicznego zapisu

            return RedirectToPage("./Index");
        }
    }
}
