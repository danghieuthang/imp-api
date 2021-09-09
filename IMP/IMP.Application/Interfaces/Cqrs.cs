using IMP.Application.DTOs;
using IMP.Application.Exceptions;
using IMP.Application.Wrappers;
using IMP.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces
{
    public interface IDeleteCommand<TEntity> : IRequest<Response<int>> where TEntity : BaseEntity, new()
    {
        int Id { get; set; }
    }

    public class DeleteCommandHandler<TEntity, TCommand> : IRequestHandler<TCommand, Response<int>>
        where TEntity : BaseEntity, new()
        where TCommand : class, IDeleteCommand<TEntity>, new()
    {
        private readonly IGenericRepositoryAsync<int, TEntity> _repositoryAsync;
        public DeleteCommandHandler(IGenericRepositoryAsync<int, TEntity> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
        }

        public async Task<Response<int>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repositoryAsync.GetByIdAsync(request.Id);
            if (entity == null)
            {
                var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                throw new ValidationException(error);
            }

            await _repositoryAsync.DeleteAsync(entity);
            return new Response<int>(entity.Id);
        }
    }
    
}
