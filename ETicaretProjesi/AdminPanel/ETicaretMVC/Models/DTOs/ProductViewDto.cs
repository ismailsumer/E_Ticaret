namespace AdminPanel.ETicaretMVC.Models.DTOs
{


    public class ProductViewDto
    { 
          public string ImageUrl { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty; // Kategori Id yerine ismini göstereceğiz
    }

    // Yeni Ürün Eklerken İsteyeceğimiz Model
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; } // Hangi kategoriye ait?
    }
}