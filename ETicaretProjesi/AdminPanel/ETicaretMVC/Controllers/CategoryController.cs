using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;
using System.Text.Json;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> Index()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("Categories");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CategoryViewModel>>>();
                if (result != null && result.Success)
                    return View(result.Data ?? new List<CategoryViewModel>());
            }

            return View(new List<CategoryViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Kategori adı boş olamaz.";
                return RedirectToAction("Index");
            }

            var client = GetHttpClient();
            var response = await client.PostAsJsonAsync("Categories", new { Name = name });

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Kategori eklenirken hata oluştu: {errorContent}";
            }
            else
            {
                TempData["Success"] = "Kategori başarıyla eklendi.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"Categories/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Kategori silinirken hata oluştu.";
            }

            return RedirectToAction("Index");
        }
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
