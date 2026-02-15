using ETicaretAPI.DTOs;
using ETicaretAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SlidersController : ControllerBase
    {
        private readonly ISliderService _sliderService;
        public SlidersController(ISliderService sliderService) { _sliderService = sliderService; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _sliderService.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SliderCreateDto dto)
            => Ok(await _sliderService.CreateAsync(dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sliderService.DeleteAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
