using ETicaretAPI.DTOs;
using ETicaretAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        public SettingsController(ISettingsService settingsService) { _settingsService = settingsService; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _settingsService.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] SiteSettingUpdateDto dto)
            => Ok(await _settingsService.UpsertAsync(dto));
    }
}
