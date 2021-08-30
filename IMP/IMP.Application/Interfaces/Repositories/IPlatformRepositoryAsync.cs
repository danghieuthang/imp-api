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
        /// <summary>
        /// Check unique platform when add new
        /// </summary>
        /// <param name="platformName"></param>
        /// <returns></returns>
        Task<bool> IsUniquePlatform(string platformName, int? id=null);

    }
}
