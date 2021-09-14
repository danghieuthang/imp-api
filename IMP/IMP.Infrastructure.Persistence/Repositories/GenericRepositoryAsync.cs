using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Domain.Common;
using IMP.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

using IMP.Infrastructure.Persistence.Helpers;


namespace IMP.Infrastructure.Persistence.Repository
{

    public class GenericRepositoryAsync<TKey, TEntity> : IGenericRepositoryAsync<TKey, TEntity>, IDisposable where TEntity : Entity<TKey>
    {
        private readonly ApplicationDbContext _dbContext;
        private bool _disposed = false;
        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id, List<string> includeProperties)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            // include
            if (includeProperties is not null && includeProperties.Count > 0)
            {
                query = ApplyIncludes(query, includeProperties);
            }
            return await query.SingleOrDefaultAsync(x => x.Id.Equals(id));
        }



        public async Task<Tuple<IReadOnlyList<TEntity>, int>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            var query = GetAll();
            var count = await query.CountAsync();
            // paging
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new Tuple<IReadOnlyList<TEntity>, int>(await query.AsNoTracking().ToListAsync(), count);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public async Task AddManyAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _dbContext
                 .Set<TEntity>()
                 .ToListAsync();
        }

        public async Task<bool> IsExistAsync(TKey id)
        {
            return await GetByIdAsync(id) != null;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this._dbContext.Dispose();
                }
            }
            _disposed = true;
        }
        private IQueryable<TEntity> GetAll(List<string> includes = null, string orderField = null, OrderBy? orderBy = null)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            // order
            if (orderField is not null)
            {
                if (orderBy.HasValue && orderBy.Value == OrderBy.DESC)
                {
                    orderField = orderField + " DESC";
                }
                query = query.OrderBy(orderField);
            }

            // include
            if (includes is not null && includes.Count > 0)
            {
                query = ApplyIncludes(query, includes);
            }
            return query;
        }
        public async Task<Tuple<IReadOnlyList<TEntity>, int>> GetPagedReponseAsync(int pageNumber, int pageSize, List<string> includes, string orderField = null, OrderBy? orderBy = null)
        {
            var query = GetAll(includes, orderField, orderBy);
            var count = await query.CountAsync();
            // paging
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new Tuple<IReadOnlyList<TEntity>, int>(await query.AsNoTracking().ToListAsync(), count);
        }

        public async Task<Tuple<IReadOnlyList<TEntity>, int>> GetPagedReponseAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate, List<string> includes, string orderField = null, OrderBy? orderBy = null)
        {
            var query = GetAll(includes, orderField, orderBy).Where(predicate); ;
            var count = await query.CountAsync();
            // paging
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new Tuple<IReadOnlyList<TEntity>, int>(await query.AsNoTracking().ToListAsync(), count);
        }


        public IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return query;
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = FindAll(includeProperties);
            return query.Where(predicate);
        }

        public async Task<IReadOnlyList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = FindAll(predicate, includeProperties);
            return await query.AsNoTracking().ToListAsync();
        }

        private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> entities, List<string> includes)
        {
            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }
            return entities;
        }

        public async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await FindAll(predicate).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<bool> IsRight(Expression<Func<TEntity, bool>> predicate)
        {
            return await FindAll(predicate).AllAsync(predicate);
        }

    }

    public class GenericRepositoryAsync<TEntity> : GenericRepositoryAsync<int, TEntity>, IGenericRepositoryAsync<TEntity> where TEntity : BaseEntity
    {
        public GenericRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }

}
