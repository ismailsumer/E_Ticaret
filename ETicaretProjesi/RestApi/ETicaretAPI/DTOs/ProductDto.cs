namespace ETicaretAPI.DTOs
{
    // Ürünleri Listelerken Kullanacağımız Model
    public class ProductViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
    }

    // Yeni Ürün Eklerken İsteyeceğimiz Model
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public IFormFile? ImageFile { get; set; } // Ürün resmi (opsiyonel)
    }

    // Ürün Güncellerken Kullanacağımız Model
    public class ProductUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
