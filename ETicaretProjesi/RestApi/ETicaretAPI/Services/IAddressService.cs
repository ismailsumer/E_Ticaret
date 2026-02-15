using ETicaretAPI.DTOs;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public interface IAddressService
    {
        Task<ServiceResponse<List<AddressDto>>> GetUserAddressesAsync(int userId);
        Task<ServiceResponse<bool>> AddAddressAsync(int userId, AddressDto addressDto);
    }
}