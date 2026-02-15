namespace ETicaretAPI.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Beklemede"; // Beklemede, Hazırlanıyor, Kargoda, Teslim Edildi
        
        // İlişkiler
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
