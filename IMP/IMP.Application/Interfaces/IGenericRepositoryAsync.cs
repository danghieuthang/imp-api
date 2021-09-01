using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces
{
    public interface IGenericRepositoryAsync<TKey, TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(TKey id);
        Task<bool> IsExistAsync(TKey id);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<IReadOnlyList<TEntity>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void Dispose();

    }
}
