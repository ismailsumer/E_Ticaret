using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdminPanel.ETicaretMVC.Filters
{
    /// <summary>
    /// Global Action Filter: JWT token cookie'sini kontrol eder.
    /// Token yoksa veya geçersizse kullanıcıyı Login sayfasına yönlendirir.
    /// AuthController bu filtreden muaftır (AllowAnonymous gibi davranır).
    /// </summary>
    public class AdminAuthorizationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // AuthController'ı filtreden muaf tut
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            if (string.Equals(controllerName, "Auth", StringComparison.OrdinalIgnoreCase))
                return;

            // JWT token cookie kontrolü
            var token = context.HttpContext.Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            // Rol kontrolü - cookie'den oku
            var role = context.HttpContext.Request.Cookies["UserRole"];
            if (string.IsNullOrEmpty(role) || !string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Token var ama Admin değilse çıkış yap
                context.HttpContext.Response.Cookies.Delete("JwtToken");
                context.HttpContext.Response.Cookies.Delete("UserRole");
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Post-action işlemi gerekmiyor
        }
    }
}
