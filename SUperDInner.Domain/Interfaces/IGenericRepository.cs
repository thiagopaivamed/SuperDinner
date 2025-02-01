using System.Linq.Expressions;

namespace SuperDinner.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task InsertAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> conditions = null, params Expression<Func<TEntity, object>>[] objectsToBeIncluded);
        Task<TEntity> GetByIdAsync(Guid entityId);

    }
}
