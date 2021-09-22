using IMP.Application.Interfaces;
using IMP.Infrastructure.EfCore.Repositories;
using IMP.Infrastructure.Identity.Contexts;
using IMP.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Identity.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly DbSet<RefreshToken> _refreshTokens;
        public RefreshTokenRepository(IdentityContext dbContext) : base(dbContext)
        {
            _refreshTokens = dbContext.Set<RefreshToken>();
        }
        public async Task<RefreshToken> GetRefreshToken(string token, string ipAddress)
        {
            var refreshToken = await _refreshTokens.AsNoTracking().Where(x => x.Token == token).Include(x => x.User).FirstOrDefaultAsync();
            return refreshToken;
        }
    }
}
