using IMP.Application.Interfaces;
using IMP.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;  
using System.Threading.Tasks;

namespace IMP.Infrastructure.EfCore.Repositories
{
    public class UnitOfWork<TContext> : IDisposable, IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private bool _disposed;
        private Dictionary<string, object> _repositoties;


        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public TContext DbContext => _context;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public IGenericRepository<TEntity> Repository<TEntity>(bool hasCustomRepository = false) where TEntity : BaseEntity
        {
            if (_repositoties == null)
            {
                _repositoties = new Dictionary<string, object>();
            }
            if (hasCustomRepository)
            {
                var repository = _context.GetService<IGenericRepository<TEntity>>();
            }
            var type = typeof(TEntity).Name;
            if (!_repositoties.ContainsKey(type))
            {
                var respositoryInstance = new GenericRepository<TEntity>(_context);
                _repositoties.Add(type, respositoryInstance);
            }
            return (IGenericRepository<TEntity>)_repositoties[type];
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters) => _context.Database.ExecuteSqlRaw(sql, parameters);
        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class => _context.Set<TEntity>().FromSqlRaw(sql, parameters);

    }
}
