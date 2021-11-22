using AutoMapper;
using FluentValidation;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
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
            public ChangeMemberActivityStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _memberActivityRepository = unitOfWork.Repository<MemberActivity>();
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<MemberActivityViewModel>> Handle(ChangeMemberActivityStatusCommand request, CancellationToken cancellationToken)
            {
                var memberActivity = await _memberActivityRepository.FindSingleAsync(x => x.Id == request.Id, x => x.CampaignActivity, x => x.CampaignActivity.Campaign);
                if (memberActivity == null)
                {
                    throw new IMP.Application.Exceptions.ValidationException(new ValidationError("id", "Không tồn tại."));
                }

                if (memberActivity.CampaignActivity.Campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new IMP.Application.Exceptions.ValidationException(new ValidationError("id", "Không có quyền."));
                }

                memberActivity.Status = (int)request.Status;
                _memberActivityRepository.Update(memberActivity);
                await UnitOfWork.CommitAsync();

                var memberActivityView = Mapper.Map<MemberActivityViewModel>(memberActivity);
                return new Response<MemberActivityViewModel>(memberActivityView);
            }
        }
    }
}
