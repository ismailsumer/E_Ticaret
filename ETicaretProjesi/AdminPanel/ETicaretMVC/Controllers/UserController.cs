using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;
using AdminPanel.ETicaretMVC.Models.DTOs;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> Index()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("Users");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            var users = new List<UserViewDto>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<ServiceResponse<List<UserViewDto>>>();
                users = json?.Data ?? new();
            }
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(int id, string role)
        {
            var client = GetHttpClient();
            var content = JsonContent.Create(new { Role = role });
            var response = await client.PutAsync($"Users/{id}/role", content);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Kullanıcı rolü güncellendi.";
            else
            {
                var errorJson = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();
                TempData["Error"] = errorJson?.Message ?? "Rol güncellenemedi.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(string username, string password, string role)
        {
            var client = GetHttpClient();
            var content = JsonContent.Create(new { Username = username, Password = password, Role = role });
            var response = await client.PostAsync("Users", content);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Kullanıcı oluşturuldu.";
            else
            {
                var errorJson = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();
                TempData["Error"] = errorJson?.Message ?? "Kullanıcı oluşturulamadı.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"Users/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Kullanıcı silindi.";
            else
                TempData["Error"] = "Kullanıcı silinemedi.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"Users/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Kullanıcı bilgileri alınamadı.";
                return RedirectToAction("Index");
            }

            var json = await response.Content.ReadFromJsonAsync<ServiceResponse<UserViewDto>>();
            var user = json?.Data;
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            return PartialView("_UserDetailModal", user);
        }
    }
}
