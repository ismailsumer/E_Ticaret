using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;
using System.Text.Json;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class BrandController : BaseController
    {
        public BrandController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> Index()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("Brands");
            
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<BrandViewModel>>>();
                if (result != null && result.Success)
                    return View(result.Data ?? new List<BrandViewModel>());
            }

            return View(new List<BrandViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Marka adı boş olamaz.";
                return RedirectToAction("Index");
            }

            var client = GetHttpClient();
            var response = await client.PostAsJsonAsync("Brands", new { Name = name });

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Marka eklenirken hata oluştu: {errorContent}";
            }
            else
            {
                TempData["Success"] = "Marka başarıyla eklendi.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"Brands/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Marka silinirken hata oluştu.";
            }

            return RedirectToAction("Index");
        }
    }

    public class BrandViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
