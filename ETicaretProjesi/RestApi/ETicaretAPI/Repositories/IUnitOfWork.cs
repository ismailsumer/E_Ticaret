namespace ETicaretAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        IProductRepository Products { get; }
        Task<int> SaveChangesAsync();
    }
}