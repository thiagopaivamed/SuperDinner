using SuperDinner.Domain.Handlers;
using SuperDinner.Infrastructure.Data.Context;

namespace SuperDinner.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SuperDinnerContext _superDinnerContext;
        private bool _disposed = false;

        public UnitOfWork(SuperDinnerContext superDinnerContext)
        {
            _superDinnerContext = superDinnerContext;
        }

        public async Task CommitAsync() 
            => await _superDinnerContext.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _superDinnerContext.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
