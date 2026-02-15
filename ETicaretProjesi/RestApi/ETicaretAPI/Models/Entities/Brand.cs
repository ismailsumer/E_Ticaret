namespace ETicaretAPI.Models.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; } = true;

        // İlişki: Bir markanın birden fazla ürünü olabilir
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}