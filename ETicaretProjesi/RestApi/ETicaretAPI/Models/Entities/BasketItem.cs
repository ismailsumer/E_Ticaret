using ETicaretAPI.Models.Entities;

namespace ETicaretAPI.Models.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; } // Hangi kullanıcının sepeti?
        public User? User { get; set; }
    }
}