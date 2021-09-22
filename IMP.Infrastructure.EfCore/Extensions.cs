using IMP.Application.Interfaces;
using IMP.Domain.Common;
using IMP.Infrastructure.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.EfCore
{
    public static class Extensions
    {
        /// <summary>
        /// Add default unit of work
        /// </summary>
        /// <typeparam name="TContext">The database context</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            return services;
        }

        /// <summary>
        /// Add Unit Of Work Of context
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext
        {
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
            where IRepository : class, IGenericRepository<TEntity>
        {
            services.AddScoped<IGenericRepository<TEntity>, IRepository>();
            return services;
        }
    }
}
