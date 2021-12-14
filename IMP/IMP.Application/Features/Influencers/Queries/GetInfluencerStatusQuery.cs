using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Influencers.Queries
{
    public class GetInfluencerStatusQuery : IQuery<int>
    {
        public class GetInfluencerStatusQueryHandler : QueryHandler<GetInfluencerStatusQuery, int>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetInfluencerStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<int>> Handle(GetInfluencerStatusQuery request, CancellationToken cancellationToken)
            {
                var user =await UnitOfWork.Repository<ApplicationUser>().GetByIdAsync(_authenticatedUserService.ApplicationUserId);
                if (user != null)
                {
                    return new Response<int>(user.Status);
                }
                throw new KeyNotFoundException();
            }
        }
    }
}
