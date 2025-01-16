using Microsoft.EntityFrameworkCore;
using SuperDinner.Domain.Handlers;
using SuperDinner.Infrastructure.Data.Context;
using System.Linq.Expressions;

namespace SuperDinner.Infrastructure.Data.Repositories
{
    public sealed class GenericRepository<TEntity>(SuperDinnerContext superDinnerContext) : IGenericRepository<TEntity> where TEntity : class
    {
        public async Task DeleteAsync(Guid entityId)
        {
            TEntity entity = await GetByIdAsync(entityId);
            superDinnerContext.Set<TEntity>().Remove(entity);
        }

        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> conditions = null, params Expression<Func<TEntity, object>>[] objectsToBeIncluded)
        {
            IQueryable<TEntity> entities = superDinnerContext.Set<TEntity>().AsNoTracking();

            if (objectsToBeIncluded is not null)
                entities = objectsToBeIncluded.Aggregate(entities, (current, include) => current.Include(include));

            if (conditions is not null)
                entities = entities.Where(conditions);

            return await entities.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid entityId)
            => await superDinnerContext.Set<TEntity>().FindAsync(entityId);


        public async Task InsertAsync(TEntity entity)
            => await superDinnerContext.Set<TEntity>().AddAsync(entity);

        public void Update(TEntity entity) => superDinnerContext.Set<TEntity>().Update(entity);
    }
}
