namespace SuperDinner.Domain.Handlers
{
    public interface IUnitOfWork
    {
        public Task CommitAsync();

        public void Dispose();
    }
}
