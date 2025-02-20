using Microsoft.EntityFrameworkCore;
using SuperDinner.Domain.Interfaces;
using SuperDinner.Domain.Responses;
using SuperDinner.Infrastructure.Data.Context;
using System.Linq.Expressions;

namespace SuperDinner.Infrastructure.Data.Repositories
{
    public class GenericRepository<TEntity>(SuperDinnerContext superDinnerContext) : IGenericRepository<TEntity> where TEntity : class
    {
        public void Delete(TEntity entity)
            => superDinnerContext.Set<TEntity>().Remove(entity);

        public async Task<PagedResponse<IReadOnlyList<TEntity>>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> conditions = null, params Expression<Func<TEntity, object>>[] objectsToBeIncluded)
        {
            IQueryable<TEntity> query = superDinnerContext.Set<TEntity>().AsNoTracking();

            if (objectsToBeIncluded is not null)
                query = objectsToBeIncluded.Aggregate(query, (current, include) => current.Include(include));

            if (conditions is not null)
                query = query.Where(conditions);

            IReadOnlyList<TEntity> entities = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int dataFoundCount = await query.CountAsync();

            return new PagedResponse<IReadOnlyList<TEntity>>(entities, dataFoundCount, pageNumber, pageSize);
        }

        public async Task<TEntity> GetByIdAsync(Guid entityId)
            => await superDinnerContext.Set<TEntity>().FindAsync(entityId);

        public async Task InsertAsync(TEntity entity)
            => await superDinnerContext.Set<TEntity>().AddAsync(entity);

        public void Update(TEntity entity)
            => superDinnerContext.Set<TEntity>().Update(entity);
    }
}
