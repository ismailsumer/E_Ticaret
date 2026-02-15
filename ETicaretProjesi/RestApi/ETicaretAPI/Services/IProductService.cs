using ETicaretAPI.DTOs;
using ETicaretAPI.Wrappers;

namespace ETicaretAPI.Services
{
    public interface IProductService
    {
        Task<PagedResponse<List<ProductViewDto>>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<PagedResponse<List<ProductViewDto>>> GetAllProductsAsync(int pageNumber, int pageSize, string? search, int? categoryId, int? brandId);
        Task<ServiceResponse<ProductViewDto>> GetProductByIdAsync(int id);
        Task<ServiceResponse<ProductViewDto>> AddProductAsync(ProductCreateDto productDto);
        Task<ServiceResponse<ProductViewDto>> UpdateProductAsync(int id, ProductUpdateDto productDto);
        Task<ServiceResponse<string>> DeleteProductAsync(int id);
        Task<ServiceResponse<string>> UploadImageAsync(int productId, IFormFile file);
    }
}
