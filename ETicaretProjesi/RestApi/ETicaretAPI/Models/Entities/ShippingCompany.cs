namespace ETicaretAPI.Models.Entities
{
    public class ShippingCompany : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
