using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ETicaretAPI.Data;
using ETicaretAPI.DTOs;
using ETicaretAPI.Models.Entities;
using ETicaretAPI.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ETicaretAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ETicaretContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ETicaretContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Register(UserRegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return ServiceResponse<string>.ErrorResponse("Bu kullanıcı adı zaten alınmış.");

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = request.PhoneNumber
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return ServiceResponse<string>.SuccessResponse("", "Kullanıcı başarıyla oluşturuldu.");
        }

        public async Task<ServiceResponse<LoginResponseDto>> Login(UserLoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null) return ServiceResponse<LoginResponseDto>.ErrorResponse("Kullanıcı bulunamadı.");

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return ServiceResponse<LoginResponseDto>.ErrorResponse("Hatalı şifre.");

            string token = CreateToken(user);
            var loginResponse = new LoginResponseDto
            {
                Token = token,
                Role = user.Role,
                Username = user.Username
            };
            return ServiceResponse<LoginResponseDto>.SuccessResponse(loginResponse, "Giriş başarılı.");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

       private string CreateToken(User user)
{
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role) // ROL bilgisini token'a ekledik
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddDays(7),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

        public async Task<ServiceResponse<string>> ChangePassword(int userId, ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return ServiceResponse<string>.ErrorResponse("Kullanıcı bulunamadı.");

            if (!VerifyPasswordHash(dto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                return ServiceResponse<string>.ErrorResponse("Mevcut şifre hatalı.");

            if (dto.NewPassword != dto.ConfirmNewPassword)
                return ServiceResponse<string>.ErrorResponse("Yeni şifreler eşleşmiyor.");

            if (dto.NewPassword.Length < 6)
                return ServiceResponse<string>.ErrorResponse("Yeni şifre en az 6 karakter olmalıdır.");

            CreatePasswordHash(dto.NewPassword, out byte[] newHash, out byte[] newSalt);
            user.PasswordHash = newHash;
            user.PasswordSalt = newSalt;
            await _context.SaveChangesAsync();

            return ServiceResponse<string>.SuccessResponse("", "Şifre başarıyla güncellendi.");
        }
        
    }
}