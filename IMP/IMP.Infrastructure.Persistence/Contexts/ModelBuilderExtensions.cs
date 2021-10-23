using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Contexts
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder EntitiesOfType<T>(this ModelBuilder modelBuilder,
       Action<EntityTypeBuilder> buildAction) where T : class
        {
            var type = typeof(T);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                if (type.IsAssignableFrom(entityType.ClrType))
                    buildAction(modelBuilder.Entity(entityType.ClrType));

            return modelBuilder;

        }
    }
}
