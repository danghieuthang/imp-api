using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace IMP.Application.Wrappers
{
    /// <typeparam name="T">The type for paging.</typeparam>
    public interface IPagedList<T>
    {
        /// <summary>
        /// Gets the page index (current).
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// Gets the page size.
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// Gets the total count of the list of type <typeparamref name="T"/>
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// Gets the total pages.
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// Gets the current page items.
        /// </summary>
        IList<T> Items { get; }

        IPagedList<TViewModel> ToResponsePagedList<TViewModel>(IMapper mapper) where TViewModel : class;
    }
}
