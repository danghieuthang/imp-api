using IMP.Domain.Common;

namespace IMP.Application.Interfaces
{
    public interface ICachedRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        string CacheKey { get; }
        void RefreshCache();
    }

}