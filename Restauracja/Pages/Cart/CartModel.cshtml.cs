using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CartModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        [BindProperty]
        public decimal TotalPrice { get; set; } = 0;

        [BindProperty]
        public decimal DiscountApplied { get; set; } = 0;

        [BindProperty]
        public bool IsDiscountApplied { get; set; } = false;

        [BindProperty]
        public decimal FinalPrice { get; set; } = 0;

        public bool IsLoggedIn => User.Identity.IsAuthenticated;

        [BindProperty]
        [Required(ErrorMessage = "Imiê i nazwisko jest wymagane")]
        [RegularExpression(@"^[A-Za-z?-Ö?-ö?-?\s]+$", ErrorMessage = "Imiê i nazwisko mo¿e zawieraæ tylko litery.")]
        public string FullName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Ulica jest wymagana.")]
        [RegularExpression(@"^[A-Za-z?-Ö?-ö?-?\s]+$", ErrorMessage = "Ulica mo¿e zawieraæ tylko litery.")]
        public string Street { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Numer budynku jest wymagany.")]
        public string BuildingNumber { get; set; }

        [BindProperty]
        public string ApartmentNumber { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
        [RegularExpression(@"\d{2}-\d{3}", ErrorMessage = "Podaj kod pocztowy w formacie XX-XXX")]
        public string PostalCode { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Miasto jest wymagane.")]
        [RegularExpression(@"^[A-Za-z?-Ö?-ö?-?\s]+$", ErrorMessage = "Miasto mo¿e zawieraæ tylko litery.")]
        public string City { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Numer telefonu jest wymagany")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi mieæ 9 cyfr")]

        public string PhoneNumber { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Identity/Account/Login");
            }

            CartItems = await _context.CartItems
                .Include(c => c.MenuItem)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            TotalPrice = CartItems.Sum(c => c.MenuItem.Price * c.Quantity);
            FinalPrice = TotalPrice;

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(int itemId)
        {
            var cartItem = await _context.CartItems.FindAsync(itemId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostApplyDiscountAsync()
        {
            var userId = _userManager.GetUserId(User);
            CartItems = await _context.CartItems
                .Include(c => c.MenuItem)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!CartItems.Any())
            {
                ModelState.AddModelError("", "Koszyk jest pusty!");
                return Page();
            }

            TotalPrice = CartItems.Sum(c => c.MenuItem.Price * c.Quantity);
            DiscountApplied = TotalPrice * 0.05m;
            IsDiscountApplied = true;
            FinalPrice = TotalPrice - DiscountApplied;

            return Page();
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
                return Page();
            }

            if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Street) || string.IsNullOrEmpty(BuildingNumber) || string.IsNullOrEmpty(PostalCode) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(PhoneNumber))
            {
                ModelState.AddModelError("", "Wszystkie pola dostawy s¹ wymagane!");
                return Page();
            }

            decimal totalBeforeDiscount = cartItems.Sum(c => c.MenuItem.Price * c.Quantity);
            decimal discount = totalBeforeDiscount * 0.05m;
            decimal finalTotalPrice = Math.Round(totalBeforeDiscount - discount, 2);


            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = Math.Round(finalTotalPrice, 2),
                Status = PaymentStatus.Oczekuj¹ce,
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    MenuItemId = c.MenuItemId,
                    Quantity = c.Quantity,
                    Price = Math.Round(c.MenuItem.Price, 2)
                }).ToList(),

                FullName = FullName,
                Street = Street,
                BuildingNumber = BuildingNumber,
                ApartmentNumber = ApartmentNumber,
                PostalCode = PostalCode,
                City = City,
                PhoneNumber = PhoneNumber,
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Orders/MyOrders");
        }


    }
}