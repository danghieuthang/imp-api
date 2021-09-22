using IMP.Domain.Common;

namespace IMP.Application.Interfaces
{
    public interface ICachedRepository<TEntity> where TEntity : BaseEntity
    {
        string CacheKey { get; set; }
        void InvalidateCache();
        void InsertIntoCache();
    }

}