namespace AdminPanel.ETicaretMVC.Models.DTOs
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public IFormFile? ImageFile { get; set; } // Resim dosyası için
    }
}