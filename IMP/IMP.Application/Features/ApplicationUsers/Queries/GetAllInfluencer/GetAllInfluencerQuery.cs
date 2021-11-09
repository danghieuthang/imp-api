using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IMP.Application.Enums;
using Microsoft.AspNetCore.Mvc;

namespace IMP.Application.Features.ApplicationUsers.Queries.GetAllInfluencer
{
    public class GetAllInfluencerQuery : PageRequest, IListQuery<InfluencerViewModel>
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }

        public class GetAllInfluencerQueryHandler : ListQueryHandler<GetAllInfluencerQuery, InfluencerViewModel>
        {
            public GetAllInfluencerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IPagedList<InfluencerViewModel>>> Handle(GetAllInfluencerQuery request, CancellationToken cancellationToken)
            {
                string name = request.Name != null ? request.Name.ToLower().Trim() : "";
                var page = await UnitOfWork.Repository<ApplicationUser>().GetPagedList(
                        predicate: x => x.FirstName.ToLower().Contains(name) || x.LastName.ToLower().Contains(name) || x.Nickname.ToLower().Contains(name),
                        include: x => x.Include(y => y.Ranking).Include(y => y.InfluencerPlatforms),
                        pageIndex: request.PageIndex,
                        pageSize: request.PageSize,
                        orderBy: request.OrderField,
                        orderByDecensing: request.OrderBy == OrderBy.DESC);

                var pageView = page.ToResponsePagedList<InfluencerViewModel>(Mapper);
                return new Response<IPagedList<InfluencerViewModel>>(pageView);

            }
        }
    }
}
