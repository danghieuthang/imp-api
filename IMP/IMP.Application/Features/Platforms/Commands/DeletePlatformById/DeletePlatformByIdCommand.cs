using IMP.Application.DTOs;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Platforms.Commands.DeletePlatformById
{
    public class DeletePlatformByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public class DeletePlatformByIdCommandHandler : IRequestHandler<DeletePlatformByIdCommand, Response<int>>
        {
            private readonly IPlatformRepositoryAsync _platformRepositoryAsync;
            public DeletePlatformByIdCommandHandler(IPlatformRepositoryAsync platformRepositoryAsync)
            {
                _platformRepositoryAsync = platformRepositoryAsync;
            }


            public async Task<Response<int>> Handle(DeletePlatformByIdCommand request, CancellationToken cancellationToken)
            {
                var entity = await _platformRepositoryAsync.GetByIdAsync(request.Id);
                if (entity is null)
                {
                    var error = new ValidationError("id", $"'{request.Id}' không tồn tại.");
                    throw new ValidationException(error);
                }
                await _platformRepositoryAsync.DeleteAsync(entity);
                return new Response<int>(entity.Id);
            }
        }
    }
}
