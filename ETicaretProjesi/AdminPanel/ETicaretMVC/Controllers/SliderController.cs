using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;
using AdminPanel.ETicaretMVC.Models.DTOs;
using System.Net.Http.Headers;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class SliderController : BaseController
    {
        public SliderController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> Index()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("Sliders");

            var items = new List<SliderViewDto>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<ServiceResponse<List<SliderViewDto>>>();
                items = json?.Data ?? new();
            }
            return View(items);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string title, string description, string linkUrl, int displayOrder, IFormFile? imageFile)
        {
            var client = GetHttpClient();
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(title ?? ""), "Title");
            content.Add(new StringContent(description ?? ""), "Description");
            content.Add(new StringContent(linkUrl ?? ""), "LinkUrl");
            content.Add(new StringContent(displayOrder.ToString()), "DisplayOrder");
            content.Add(new StringContent("true"), "IsActive");

            if (imageFile != null && imageFile.Length > 0)
            {
                var streamContent = new StreamContent(imageFile.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType ?? "image/jpeg");
                content.Add(streamContent, "ImageFile", imageFile.FileName);
            }

            var response = await client.PostAsync("Sliders", content);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Slider eklendi.";
            else
                TempData["Error"] = "Slider eklenemedi.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"Sliders/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Slider silindi.";
            else
                TempData["Error"] = "Slider silinemedi.";

            return RedirectToAction("Index");
        }
    }
}
