using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Interfaces;
using Data;
using Domain.CoreEntities;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DatabaseContext context;

        protected GenericRepository(DatabaseContext context)
        {
            this.context = context;
        }



        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<TEntity> GetById(long id)
        {
            return await context.Set<TEntity>()
                .SingleOrDefaultAsync(i => i.Id == id);
        }

        public virtual async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate) => await context.Set<TEntity>().FirstOrDefaultAsync(predicate);

        public virtual async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>()
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query.AsNoTracking();
        }

        public virtual async Task Add(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task AddRange(IEnumerable<TEntity> entities)
        {
            await context.Set<TEntity>().AddRangeAsync(entities);
        }
    }
}
