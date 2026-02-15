using ETicaretAPI.Models.Entities;

namespace ETicaretAPI.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<(List<Product> Data, int TotalRecords)> GetProductsWithDetailsAsync(
            int pageNumber,
            int pageSize,
            string? search = null,
            int? categoryId = null,
            int? brandId = null);
    }
}
