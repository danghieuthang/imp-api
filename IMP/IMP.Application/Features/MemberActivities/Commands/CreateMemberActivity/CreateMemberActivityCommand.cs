using AutoMapper;
using FluentValidation;
using IMP.Application.Exceptions;
using IMP.Application.Extensions;
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
using ValidationException = IMP.Application.Exceptions.ValidationException;

namespace IMP.Application.Features.MemberActivities.Commands.CreateMemberActivity
{
    public class CreateMemberActivityCommand : ICommand<MemberActivityViewModel>
    {
        public int CampaignActivityId { get; set; }
        public int CampaignMemberId { get; set; }
        public class CreateMemberActivityCommandValidator : AbstractValidator<CreateMemberActivityCommand>
        {
            public CreateMemberActivityCommandValidator(IUnitOfWork unitOfWork)
            {
                RuleFor(x => x.CampaignMemberId).MustExistEntityId(async (id, c) =>
                {
                    return await unitOfWork.Repository<CampaignMember>().IsExistAsync(id);
                });

                RuleFor(x => x.CampaignActivityId).MustExistEntityId(async (id, c) =>
                {
                    return await unitOfWork.Repository<CampaignActivity>().IsExistAsync(id);
                });

            }
        }

        public class CreateMemberActivityCommandHandler : CommandHandler<CreateMemberActivityCommand, MemberActivityViewModel>
        {
            private readonly IGenericRepository<MemberActivity> _memberActivityRepository;
            public CreateMemberActivityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _memberActivityRepository = unitOfWork.Repository<MemberActivity>();
            }

            public override async Task<Response<MemberActivityViewModel>> Handle(CreateMemberActivityCommand request, CancellationToken cancellationToken)
            {
                var memberActivityExist = await _memberActivityRepository.IsExistAsync(x => x.CampaignMemberId == request.CampaignMemberId && x.CampaignActivityId == request.CampaignActivityId);
                if (memberActivityExist)
                {
                    throw new ValidationException(new ValidationError("", "Thành viên với hoạt động này đã tồn tại."));
                }

                var memberAcitity = new MemberActivity
                {
                    CampaignActivityId = request.CampaignActivityId,
                    CampaignMemberId = request.CampaignMemberId
                };
                _memberActivityRepository.Add(memberAcitity);
                await UnitOfWork.CommitAsync();

                var memberAcitityView = Mapper.Map<MemberActivityViewModel>(memberAcitity);
                return new Response<MemberActivityViewModel>(memberAcitityView);
            }
        }
    }
}
