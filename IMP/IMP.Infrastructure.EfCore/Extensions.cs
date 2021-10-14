using System;
using System.Collections.Generic;
using IMP.Application.Interfaces;
using IMP.Domain.Common;
using IMP.Infrastructure.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
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


        public static object[] GetPrimaryKeyValues(this EntityEntry entry)
        {
            return entry.Metadata.FindPrimaryKey()
                .Properties
                .Select(p => entry.Property(p.Name).CurrentValue)
                .ToArray();
        }
        /// <summary>
        /// simple update method that will help you to do a full update to an aggregate graph with all related entities in it.
        /// the update method will take the loaded aggregate entity from the DB and the passed one that may come from the API layer.
        /// the method will update just the eager loaded entities in the aggregate "The included entities"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="newEntity">The De-Attached Entity</param>
        /// <param name="existingEntity">The Attached BD Entity</param>
        public static void InsertUpdateOrDeleteGraph<T>(this DbContext context, T newEntity, T existingEntity) where T : class
        {
            insertUpdateOrDeleteGraph(context, newEntity, existingEntity, null);
        }

        private static void insertUpdateOrDeleteGraph<T>(DbContext context, T newEntity, T existingEntity, string aggregateType) where T : class
        {
            if (existingEntity == null)
            {
                context.Add(newEntity);
            }
            else if (newEntity == null)
            {
                context.Remove(existingEntity);
            }
            else
            {
                var existingEntry = context.Entry(existingEntity);
                existingEntry.CurrentValues.SetValues(newEntity);

                foreach (var navigationEntry in existingEntry.Navigations.Where(n => n.IsLoaded && n.Metadata.ClrType.FullName != aggregateType))
                {
                    var passedNavigationObject = existingEntry.Entity.GetType().GetProperty(navigationEntry.Metadata.Name)?.GetValue(newEntity);

                    if (navigationEntry.Metadata.IsCollection)
                    {
                        // the navigation property is list
                        if (!(navigationEntry.CurrentValue is IEnumerable<object> existingNavigationObject))
                            throw new NullReferenceException($"Couldn't iterate through the DB value of the Navigation '{navigationEntry.Metadata.Name}'");

                        if (!(passedNavigationObject is IEnumerable<object> passedNavigationObjectEnumerable))
                            throw new NullReferenceException($"Couldn't iterate through the passed Navigation list of '{navigationEntry.Metadata.Name}'");

                        foreach (var newValue in passedNavigationObjectEnumerable)
                        {
                            var newId = context.Entry(newValue).GetPrimaryKeyValues();
                            var existingValue = existingNavigationObject.FirstOrDefault(v => context.Entry(v).GetPrimaryKeyValues().SequenceEqual(newId));
                            if (existingValue == null)
                            {
                                var addMethod = existingNavigationObject.GetType().GetMethod("Add");

                                if (addMethod == null)
                                    throw new NullReferenceException($"The collection type in the Navigation property '{navigationEntry.Metadata.Name}' doesn't have an 'Add' method.");

                                addMethod.Invoke(existingNavigationObject, new[] { newValue });
                            }

                            //Update sub navigation
                            insertUpdateOrDeleteGraph(context, newValue, existingValue, existingEntry.Metadata.ClrType.FullName);
                        }

                        foreach (var existingValue in existingNavigationObject.ToList())
                        {
                            var existingId = context.Entry(existingValue).GetPrimaryKeyValues();

                            if (passedNavigationObjectEnumerable.All(v => !context.Entry(v).GetPrimaryKeyValues().SequenceEqual(existingId)))
                            {
                                var addMethod = existingNavigationObject.GetType().GetMethod("Remove");

                                if (addMethod == null)
                                    throw new NullReferenceException($"The collection type in the Navigation property '{navigationEntry.Metadata.Name}' doesn't have an 'Remove' method.");

                                addMethod.Invoke(existingNavigationObject, new[] { existingValue });
                            }
                        }
                    }
                    else
                    {
                        // the navigation is not a list
                        insertUpdateOrDeleteGraph(context, passedNavigationObject, navigationEntry.CurrentValue, existingEntry.Metadata.ClrType.FullName);
                    }
                }
            }
        }
    }
}
