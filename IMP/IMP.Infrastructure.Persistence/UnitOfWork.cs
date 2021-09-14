using IMP.Application.Interfaces;
using IMP.Domain.Common;
using IMP.Infrastructure.Persistence.Contexts;
using IMP.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;
        private Dictionary<string, object> _repositoties;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }



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

        public IGenericRepositoryAsync<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositoties == null)
            {
                _repositoties = new Dictionary<string, object>();
            }
            var type = typeof(TEntity).Name;
            if (!_repositoties.ContainsKey(type))
            {
                var respositoryInstance = new GenericRepositoryAsync<TEntity>(_context);
                _repositoties.Add(type, respositoryInstance);
            }
            return (IGenericRepositoryAsync<TEntity>)_repositoties[type];
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
