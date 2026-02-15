using ETicaretAPI.Data;

namespace ETicaretAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ETicaretContext _context;
        public IProductRepository Products { get; private set; }

        public UnitOfWork(ETicaretContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}