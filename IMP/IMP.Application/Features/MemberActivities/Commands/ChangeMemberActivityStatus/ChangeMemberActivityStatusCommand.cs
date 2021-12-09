using AutoMapper;
using FluentValidation;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
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

namespace IMP.Application.Features.MemberActivities.Commands.ChangeMemberActivityStatus
{
    public class ChangeMemberActivityStatusCommand : ICommand<MemberActivityViewModel>
    {
        public int Id { get; set; }
        public MemberActivityStatus Status { get; set; }
        public class ChangeMemberAcitivytStatusCommandValidator : AbstractValidator<ChangeMemberActivityStatusCommand>
        {
            public ChangeMemberAcitivytStatusCommandValidator()
            {
                RuleFor(x => x.Status).IsInEnum().WithMessage("Trạng thái không hợp lệ.");
            }
        }

        public class ChangeMemberActivityStatusCommandHandler : CommandHandler<ChangeMemberActivityStatusCommand, MemberActivityViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IGenericRepository<MemberActivity> _memberActivityRepository;
            private readonly INotificationService _notificationService;
            public ChangeMemberActivityStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService, INotificationService notificationService) : base(unitOfWork, mapper)
            {
                _memberActivityRepository = unitOfWork.Repository<MemberActivity>();
                _authenticatedUserService = authenticatedUserService;
                _notificationService = notificationService;
            }

            public override async Task<Response<MemberActivityViewModel>> Handle(ChangeMemberActivityStatusCommand request, CancellationToken cancellationToken)
            {
                var memberActivity = await _memberActivityRepository.FindSingleAsync(x => x.Id == request.Id, x => x.CampaignMember, x => x.CampaignMember.Campaign);
                if (memberActivity == null)
                {
                    throw new IMP.Application.Exceptions.ValidationException(new ValidationError("id", "Không tồn tại."));
                }

                if (memberActivity.CampaignMember.Campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new IMP.Application.Exceptions.ValidationException(new ValidationError("id", "Không có quyền."));
                }

                if (memberActivity.Status == (int)MemberActivityStatus.Completed)
                {
                    memberActivity.CampaignMember.Status = (int)CampaignMemberStatus.Completed; // change status of campaign member
                    // Update money earning after completed activity
                    var campaignRewards = await UnitOfWork.Repository<CampaignReward>().GetAll(predicate: x => x.IsDefaultReward && x.CampaignId == memberActivity.CampaignMember.CampaignId).ToListAsync();
                    //memberActivity.CampaignMember.Money += campaignRewards.Sum(x => x.Price);
                    memberActivity.CampaignMember.CompletedDate = DateTime.UtcNow;

                    UnitOfWork.Repository<CampaignMember>().Update(memberActivity.CampaignMember);
                }

                memberActivity.Status = (int)request.Status;
                _memberActivityRepository.Update(memberActivity);
                await UnitOfWork.CommitAsync();
                if (request.Status == MemberActivityStatus.Completed)
                {
                    await _notificationService.PutNotication(memberActivity.CampaignMember.InfluencerId, memberActivity.Id, NotificationType.BrandApprovedMemberActivity);
                }
                else
                {
                    await _notificationService.PutNotication(memberActivity.CampaignMember.InfluencerId, memberActivity.Id, NotificationType.BrandRejectMemberActivity);
                }
                var memberActivityView = Mapper.Map<MemberActivityViewModel>(memberActivity);
                return new Response<MemberActivityViewModel>(memberActivityView);
            }
        }
    }
}
