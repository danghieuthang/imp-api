using IMP.Application.Interfaces;
using IMP.Application.Wrappers;
using IMP.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Infrastructure.EfCore.Repositories
{
    public class CachedRepository<TEntity> : GenericRepository<TEntity>, ICachedRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ICacheService _cacheService;
        public string CacheKey => typeof(TEntity).Name;


        public CachedRepository(ICacheService cacheService, DbContext dbContext) : base(dbContext)
        {
            _cacheService = cacheService;
        }

        public void RefreshCache()
        {
            _cacheService.Remove(CacheKey);
        }

        public override async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            if (!_cacheService.TryGet(CacheKey, out IReadOnlyList<TEntity> cacheList))
            {
                cacheList = await base.GetAllAsync();
                _cacheService.Set(CacheKey, cacheList);
            }
            return cacheList;
        }
    }
}