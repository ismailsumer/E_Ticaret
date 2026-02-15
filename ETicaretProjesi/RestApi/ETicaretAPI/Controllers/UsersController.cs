using ETicaretAPI.Data;
using ETicaretAPI.DTOs;
using ETicaretAPI.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly ETicaretContext _context;
        public UsersController(ETicaretContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
                .Select(u => new UserViewDto { Id = u.Id, Username = u.Username, Role = u.Role, PhoneNumber = u.PhoneNumber })
                .ToListAsync();
            return Ok(ServiceResponse<List<UserViewDto>>.SuccessResponse(users));
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _context.Users
                .Where(u => u.Role == "Customer")
                .Select(u => new UserViewDto { Id = u.Id, Username = u.Username, Role = u.Role, PhoneNumber = u.PhoneNumber })
                .ToListAsync();
            return Ok(ServiceResponse<List<UserViewDto>>.SuccessResponse(customers));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(ServiceResponse<string>.ErrorResponse("Kullanıcı bulunamadı."));

            var dto = new UserViewDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber
            };
            return Ok(ServiceResponse<UserViewDto>.SuccessResponse(dto));
        }

        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UserRoleUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(ServiceResponse<string>.ErrorResponse("Kullanıcı bulunamadı."));

            var validRoles = new[] { "Admin", "Customer" };
            if (!validRoles.Contains(dto.Role))
                return BadRequest(ServiceResponse<string>.ErrorResponse("Geçersiz rol. (Admin veya Customer)"));

            user.Role = dto.Role;
            await _context.SaveChangesAsync();
            return Ok(ServiceResponse<string>.SuccessResponse("", $"Kullanıcı rolü '{dto.Role}' olarak güncellendi."));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminCreateDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(ServiceResponse<string>.ErrorResponse("Bu kullanıcı adı zaten mevcut."));

            var validRoles = new[] { "Admin", "Customer" };
            if (!validRoles.Contains(dto.Role))
                return BadRequest(ServiceResponse<string>.ErrorResponse("Geçersiz rol."));

            using var hmac = new HMACSHA512();
            var user = new Models.Entities.User
            {
                Username = dto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key,
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(ServiceResponse<string>.SuccessResponse("", "Kullanıcı başarıyla oluşturuldu."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(ServiceResponse<string>.ErrorResponse("Kullanıcı bulunamadı."));

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(ServiceResponse<string>.SuccessResponse("", "Kullanıcı silindi."));
        }
    }
}
