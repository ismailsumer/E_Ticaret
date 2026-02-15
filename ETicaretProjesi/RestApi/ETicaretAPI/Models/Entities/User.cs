namespace ETicaretAPI.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string Role { get; set; } = "Customer";
        public string PhoneNumber { get; set; } = string.Empty;
    }
}