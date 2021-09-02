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
    public class ApplicationUserRepositoryAsync : GenericRepositoryAsync<int, ApplicationUser>, IApplicationUserRepositoryAsync
    {
        private readonly DbSet<ApplicationUser> _users;
        public ApplicationUserRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _users = dbContext.Set<ApplicationUser>();
        }

        public async Task<ApplicationUser> GetByUserName(string username)
        {
            var user = await _users.AsNoTracking().Where(x => x.UserName.Equals(username)).FirstOrDefaultAsync();
            return user;
        }
    }
}
