using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.InfluencerPlatforms.Commands.DeleteInfluencerPlatformById
{
    public class DeleteInfluencerPlatformCommand : IDeleteCommand<InfluencerPlatform>
    {
        public int Id { get; set; }
        public int InfluencerId { get; set; }

        public class DeleteInfluencerPlatformCommandHandler : DeleteCommandHandler<InfluencerPlatform, DeleteInfluencerPlatformCommand>
        {
            public DeleteInfluencerPlatformCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }


            public override async Task<Response<int>> Handle(DeleteInfluencerPlatformCommand request, CancellationToken cancellationToken)
            {
                var influencerPlatform = await RepositoryAsync.GetByIdAsync(request.Id);
                if (influencerPlatform == null)
                {
                    var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                    throw new ValidationException(error);
                }

                if (influencerPlatform.InfluencerId != request.InfluencerId)
                {
                    var error = new ValidationError("id", $"Không có quyền xóa.");
                    throw new ValidationException(error);
                }

                RepositoryAsync.Delete(influencerPlatform);
                await UnitOfWork.CommitAsync();
                return new Response<int>(influencerPlatform.Id);
            }

        }
    }
}
