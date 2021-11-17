using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.MemberActivities.Queries.GetMemberActivityById
{
    public class GetMemberActivityByIdQuery : IGetByIdQuery<MemberActivity, MemberActivityViewModel>
    {
        public int Id { get; set; }
        public class GetMemberActivityByIdQueryHandler : GetByIdQueryHandler<GetMemberActivityByIdQuery, MemberActivity, MemberActivityViewModel>
        {
            public GetMemberActivityByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<MemberActivityViewModel>> Handle(GetMemberActivityByIdQuery request, CancellationToken cancellationToken)
            {
                var memberActivity = await Repository.FindSingleAsync(x => x.Id == request.Id, x => x.Evidences, x => x.ActivityComments);
                if (memberActivity == null)
                {
                    throw new KeyNotFoundException();
                }

                var viewModel = Mapper.Map<MemberActivityViewModel>(memberActivity);
                return new Response<MemberActivityViewModel>(viewModel);
            }
        }
    }
}
