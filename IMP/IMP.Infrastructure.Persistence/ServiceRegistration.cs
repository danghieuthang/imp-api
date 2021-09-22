using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Domain.Common;
using IMP.Infrastructure.EfCore;
using IMP.Infrastructure.Persistence.Contexts;
using IMP.Infrastructure.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            #region Repositories and UnitOfWork
            services.AddDefaultUnitOfWork<ApplicationDbContext>();
            #endregion

            #region services
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            #endregion
        }
    }
}
