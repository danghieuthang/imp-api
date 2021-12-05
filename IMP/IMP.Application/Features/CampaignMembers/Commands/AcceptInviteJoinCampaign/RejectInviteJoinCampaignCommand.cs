using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignMembers.Commands.AcceptInviteJoinCampaign
{
    public class RejectInviteJoinCampaignCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public class RejectInviteJoinCampaignCommandHandler : CommandHandler<RejectInviteJoinCampaignCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public RejectInviteJoinCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<bool>> Handle(RejectInviteJoinCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaignMember = await UnitOfWork.Repository<CampaignMember>().GetByIdAsync(request.Id);
                if (campaignMember == null)
                {
                    throw new ValidationException(new ValidationError("id", "Không tồn tại."));
                }

                if (campaignMember.InfluencerId != _authenticatedUserService.ApplicationUserId)
                {
                    throw new ValidationException(new ValidationError("id", "Không có quyền."));
                }
                if (campaignMember.Status != (int)CampaignMemberStatus.Invited)
                {
                    throw new ValidationException(new ValidationError("id", "Bạn không trong trạng thái được mời."));
                }

                campaignMember.Status = (int)CampaignMemberStatus.RefuseInvitated;

                UnitOfWork.Repository<CampaignMember>().Update(campaignMember);

                await UnitOfWork.CommitAsync();
                return new Response<bool>(true);
            }
        }
    }
}
