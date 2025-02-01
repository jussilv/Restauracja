using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Restauracja.Pages.Promotion
{
    public class PublicModel : PageModel
    {
        public List<PromotionModel> PublicPromotions { get; set; }

        public void OnGet()
        {
            PublicPromotions = new List<PromotionModel>
            {
                
                new PromotionModel { Title = "Rodzinny Weekend", Description = "Darmowy deser dla rodzin powy¿ej 4 osób!", Discount = 100 },
            };
        }
    }

    public class PromotionModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Discount { get; set; }
    }
}