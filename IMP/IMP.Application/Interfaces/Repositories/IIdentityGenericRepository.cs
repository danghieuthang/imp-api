using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces.Repositories.Identities
{
    public interface IIdentityGenericRepository<TKey, TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void Dispose();
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TKey id);
        Task<bool> IsExistAsync(TKey id);
        Task UpdateAsync(TEntity entity);
    }
}
