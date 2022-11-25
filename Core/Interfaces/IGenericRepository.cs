using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.CoreEntities;

namespace Core.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> GetById(long id);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);

    }
}
