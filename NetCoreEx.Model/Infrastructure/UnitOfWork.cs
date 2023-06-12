namespace NetCoreEx.Model.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly NetCoreExDbContext _context;

        public UnitOfWork(NetCoreExDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}