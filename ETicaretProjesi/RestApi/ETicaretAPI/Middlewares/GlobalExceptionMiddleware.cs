using System.Net;
using System.Text.Json;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Gelen isteği normal şekilde işle
                await _next(context);
            }
            catch (Exception error)
            {
                // Hatanın loglanmasını sağlıyoruz
                Serilog.Log.Error(error, "Bir hata oluştu: {Message}", error.Message);
                // Eğer bir hata patlarsa buraya düşecek
                await HandleExceptionAsync(context, error);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ServiceResponse<string>();

            switch (exception)
            {
                case FluentValidation.ValidationException validationEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    // Hataları birleştirip mesaj olarak dönüyoruz
                    var errors = string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage));
                    response = ServiceResponse<string>.ErrorResponse(errors);
                    break;
                
                case UnauthorizedAccessException:
                     context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                     response = ServiceResponse<string>.ErrorResponse("Bu işlem için yetkiniz yok.");
                     break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = ServiceResponse<string>.ErrorResponse($"Bir hata oluştu: {exception.Message}");
                    break;
            }

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}