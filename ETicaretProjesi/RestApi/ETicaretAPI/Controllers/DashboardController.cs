using ETicaretAPI.Data;
using ETicaretAPI.DTOs;
using ETicaretAPI.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly ETicaretContext _context;
        public DashboardController(ETicaretContext context) { _context = context; }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = new DashboardStatsDto
            {
                TotalProducts = await _context.Products.CountAsync(p => !p.IsDeleted),
                TotalUsers = await _context.Users.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalComments = await _context.Comments.CountAsync()
            };
            return Ok(ServiceResponse<DashboardStatsDto>.SuccessResponse(stats));
        }
    }
}
