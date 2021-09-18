using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMP.Domain.Common;

namespace IMP.Application.Interfaces
{
    public interface IRepositoryFactory
    {
        IGenericRepositoryAsync<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    }
}
