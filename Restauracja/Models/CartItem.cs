namespace Restauracja.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }

        public MenuItem MenuItem { get; set; }

    }
}