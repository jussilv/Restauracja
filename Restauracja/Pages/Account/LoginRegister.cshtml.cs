using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Restauracja.Pages.Account
{
    public class LoginRegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginRegisterModel> _logger;

        public LoginRegisterModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<LoginRegisterModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public LoginInputModel LoginInput { get; set; }

        [BindProperty]
        public RegisterInputModel RegisterInput { get; set; }

        public class LoginInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public class RegisterInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Has³a musz¹ siê zgadzaæ.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            _logger.LogInformation("?? Próba logowania: {Email}", LoginInput.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("?? Logowanie przerwane - model niepoprawny");
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(LoginInput.Email, LoginInput.Password, false, false);

            if (result.Succeeded)
            {
                _logger.LogInformation("? Logowanie udane dla: {Email}", LoginInput.Email);
                return RedirectToPage("/Orders/MyOrders");
            }

            _logger.LogWarning("? B³¹d logowania dla: {Email}", LoginInput.Email);
            ModelState.AddModelError(string.Empty, "Nieprawid³owy login lub has³o.");
            return Page();
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            _logger.LogInformation("?? Próba rejestracji: {Email}", RegisterInput.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("?? Rejestracja przerwana - model niepoprawny");
                return Page();
            }

            var user = new IdentityUser { UserName = RegisterInput.Email, Email = RegisterInput.Email };
            var result = await _userManager.CreateAsync(user, RegisterInput.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("? U¿ytkownik zarejestrowany: {Email}", RegisterInput.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Orders/MyOrders");
            }

            foreach (var error in result.Errors)
            {
                _logger.LogWarning("? B³¹d rejestracji: {Error}", error.Description);
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}