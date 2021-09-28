using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces
{
    /// <summary>
    /// Defines the interfaces for cache service
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Try get value from cache
        /// </summary>
        /// <typeparam name="T">The type of data get</typeparam>
        /// <param name="cacheKey">The  key</param>
        /// <param name="value">The return parameter</param>
        /// <returns></returns>
        bool TryGet<T>(string cacheKey, out T value);

        /// <summary>
        /// Set value to cache
        /// </summary>
        /// <typeparam name="T">The type of value set</typeparam>
        /// <param name="cacheKey">The key</param>
        /// <param name="value">The value set</param>
        /// <returns></returns>
        T Set<T>(string cacheKey, T value);

        /// <summary>
        /// Remove a value from cache
        /// </summary>
        /// <param name="cacheKey">The key</param>
        void Remove(string cacheKey);
    }
}
