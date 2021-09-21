using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMP.Application.Wrappers
{
    public class PagedList<T> : IPagedList<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public IList<T> Items { get; set; }
        public PagedList()
        {

        }

        public IPagedList<TViewModel> ToResponsePagedList<TViewModel>(IMapper mapper) where TViewModel : class
        {
            int pageSize = this.PageSize;
            int pageIndex = this.PageIndex;
            int totalCount = this.TotalCount;
            int totalPages = this.TotalPages;
            return new PagedList<TViewModel>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = mapper.Map<IList<TViewModel>>(this.Items)
            };
        }
    }
}
