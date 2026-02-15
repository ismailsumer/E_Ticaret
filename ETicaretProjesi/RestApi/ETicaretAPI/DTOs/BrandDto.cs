namespace ETicaretAPI.DTOs
{
    public class BrandViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class BrandCreateDto
    {
        public string Name { get; set; } = string.Empty;
    }
}