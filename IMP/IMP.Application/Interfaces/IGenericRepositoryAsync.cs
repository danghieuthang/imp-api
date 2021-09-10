using IMP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces
{
    public interface IGenericRepositoryAsync<TKey, TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(TKey id);
        Task<TEntity> GetByIdAsync(TKey id, List<string> includeProperties);
        Task<bool> IsExistAsync(TKey id);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<Tuple<IReadOnlyList<TEntity>, int>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<Tuple<IReadOnlyList<TEntity>, int>> GetPagedReponseAsync(int pageNumber, int pageSize, List<string> includes, string orderByField = null, OrderBy? orderBy = null);
        Task<Tuple<IReadOnlyList<TEntity>, int>> GetPagedReponseAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate, List<string> includes, string orderByField = null, OrderBy? orderBy = null);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void Dispose();

    }
}
