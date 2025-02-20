using SuperDinner.Domain.Interfaces;
using SuperDinner.Infrastructure.Data.Context;

namespace SuperDinner.Infrastructure.Data.Repositories
{
    public sealed class UnitOfWork(SuperDinnerContext superDinnerContext) : IUnitOfWork, IDisposable
    {
        private bool _disposed = false;        

        public async Task CommitAsync() 
            => await superDinnerContext.SaveChangesAsync();

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                superDinnerContext.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
