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
        Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> IsExistAsync(TKey id);

        // Summary:
        //     Asynchronously determines whether all the elements of a sequence satisfy a condition.
        //
        // Parameters:
        //   predicate:
        //     A function to test each element for a condition.
        //
        //
        // Type parameters:
        //   TEntity:
        //     The type of the elements of source.
        //
        // Returns:
        //     A task that represents the asynchronous operation. The task result contains true
        //     if every element of the source sequence passes the test in the specified predicate;
        //     otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        // Remarks:
        //     Multiple active operations on the same context instance are not supported. Use
        //     'await' to ensure that any asynchronous operations have completed before calling
        //     another method on this context.
        Task<bool> IsRight(Expression<Func<TEntity, bool>> predicate);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<Tuple<IReadOnlyList<TEntity>, int>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<Tuple<IReadOnlyList<TEntity>, int>> GetPagedReponseAsync(int pageNumber, int pageSize, List<string> includes, string orderByField = null, OrderBy? orderBy = null);
        Task<Tuple<IReadOnlyList<TEntity>, int>> GetPagedReponseAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate, List<string> includes, string orderByField = null, OrderBy? orderBy = null);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void Dispose();
    }

    public interface IGenericRepositoryAsync<TEntity> : IGenericRepositoryAsync<int, TEntity> where TEntity : class
    {

    }
}
