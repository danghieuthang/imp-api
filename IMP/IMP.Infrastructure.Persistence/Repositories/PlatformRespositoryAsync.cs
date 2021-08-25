using IMP.Application.Interfaces.Repositories;
using IMP.Domain.Entities;
using IMP.Infrastructure.Persistence.Contexts;
using IMP.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Repositories
{
    public class PlatformRespositoryAsync : GenericRepositoryAsync<int, Platform>, IPlatformRepositoryAsync
    {
        private readonly DbSet<Platform> _platforms;
        public PlatformRespositoryAsync(ApplicationDbContext context) : base(context)
        {
            _platforms = context.Set<Platform>();
        }

        public Task<bool> IsUniquePlatform(string platformName)
        {
            return _platforms.AllAsync(p => p.Name != platformName);
        }
    }
}
