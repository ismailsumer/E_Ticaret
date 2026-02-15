using ETicaretAPI.DTOs;
using ETicaretAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) { _authService = authService; }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto request) => Ok(await _authService.Register(request));

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request) => Ok(await _authService.Login(request));

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { success = false, message = "Geçersiz token." });

            return Ok(await _authService.ChangePassword(userId, dto));
        }
    }
}