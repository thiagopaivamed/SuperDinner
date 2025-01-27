namespace SuperDinner.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public Task CommitAsync();
    }
}
