using System;
using System.ComponentModel.DataAnnotations;
namespace Restauracja.Models
{
    public class Promotion
    {
        public int Id { get; set; }


        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; }

        [Required]
        public string WeeklyPromotion { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
