using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;
using AdminPanel.ETicaretMVC.Models.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<IActionResult> Index()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("products");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            if (response.IsSuccessStatusCode)
            {
                var serviceResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductViewDto>>>();
                if (serviceResponse != null && serviceResponse.Success)
                    return View(serviceResponse.Data);
            }

            return View(new List<ProductViewDto>());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var client = GetHttpClient();

            try
            {
                var catResponse = await client.GetFromJsonAsync<ServiceResponse<List<CategoryDto>>>("categories");
                var brandResponse = await client.GetFromJsonAsync<ServiceResponse<List<BrandDto>>>("brands");

                ViewBag.Categories = catResponse?.Data ?? new List<CategoryDto>();
                ViewBag.Brands = brandResponse?.Data ?? new List<BrandDto>();
            }
            catch
            {
                ViewBag.Categories = new List<CategoryDto>();
                ViewBag.Brands = new List<BrandDto>();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen tüm alanları doğru doldurunuz.";
                return await Create();
            }

            var client = GetHttpClient();

            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.Name ?? ""), "Name");
            content.Add(new StringContent(model.Description ?? ""), "Description");
            content.Add(new StringContent(model.Price.ToString(System.Globalization.CultureInfo.InvariantCulture)), "Price");
            content.Add(new StringContent(model.Stock.ToString()), "Stock");
            content.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(model.BrandId.ToString()), "BrandId");

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileContent = new StreamContent(model.ImageFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.ImageFile.ContentType ?? "application/octet-stream");
                content.Add(fileContent, "ImageFile", model.ImageFile.FileName);
            }

            var response = await client.PostAsync("products", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Ürün başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                // API'den dönen hata mesajını oku
                try
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ServiceResponse<object>>(errorJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    TempData["Error"] = errorResponse?.Message ?? $"Ürün eklenirken hata oluştu. (HTTP {(int)response.StatusCode})";
                }
                catch
                {
                    TempData["Error"] = $"Ürün eklenirken hata oluştu. (HTTP {(int)response.StatusCode})";
                }
            }

            return await Create();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"products/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Ürün silinirken hata oluştu.";
            }
            else
            {
                TempData["Success"] = "Ürün başarıyla silindi.";
            }

            return RedirectToAction("Index");
        }
    }
}