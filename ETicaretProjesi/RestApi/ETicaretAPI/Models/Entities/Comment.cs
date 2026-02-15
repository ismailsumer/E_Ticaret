namespace ETicaretAPI.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5 arası puanlama
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}