using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;
using AdminPanel.ETicaretMVC.Models.DTOs;
using System.Text.Json;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();
            var client = GetHttpClient();
            try
            {
                var response = await client.GetAsync("Dashboard/stats");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadFromJsonAsync<ServiceResponse<DashboardViewModel>>();
                    if (json?.Data != null)
                        model = json.Data;
                }
            }
            catch { }

            return View(model);
        }
    }
}
