using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Domain.Common;
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
            services.AddUnitOfWork<ApplicationDbContext>();
            #endregion

            #region services
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            #endregion
        }

        /// <summary>
        /// Add default unit of work
        /// </summary>
        /// <typeparam name="TContext">The database context</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
            return services;
        }

        /// <summary>
        /// Register custom repository
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="IRepository">The type of custom repository.</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomRepository<TEntity, IRepository>(this IServiceCollection services)
            where TEntity : BaseEntity
            where IRepository : class, IGenericRepositoryAsync<TEntity>
        {
            services.AddScoped<IGenericRepositoryAsync<TEntity>, IRepository>();
            return services;
        }
    }
}
