using AutoMapper;
using IMP.Application.Models;
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

    public interface IGetByIdQuery<TEntity, TViewModel> : IRequest<Response<TViewModel>>
        where TEntity : BaseEntity, new()
        where TViewModel : BaseViewModel<int>
    {
        int Id { get; set; }
    }


    public abstract class DeleteCommandHandler<TEntity, TCommand> : IRequestHandler<TCommand, Response<int>>
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


    public abstract class GetByIdQueryHandle<TRequest, TEntity, TViewModel> : IRequestHandler<TRequest, Response<TViewModel>>
        where TViewModel : BaseViewModel<int>, new()
        where TEntity : BaseEntity, new()
        where TRequest : IGetByIdQuery<TEntity, TViewModel>
    {
        private readonly IGenericRepositoryAsync<int, TEntity> _repositoryAsync;
        private readonly IMapper _mapper;
        public GetByIdQueryHandle(IGenericRepositoryAsync<int, TEntity> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public virtual async Task<Response<TViewModel>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var entity = await _repositoryAsync.GetByIdAsync(request.Id);
            if (entity == null)
            {
                //var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                throw new KeyNotFoundException();
            }

            var data = _mapper.Map<TViewModel>(entity);
            return new Response<TViewModel>(data);
        }
    }
}
