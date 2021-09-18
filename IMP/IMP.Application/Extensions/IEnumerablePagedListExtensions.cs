using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IMP.Application.Wrappers;

namespace IMP.Application.Extensions
{
    public static class IEnumerablePagedListExtensions
    {
        /// <summary>
        /// Converts the specified source to <see cref="PagedList{T}"/> by the specified <paramref name="pageIndex"/> and <paramref name="pageSize"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="mapper">The mapper to mapping Entity to View Model.</param>
        /// <param name="indexFrom">The start index value.</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.</returns>
        public static IPagedList<TEntity> ToPagedList<TEntity>(this IEnumerable<TEntity> source, int pageIndex, int pageSize,int indexFrom = 0)
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }
            int count = source.Count();
            var items = source.Skip((pageIndex - indexFrom) * pageSize).Take(pageSize).ToList();

            var pagedList = new PagedList<TEntity>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = count,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;

        }
    }
}

    