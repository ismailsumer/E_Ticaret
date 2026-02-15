namespace ETicaretAPI.DTOs
{
    public class CommentCreateDto
    {
        public int ProductId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
    }

    public class CommentViewDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}
