using ETicaretAPI.DTOs;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public interface IOrderService
    {
        Task<ServiceResponse<int>> CreateOrderAsync(int userId);
        Task<ServiceResponse<List<OrderViewDto>>> GetUserOrdersAsync(int userId);
        Task<ServiceResponse<List<OrderViewDto>>> GetAllOrdersAsync();
        Task<ServiceResponse<string>> UpdateOrderStatusAsync(int orderId, string status);
    }
}