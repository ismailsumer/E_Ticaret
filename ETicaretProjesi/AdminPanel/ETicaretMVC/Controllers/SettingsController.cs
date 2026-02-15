using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;
using AdminPanel.ETicaretMVC.Models.DTOs;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class SettingsController : BaseController
    {
        public SettingsController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> Index()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("Settings");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            var settings = new List<SiteSettingDto>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<ServiceResponse<List<SiteSettingDto>>>();
                settings = json?.Data ?? new();
            }
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Save(string key, string value)
        {
            var client = GetHttpClient();
            var content = JsonContent.Create(new { Key = key, Value = value });
            var response = await client.PostAsync("Settings", content);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Ayar kaydedildi.";
            else
                TempData["Error"] = "Ayar kaydedilemedi.";

            return RedirectToAction("Index");
        }
    }
}
