using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;
using AdminPanel.ETicaretMVC.Models.DTOs;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class CommentController : BaseController
    {
        public CommentController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> Index()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("Comments/all");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            var comments = new List<CommentViewDto>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CommentViewDto>>>();
                comments = json?.Data ?? new();
            }
            return View(comments);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"Comments/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Yorum silindi.";
            else
                TempData["Error"] = "Yorum silinemedi.";

            return RedirectToAction("Index");
        }
    }
}
