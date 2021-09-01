using IMP.Application.Interfaces.Repositories.Identities;
using IMP.Infrastructure.Identity.Contexts;
using IMP.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Identity.Reponsitories
{
    public interface IRefreshTokenRepository : IIdentityGenericRepository<int, RefreshToken>
    {
    }
    public class RefreshTokenRepository : IdentityGenericRepository<int, RefreshToken>, IRefreshTokenRepository
    {
        private readonly DbSet<RefreshToken> _refreshTokens;
        public RefreshTokenRepository(IdentityContext dbContext) : base(dbContext)
        {
            _refreshTokens = dbContext.Set<RefreshToken>();
        }
    }
}
