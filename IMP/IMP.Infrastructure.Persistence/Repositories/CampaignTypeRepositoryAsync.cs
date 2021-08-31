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
    public class CampaignTypeRepositoryAsync : GenericRepositoryAsync<int, CampaignType>, ICampaignTypeRepositoryAsync
    {
        private readonly DbSet<CampaignType> _campaignTypes;
        public CampaignTypeRepositoryAsync(ApplicationDbContext context) : base(context)
        {
            _campaignTypes = context.Set<CampaignType>();
        }
        public async Task<bool> IsUniqueCampaignType(string name, int? id = null)
        {
            if (id == null)
            {
                return await _campaignTypes.AllAsync(p => p.Name != name);
            }
            return await _campaignTypes.AllAsync(p => (p.Id == id.Value && p.Name == name) || p.Name != name);
        }
    }
}
