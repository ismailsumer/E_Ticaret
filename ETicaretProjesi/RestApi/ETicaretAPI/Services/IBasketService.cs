using ETicaretAPI.DTOs;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public interface IBasketService
    {
        Task<ServiceResponse<List<BasketItemViewDto>>> GetUserBasketAsync(int userId);
        Task<ServiceResponse<bool>> AddToBasketAsync(int userId, BasketItemCreateDto basketDto);
        Task<ServiceResponse<bool>> ClearBasketAsync(int userId);
    }
}