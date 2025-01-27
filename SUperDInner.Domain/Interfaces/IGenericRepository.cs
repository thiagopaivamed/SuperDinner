using System.Linq.Expressions;

namespace SuperDinner.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task InsertAsync(TEntity entity);
        void Update(TEntity entity);
        Task DeleteAsync(Guid entityId);
        Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> conditions = null, params Expression<Func<TEntity, object>>[] objectsToBeIncluded);
        Task<TEntity> GetByIdAsync(Guid entityId);

    }
}
