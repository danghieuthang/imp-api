using AutoMapper;
using FluentValidation;
using IMP.Application.Enums;
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

namespace IMP.Application.Features.Evidences.Commands.CreateEvidence
{
    public class CreateEvidenceCommand : ICommand<EvidenceViewModel>
    {
        public int MemberActivityId { get; set; }
        public int EvidenceTypeId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public class CreateEvidenceCommandValidator : AbstractValidator<CreateEvidenceCommand>
        {
            public CreateEvidenceCommandValidator()
            {
                RuleFor(x => x.Url).MustValidUrl();
                RuleFor(x => x.Description).MustMaxLength(256);
            }
        }

        public class CreateEvidenceCommandHandler : CommandHandler<CreateEvidenceCommand, EvidenceViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public CreateEvidenceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<EvidenceViewModel>> Handle(CreateEvidenceCommand request, CancellationToken cancellationToken)
            {
                var memberActivity = await UnitOfWork.Repository<MemberActivity>().FindSingleAsync(x => x.Id == request.MemberActivityId, x => x.CampaignMember, x => x.CampaignActivity);

                if (memberActivity?.CampaignMember == null)
                {
                    throw new IMP.Application.Exceptions.ValidationException(new ValidationError("member_activity_id", "Không tồn tại."));
                }

                if (memberActivity.CampaignMember.InfluencerId != _authenticatedUserService.ApplicationUserId)
                {
                    throw new IMP.Application.Exceptions.ValidationException(new ValidationError("member_activity_id", "Không có quyền tạo."));
                }

                if (memberActivity.CampaignActivity.EvidenceTypeId != request.EvidenceTypeId)
                {
                    throw new IMP.Application.Exceptions.ValidationException(new ValidationError("evidence_type_id", "Bằng chứng không giống hoạt động yêu cầu."));
                }

                var evidence = await UnitOfWork.Repository<Evidence>().FindSingleAsync(
                    predicate: x => x.MemberActivityId == request.MemberActivityId);

                if (evidence == null) // create new evidence if not exist
                {
                    evidence = new Evidence();
                    evidence.EvidenceTypeId = request.EvidenceTypeId;
                    evidence.MemberActivityId = request.MemberActivityId;
                    evidence.Url = request.Url;
                    UnitOfWork.Repository<Evidence>().Add(evidence);
                }
                else // update if evidence of member activity exist
                {
                    evidence.EvidenceTypeId = request.EvidenceTypeId;
                    evidence.MemberActivityId = request.MemberActivityId;
                    evidence.Url = request.Url;
                    UnitOfWork.Repository<Evidence>().Update(evidence);
                }


                memberActivity.Status = (int)MemberActivityStatus.Waiting;
                UnitOfWork.Repository<MemberActivity>().Update(memberActivity);
                await UnitOfWork.CommitAsync();

                var evidenceVIew = Mapper.Map<EvidenceViewModel>(evidence);
                return new Response<EvidenceViewModel>(evidenceVIew);
            }
        }
    }
}
