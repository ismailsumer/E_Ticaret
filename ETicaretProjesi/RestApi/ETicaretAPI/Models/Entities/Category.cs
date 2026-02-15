using System.Collections.Generic;

namespace ETicaretAPI.Models.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty; // Kategori Adı (Örn: Elektronik)
        public string Description { get; set; } = string.Empty; // Açıklama

        // İlişki: Bir kategorinin birden fazla ürünü olabilir.
        public ICollection<Product> Products  { get; set; }
    }
}