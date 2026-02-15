using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        
        public BaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient GetHttpClient()
        {
            var client = _httpClientFactory.CreateClient("ETicaretAPI");
            
            // Cookie'den token oku ve header'a ekle
            if (Request.Cookies.TryGetValue("JwtToken", out var token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            return client;
        }
    }
}