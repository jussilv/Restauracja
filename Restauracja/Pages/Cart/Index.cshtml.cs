using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Restauracja.Data;
using Restauracja.Models;
using Microsoft.EntityFrameworkCore;


namespace Restauracja.Pages.Cart
{
    public class CartIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CartIndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<CartItem> CartItems { get; set; }
        public List<Restauracja.Models.Promotion> AvailablePromotions { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal DiscountApplied { get; set; } = 0;

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            CartItems = await _context.CartItems
                .Include(c => c.MenuItem)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            AvailablePromotions = await _context.Promotions
                .Where(p => p.IsActive)
                .ToListAsync();

            TotalPrice = CartItems.Sum(c => c.MenuItem.Price * c.Quantity) - DiscountApplied;

            return Page();
        }

        public async Task<IActionResult> OnPostApplyPromotionAsync(int promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);
            if (promotion == null || !promotion.IsActive)
            {
                ModelState.AddModelError("", "Nieprawid³owa promocja!");
                return await OnGetAsync();
            }

            DiscountApplied = promotion.DiscountAmount;
            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _context.CartItems
                .Include(c => c.MenuItem)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                ModelState.AddModelError("", "Koszyk jest pusty!");
                return await OnGetAsync();
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = cartItems.Sum(c => c.MenuItem.Price * c.Quantity) - DiscountApplied,
                Status = PaymentStatus.Oczekuj¹ce,
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    MenuItemId = c.MenuItemId,
                    Quantity = c.Quantity,
                    Price = c.MenuItem.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Orders/MyOrders");
        }
    }
}