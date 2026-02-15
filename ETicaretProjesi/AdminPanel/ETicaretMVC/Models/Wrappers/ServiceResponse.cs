namespace AdminPanel.ETicaretMVC.Models.Wrappers
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        // Başarılı cevaplar için
        public static ServiceResponse<T> SuccessResponse(T data, string message = "İşlem Başarılı")
        {
            return new ServiceResponse<T> { Data = data, Success = true, Message = message };
        }

        // Hatalı cevaplar için
        public static ServiceResponse<T> ErrorResponse(string message)
        {
            return new ServiceResponse<T> { Data = default, Success = false, Message = message };
        }
    }
}