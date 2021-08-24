using IMP.Application.Interfaces.Repositories;
using IMP.Domain.Entities;
using IMP.Infrastructure.Persistence.Contexts;
using IMP.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Infrastructure.Persistence.Repositories
{
    public class CampaignRepositoryAsync : GenericRepositoryAsync<int, Campaign>, ICampaignRepositoryAsync
    {
        private readonly DbSet<Campaign> _campaigns;
        public CampaignRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _campaigns = dbContext.Set<Campaign>();
        }
    }
}
