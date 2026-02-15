namespace ETicaretAPI.Models.Entities
{
    public class Product : BaseEntity
    {
       public string Name { get; set; } = string.Empty; // Ürün Adı
        public string Description { get; set; } = string.Empty; // Ürün Açıklaması
        public decimal Price { get; set; } // Fiyat
        public int Stock { get; set; } // Stok Adedi
        public string ImageUrl { get; set; } = string.Empty; // Resim dosyası yolu
        // İlişki: Bir ürünün bir kategorisi olur.
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }
    }
}