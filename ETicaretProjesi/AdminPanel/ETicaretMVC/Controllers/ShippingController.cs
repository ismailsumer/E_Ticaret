using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;
using AdminPanel.ETicaretMVC.Models.DTOs;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class ShippingController : BaseController
    {
        public ShippingController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> Index()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("Shipping");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            var items = new List<ShippingViewDto>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<ServiceResponse<List<ShippingViewDto>>>();
                items = json?.Data ?? new();
            }
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, decimal price)
        {
            var client = GetHttpClient();
            var content = JsonContent.Create(new { Name = name, Price = price, IsActive = true });
            var response = await client.PostAsync("Shipping", content);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Kargo firması eklendi.";
            else
                TempData["Error"] = "Kargo firması eklenemedi.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"Shipping/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Kargo firması silindi.";
            else
                TempData["Error"] = "Kargo firması silinemedi.";

            return RedirectToAction("Index");
        }
    }
}
