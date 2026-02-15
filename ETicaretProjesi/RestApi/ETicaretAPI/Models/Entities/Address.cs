namespace ETicaretAPI.Models.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // Ev, İş vb.
        public string FullAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty; // İlçe
        
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}