using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;

namespace IMP.Application.Features.Pages.Queries.GetAllPageOfInfluencer
{
    public class GetAllPageOfInfluencerQuery : IGetAllQuery<PageViewModel>
    {
        public int InfluencerId { get; set; }
        public class GetAllPageOfInfluencerQueryHandler : GetAllQueryHandler<GetAllPageOfInfluencerQuery, Page, PageViewModel>
        {
            public GetAllPageOfInfluencerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<PageViewModel>>> Handle(GetAllPageOfInfluencerQuery request, CancellationToken cancellationToken)
            {
                var entities = await _repositoryAsync.FindAllAsync(x => x.InfluencerId == request.InfluencerId);
                var views = _mapper.Map<IEnumerable<PageViewModel>>(entities);
                return new Response<IEnumerable<PageViewModel>>(views);
            }
        }
    }
}