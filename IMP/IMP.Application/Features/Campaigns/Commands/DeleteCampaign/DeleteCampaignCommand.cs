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

namespace IMP.Application.Features.Campaigns.Commands.DeleteCampaign
{
    public class DeleteCampaignCommand : IDeleteCommand<Campaign>
    {
        public int Id { get; set; }
        public class DeleteCampaignCommandHandler : DeleteCommandHandler<Campaign, DeleteCampaignCommand>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public DeleteCampaignCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<int>> Handle(DeleteCampaignCommand request, CancellationToken cancellationToken)
            {
                var entity = await Repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                    throw new ValidationException(error);
                }

                if (entity.BrandId != _authenticatedUserService.BrandId)
                {
                    var error = new ValidationError("id", $"Không có quyền xóa campain này.");
                    throw new ValidationException(error);
                }

                Repository.Delete(entity);
                await UnitOfWork.CommitAsync();
                return new Response<int>(entity.Id);
            }
        }
    }
}
