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

namespace IMP.Application.Features.ActivityComments.Commands.DeleteAcitivityComment
{
    public class DeleteActivityCommentByIdCommand : IDeleteCommand<ActivityComment>
    {
        public int Id { get; set; }
        public class DeleteActivityCommentByIdCommandHandler : DeleteCommandHandler<ActivityComment, DeleteActivityCommentByIdCommand>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public DeleteActivityCommentByIdCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<int>> Handle(DeleteActivityCommentByIdCommand request, CancellationToken cancellationToken)
            {
                var entity = await Repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    var error = new ValidationError("id", $"'{request.Id}' không tồn tại.");
                    throw new ValidationException(error);
                }
                if (entity.ApplicationUserId != _authenticatedUserService.ApplicationUserId)
                {
                    var error = new ValidationError("id", $"Không có quyền xóa.");
                    throw new ValidationException(error);
                }
                Repository.Delete(entity);
                await UnitOfWork.CommitAsync();

                return new Response<int>(entity.Id);
            }
        }
    }
}
