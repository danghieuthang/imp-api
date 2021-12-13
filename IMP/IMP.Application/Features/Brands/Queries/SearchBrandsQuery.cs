using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Brands.Queries
{
    public class SearchBrandsQuery : PageRequest, IListQuery<BrandViewModel>
    {
        public SearchBrandsQuery()
        {
            Statuses = new List<int>();
        }
        [FromQuery(Name = "status")]
        public List<int> Statuses { get; set; }
        public class SearchBrandsQueryHandler : ListQueryHandler<SearchBrandsQuery, BrandViewModel>
        {
            public SearchBrandsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IPagedList<BrandViewModel>>> Handle(SearchBrandsQuery request, CancellationToken cancellationToken)
            {
                var page = await UnitOfWork.Repository<Brand>().GetPagedList(
                            predicate: x => request.Statuses.Count == 0 || request.Statuses.Contains(x.Status),
                            orderBy: request.OrderField,
                            orderByDecensing: request.OrderBy == Enums.OrderBy.DESC,
                            pageIndex: request.PageIndex, pageSize: request.PageSize,
                            cancellationToken: cancellationToken);
                var pageView = page.ToResponsePagedList<BrandViewModel>(Mapper);
                return new Response<IPagedList<BrandViewModel>>(pageView);
            }
        }
    }
}
