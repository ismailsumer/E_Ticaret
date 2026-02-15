using AdminPanel.ETicaretMVC.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using AdminPanel.ETicaretMVC.Models.Wrappers;

namespace AdminPanel.ETicaretMVC.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        [HttpGet]
        public IActionResult Login()
        {
            // Zaten giriş yapmışsa ana sayfaya yönlendir
            if (Request.Cookies.ContainsKey("JwtToken") && Request.Cookies.ContainsKey("UserRole"))
            {
                var role = Request.Cookies["UserRole"];
                if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
                    return RedirectToAction("Index", "Product");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var client = _httpClientFactory.CreateClient("ETicaretAPI");
            var response = await client.PostAsJsonAsync("auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<LoginResponseDto>>();
                if (result != null && result.Success && result.Data != null)
                {
                    // Admin rolü kontrolü
                    if (!string.Equals(result.Data.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        ViewBag.Error = "Bu panele sadece Admin rolüne sahip kullanıcılar giriş yapabilir.";
                        return View(loginDto);
                    }

                    // Token'ı güvenli bir şekilde sakla
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Development'da HTTP kullanıldığı için false
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.Now.AddDays(1)
                    };

                    Response.Cookies.Append("JwtToken", result.Data.Token, cookieOptions);
                    Response.Cookies.Append("UserRole", result.Data.Role, cookieOptions);
                    Response.Cookies.Append("Username", result.Data.Username, cookieOptions);

                    return RedirectToAction("Index", "Product");
                }
            }

            ViewBag.Error = "Giriş başarısız. Lütfen bilgilerinizi kontrol edin.";
            return View(loginDto);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");
            Response.Cookies.Delete("UserRole");
            Response.Cookies.Delete("Username");
            return RedirectToAction("Login");
        }
    }
}