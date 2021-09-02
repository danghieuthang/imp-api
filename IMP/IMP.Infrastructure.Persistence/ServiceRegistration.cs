using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Interfaces.Services;
using IMP.Infrastructure.Persistence.Contexts;
using IMP.Infrastructure.Persistence.Repositories;
using IMP.Infrastructure.Persistence.Repository;
using IMP.Infrastructure.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<,>), typeof(GenericRepositoryAsync<,>));
            services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();
            services.AddTransient<ICampaignRepositoryAsync, CampaignRepositoryAsync>();
            services.AddTransient<IPlatformRepositoryAsync, PlatformRespositoryAsync>();
            services.AddTransient<IPlatformRepositoryAsync, PlatformRespositoryAsync>();
            services.AddTransient<ICampaignTypeRepositoryAsync, CampaignTypeRepositoryAsync>();
            services.AddTransient<IApplicationUserRepositoryAsync, ApplicationUserRepositoryAsync>();
            #endregion

            #region services
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            #endregion
        }
    }
}
