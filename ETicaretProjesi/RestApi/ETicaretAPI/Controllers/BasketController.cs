using System.Security.Claims;
using ETicaretAPI.DTOs;
using ETicaretAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece giriş yapanlar sepet kullanabilir
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _basketService.GetUserBasketAsync(UserId));

        [HttpPost]
        public async Task<IActionResult> Post(BasketItemCreateDto dto) => Ok(await _basketService.AddToBasketAsync(UserId, dto));

        [HttpDelete]
        public async Task<IActionResult> Delete() => Ok(await _basketService.ClearBasketAsync(UserId));
    }
}