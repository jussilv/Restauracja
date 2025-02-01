using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Restauracja.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Imię i nazwisko jest wymagane.")]
        [StringLength(50, ErrorMessage = "Imię i nazwisko może mieć maksymalnie 50 znaków.")]
        public string FullName { get; set; }
        [Required]
        public string Street { get; set; }

        [Required]
        public string BuildingNumber { get; set; }

        [Required]
        [RegularExpression(@"\d{2}-\d{3}", ErrorMessage = "Podaj kod pocztowy w formacie XX-XXX")]
        public string PostalCode { get; set; }

        public string ApartmentNumber { get; set; }

        [Required]
        public string City { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi zawierać dokładnie 9 cyfr.")]
        public string PhoneNumber { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Kwota musi być liczbą dodatnią.")]
        public decimal TotalPrice { get; set; }

        public PaymentStatus Status { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public List<OrderDetail> OrderDetails { get; set; }
    }

    



}
