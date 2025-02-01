using System;
using System.ComponentModel.DataAnnotations;

namespace Restauracja.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public bool IsPaid { get; set; } = false;
    }
}