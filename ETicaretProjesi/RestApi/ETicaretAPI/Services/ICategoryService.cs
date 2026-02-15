using ETicaretAPI.DTOs;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<CategoryViewDto>>> GetAllCategoriesAsync();
        Task<ServiceResponse<CategoryViewDto>> GetCategoryByIdAsync(int id);
        Task<ServiceResponse<CategoryViewDto>> CreateCategoryAsync(CategoryCreateDto categoryDto);
        Task<ServiceResponse<CategoryViewDto>> UpdateCategoryAsync(int id, CategoryCreateDto categoryDto);
        Task<ServiceResponse<string>> DeleteCategoryAsync(int id);
    }
}