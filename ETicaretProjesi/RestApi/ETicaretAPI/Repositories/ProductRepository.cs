using ETicaretAPI.Data;
using ETicaretAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ETicaretContext context) : base(context)
        {
        }

        public async Task<(List<Product> Data, int TotalRecords)> GetProductsWithDetailsAsync(
            int pageNumber,
            int pageSize,
            string? search = null,
            int? categoryId = null,
            int? brandId = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Name.ToLower().Contains(search.ToLower()) ||
                    x.Description.ToLower().Contains(search.ToLower()));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(x => x.BrandId == brandId.Value);
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }
    }
}
