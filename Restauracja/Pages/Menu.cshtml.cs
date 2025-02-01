using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;

namespace Restauracja.Pages
{
    public class MenuModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MenuModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<MenuItem> MenuItems { get; set; } = new();

        public async Task OnGetAsync()
        {
            MenuItems = await _context.MenuItems.Where(m => m.IsAvailable).ToListAsync();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/LoginRegister");
            }

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.MenuItemId == id && c.UserId == userId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    MenuItemId = id,
                    UserId = userId,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Cart/Index");
        }

    }
}
