using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Helpers
{
    public static class CommonHelper
    {
        /// <summary>
        /// Get dictionay from dynamic type(property name:propertyvalue)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DictionaryFromDynamic(dynamic obj)
        {
            if (obj == null) return new Dictionary<string, object>();

            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();

            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (var prop in props)
            {
                object value = prop.GetValue(obj, null);
                result.Add(prop.Name, value);
            }
            return result;
        }
    }
}
