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

namespace IMP.Application.Features.Evidences.Commands.DeleteEvidence
{
    public class DeleteEvidenceCommand : IDeleteCommand<Evidence>
    {
        public int Id { get; set; }
        public class DeleteEvidenceCommandHandler : DeleteCommandHandler<Evidence, DeleteEvidenceCommand>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public DeleteEvidenceCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<int>> Handle(DeleteEvidenceCommand request, CancellationToken cancellationToken)
            {
                var evidence = await Repository.FindSingleAsync(x => x.Id == request.Id, x => x.MemberActivity, x => x.MemberActivity.CampaignMember);
                if (evidence == null)
                {
                    var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                    throw new ValidationException(error);
                }

                if (evidence.MemberActivity.CampaignMember.InfluencerId != _authenticatedUserService.ApplicationUserId)
                {
                    var error = new ValidationError("id", $"Không có quyền xóa.");
                    throw new ValidationException(error);
                }

                Repository.Delete(evidence);
                await UnitOfWork.CommitAsync();
                return new Response<int>(evidence.Id);
            }
        }
    }
}
