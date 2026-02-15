using System.Security.Claims;
using ETicaretAPI.DTOs;
using ETicaretAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService) { _orderService = orderService; }

        private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout() => Ok(await _orderService.CreateOrderAsync(UserId));

        [HttpGet]
        public async Task<IActionResult> GetMyOrders() => Ok(await _orderService.GetUserOrdersAsync(UserId));

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders() => Ok(await _orderService.GetAllOrdersAsync());

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderStatusUpdateDto dto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, dto.Status);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}