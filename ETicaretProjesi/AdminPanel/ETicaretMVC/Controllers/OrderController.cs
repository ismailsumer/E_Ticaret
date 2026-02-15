using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;
using AdminPanel.ETicaretMVC.Models.DTOs;
using System.Text.Json;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> Index()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("Orders/all");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            var orders = new List<OrderViewDto>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<ServiceResponse<List<OrderViewDto>>>();
                orders = json?.Data ?? new();
            }
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("Orders/all");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<ServiceResponse<List<OrderViewDto>>>();
                var order = json?.Data?.FirstOrDefault(o => o.Id == id);
                if (order != null)
                    return View(order);
            }

            TempData["Error"] = "Sipariş bulunamadı.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var client = GetHttpClient();
            var content = JsonContent.Create(new { Status = status });
            var response = await client.PutAsync($"Orders/{id}/status", content);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Sipariş durumu güncellendi.";
            else
            {
                var errorJson = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();
                TempData["Error"] = errorJson?.Message ?? "Güncelleme başarısız.";
            }

            return RedirectToAction("Details", new { id });
        }
    }
}
