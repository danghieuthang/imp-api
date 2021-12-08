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
    public class CancelInviteJoinCampaignCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public class CancelInviteJoinCampaignCommandHandler : CommandHandler<CancelInviteJoinCampaignCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public CancelInviteJoinCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<bool>> Handle(CancelInviteJoinCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.Id == request.Id, x => x.Campaign);
                if (campaignMember == null)
                {
                    throw new ValidationException(new ValidationError("id", "Không tồn tại."));
                }

                if (campaignMember.Campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("id", "Không có quyền."));
                }
                if (campaignMember.Status != (int)CampaignMemberStatus.Invited)
                {
                    throw new ValidationException(new ValidationError("id", "Thành viên không trong trạng thái mời."));
                }
                UnitOfWork.Repository<CampaignMember>().Delete(campaignMember);
                await UnitOfWork.CommitAsync();
                return new Response<bool>(true);
            }
        }
    }
}
