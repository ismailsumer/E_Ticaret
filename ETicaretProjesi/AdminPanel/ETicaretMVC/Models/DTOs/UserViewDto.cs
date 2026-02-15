namespace AdminPanel.ETicaretMVC.Models.DTOs
{
    public class UserViewDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
