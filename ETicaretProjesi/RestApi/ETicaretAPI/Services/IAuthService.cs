using ETicaretAPI.DTOs;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(UserRegisterDto request);
        Task<ServiceResponse<LoginResponseDto>> Login(UserLoginDto request);
        Task<ServiceResponse<string>> ChangePassword(int userId, ChangePasswordDto dto);
    }
}