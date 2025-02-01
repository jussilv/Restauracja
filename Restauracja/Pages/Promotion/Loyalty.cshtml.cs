using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Restauracja.Pages.Promotion
{
    [Authorize]
    public class LoyaltyModel : PageModel
    {
        public List<PromotionModel> LoyaltyPromotions { get; set; }

        public void OnGet()
        {
            LoyaltyPromotions = new List<PromotionModel>
            {
               
                new PromotionModel { Title = "Has³o: luty", Description = "Darmowa kawa do ka¿dego œniadania! na miejscu", Discount = 100 },
                
            };
        }
    }
}