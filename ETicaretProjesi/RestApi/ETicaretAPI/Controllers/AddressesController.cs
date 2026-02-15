using System.Security.Claims;
using ETicaretAPI.DTOs;
using ETicaretAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Dosyadaki "Role-based güvenlik" kuralı gereği
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // Token içindeki NameIdentifier'dan (User ID) otomatik ID alıyoruz
        private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        [HttpGet]
        public async Task<IActionResult> GetMyAddresses()
        {
            var result = await _addressService.GetUserAddressesAsync(UserId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressDto dto)
        {
            var result = await _addressService.AddAddressAsync(UserId, dto);
            return Ok(result);
        }
    }
}