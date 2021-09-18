using IMP.Application.Interfaces;
using IMP.Domain.Common;
using System;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces
{
    public interface IUnitOfWork : IRepositoryFactory
    {
        void Dispose();
        void Dispose(bool disposing);
        void Commit();
        Task CommitAsync();
    }
}