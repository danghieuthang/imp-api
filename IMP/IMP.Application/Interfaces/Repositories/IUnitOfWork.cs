using IMP.Application.Interfaces;
using IMP.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces
{
    /// <summary>
    /// Define interface of generic unit of work
    /// </summary>
    public interface IUnitOfWork
    {
        void Dispose();
        void Dispose(bool disposing);
        void Commit();
        Task CommitAsync();
        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="hasCustomRepository"><c>True</c> if providing custom repositry</param>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IGenericRepository{TEntity}}"/> interface.</returns>
        IGenericRepository<TEntity> Repository<TEntity>(bool hasCustomRepository = false) where TEntity : BaseEntity;
        /// <summary>
        /// Gets the specified cache repository for the <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="hasCustomRepository"><c>True<c> if providibg custom cache repository</c>/param>
        /// <returns>An instance of type inherited from <see cref="ICachedRepository{TEntity}"/> interface.</returns>
        ICachedRepository<TEntity> CacheRepository<TEntity>(bool hasCustomRepository = false) where TEntity : BaseEntity;
        int ExecuteSqlCommand(string sql, params object[] parameters);
        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;
    }

    /// <summary>
    /// Define interface of generic unit of work
    /// </summary>
    /// <typeparam name="TContext">The db context.</typeparam>
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext DbContext { get; }
    }
}