using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces.Repositories
{
    public interface IPlatformRepositoryAsync : IGenericRepositoryAsync<int, Platform>
    {
        Task<bool> IsUniquePlatform(string platformName);
    }
}
