using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Helpers
{
    public static class LinqHelper
    {
        public static string GetReflectedPropertyValue<TEntity>(this TEntity entity, string name)
        {
            var prop = entity.GetType().GetProperty(name, BindingFlags.IgnoreCase).GetValue(entity, null);
            return prop != null ? prop.ToString() : "";
        }
    }
}
