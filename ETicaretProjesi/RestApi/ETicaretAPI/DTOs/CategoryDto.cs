namespace ETicaretAPI.DTOs
{
    public class CategoryViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
    }
}