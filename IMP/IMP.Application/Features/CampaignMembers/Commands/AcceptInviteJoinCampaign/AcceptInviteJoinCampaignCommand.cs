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
    public class AcceptInviteJoinCampaignCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public class AcceptInviteJoinCampaignCommandHandler : CommandHandler<AcceptInviteJoinCampaignCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public AcceptInviteJoinCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<bool>> Handle(AcceptInviteJoinCampaignCommand request, CancellationToken cancellationToken)
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

                campaignMember.Status = (int)CampaignMemberStatus.Approved;

                var campaignActivities = UnitOfWork.Repository<CampaignActivity>().GetAll(predicate: x => x.CampaignId == campaignMember.CampaignId).ToList();
                var memberActivityRepository = UnitOfWork.Repository<MemberActivity>();
                if (!await UnitOfWork.Repository<MemberActivity>().IsExistAsync(x => x.CampaignMemberId == campaignMember.Id))
                {
                    foreach (var campaignActivity in campaignActivities)
                    {
                        memberActivityRepository.Add(new MemberActivity
                        {
                            CampaignActivityId = campaignActivity.Id,
                            CampaignMemberId = campaignMember.Id,
                            Status = (int)MemberActivityStatus.NotYet,
                        });
                    }
                }
                campaignMember.ApprovedDate = DateTime.UtcNow;
                UnitOfWork.Repository<CampaignMember>().Update(campaignMember);

                await UnitOfWork.CommitAsync();
                return new Response<bool>(true);
            }
        }
    }
}
