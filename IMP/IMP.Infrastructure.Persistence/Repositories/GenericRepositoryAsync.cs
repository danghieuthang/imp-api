﻿using IMP.Application.Enums;
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
using IMP.Application.Wrappers;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading;
using IMP.Application.Extensions;

namespace IMP.Infrastructure.Persistence.Repository
{

    public class GenericRepositoryAsync<TKey, TEntity> : IGenericRepositoryAsync<TKey, TEntity>, IDisposable where TEntity : Entity<TKey>
    {
        private readonly DbContext _dbContext;
        private bool _disposed = false;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(ApplicationDbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            var entity = await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            return entity;
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id, List<string> includeProperties)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            // include
            if (includeProperties is not null && includeProperties.Count > 0)
            {
                query = ApplyIncludes(query, includeProperties);
            }
            return await query.AsNoTracking().SingleOrDefaultAsync(x => x.Id.Equals(id));
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
            //_dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task AddManyAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            //_dbContext.Entry(entities).State = EntityState.Detached;
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
            var query = FindAll();
            return await query.AnyAsync(x => x.Id.Equals(id));
        }
        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = FindAll();
            return await query.AnyAsync(predicate);
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

        public async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await FindAll(predicate, includeProperties).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IPagedList<TEntity>> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int pageIndex = 0,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
        {
            var query = GetAll(predicate: predicate, orderBy: orderBy, include: include);
            return await query.ToPagedListAsync(pageIndex: pageIndex, pageSize: pageSize, cancellationToken: cancellationToken);
        }

        public async Task<IPagedList<TEntity>> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
        string orderBy = null,
        bool orderByDecensing = false,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int pageIndex = 0,
        int pageSize = 20, CancellationToken cancellationToken = default)
        {
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByFunc = null;

            if (orderBy != null)
            {
                if (orderByDecensing == false)
                {
                    orderByFunc = x => x.OrderBy(orderBy);
                }
                else
                {
                    orderByFunc = x => x.OrderBy(orderBy + " DESC");
                }
            }

            var query = GetAll(predicate: predicate, orderBy: orderByFunc, include: include);
            return await query.ToPagedListAsync(pageIndex: pageIndex, pageSize: pageSize, cancellationToken: cancellationToken);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var query = _dbSet.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            return query;
        }

        public IQueryable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var query = GetAll(predicate: predicate, orderBy: orderBy, include: include);
            return query.Select(selector);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await GetAll().CountAsync();
            }
            return await GetAll().Where(predicate).CountAsync();
        }


    }

    public class GenericRepositoryAsync<TEntity> : GenericRepositoryAsync<int, TEntity>, IGenericRepositoryAsync<TEntity> where TEntity : BaseEntity
    {
        public GenericRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }

}
