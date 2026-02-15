using ETicaretAPI.DTOs;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public interface IBrandService
    {
        Task<ServiceResponse<List<BrandViewDto>>> GetAllBrandsAsync();
        Task<ServiceResponse<BrandViewDto>> CreateBrandAsync(BrandCreateDto brandDto);
        Task<ServiceResponse<BrandViewDto>> UpdateBrandAsync(int id, BrandCreateDto brandDto);
        Task<ServiceResponse<string>> DeleteBrandAsync(int id);
    }
}
