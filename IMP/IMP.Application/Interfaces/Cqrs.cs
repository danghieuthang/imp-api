using System.Reflection.Metadata;
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
using Microsoft.AspNetCore.Mvc;
using IMP.Application.Enums;

namespace IMP.Application.Interfaces
{
    #region generic interfaces
    public interface ICommand<T> : IRequest<Response<T>>
        where T : notnull
    {

    }

    public interface IQuery<T> : IRequest<Response<T>>
        where T : notnull
    {

    }

    public interface ICreateCommand<TRequest, TResponse> : ICommand<TResponse>
       where TRequest : notnull
       where TResponse : notnull
    {
        TRequest Model { get; init; }
    }

    public interface IUpdateCommand<TRequest, TResponse> : ICommand<TResponse>
        where TRequest : notnull
        where TResponse : notnull
    {
        public TRequest Model { get; set; }
    }
    public interface IDeleteCommand<TId, TResponse> : ICommand<TResponse>
       where TId : struct
       where TResponse : notnull
    {
        public TId Id { get; set; }
    }

    /// <summary>
    /// Provides the interface(s) for Query List
    /// </summary>
    /// <typeparam name="T">The View Model</typeparam>
    public interface IListQuery<T> : IQuery<IPagedList<T>>
        where T : notnull
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderField { get; set; }
        public OrderBy OrderBy { get; set; }
    }
    public interface IItemQuery<TId, TResponse> : IQuery<TResponse>
      where TId : struct
      where TResponse : notnull
    {
        public TId Id { get; set; }
    }

    #endregion

    #region generic abstract class
    public abstract class CommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Response<TResponse>>
        where TCommand : ICommand<TResponse>, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public abstract Task<Response<TResponse>> Handle(TCommand request, CancellationToken cancellationToken);
        public IUnitOfWork UnitOfWork => _unitOfWork;
        public IMapper Mapper => _mapper;

    }

    public abstract class QueryHandler<TCommand, TResponse> : IRequestHandler<TCommand, Response<TResponse>>
        where TCommand : IQuery<TResponse>, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public abstract Task<Response<TResponse>> Handle(TCommand request, CancellationToken cancellationToken);
        public IUnitOfWork UnitOfWork => _unitOfWork;
        public IMapper Mapper => _mapper;

    }

    public abstract class ListQueryHandler<TCommand, TViewModel> : IRequestHandler<TCommand, Response<IPagedList<TViewModel>>>
       where TCommand : IListQuery<TViewModel>, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public abstract Task<Response<IPagedList<TViewModel>>> Handle(TCommand request, CancellationToken cancellationToken);
        public IUnitOfWork UnitOfWork => _unitOfWork;
        public IMapper Mapper => _mapper;
    }
    #endregion

    #region generic delete
    public interface IDeleteCommand<TEntity> : IDeleteCommand<int, int> where TEntity : BaseEntity, new()
    {
    }

    public abstract class DeleteCommandHandler<TEntity, TCommand> : IRequestHandler<TCommand, Response<int>>
       where TEntity : BaseEntity, new()
       where TCommand : class, IDeleteCommand<TEntity>, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepositoryAsync<TEntity> _repositoryAsync;
        public DeleteCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            _repositoryAsync = unitOfWork.Repository<TEntity>();
        }

        public virtual async Task<Response<int>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repositoryAsync.GetByIdAsync(request.Id);
            if (entity == null)
            {
                var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                throw new ValidationException(error);
            }

            _repositoryAsync.Delete(entity);
            await _unitOfWork.CommitAsync();
            return new Response<int>(entity.Id);
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;
        public IGenericRepositoryAsync<TEntity> RepositoryAsync => _repositoryAsync;

    }
    #endregion

    #region generic get by id
    public interface IGetByIdQuery<TEntity, TViewModel> : IItemQuery<int, TViewModel>
       where TEntity : BaseEntity, new()
       where TViewModel : BaseViewModel<int>
    {
    }
    public abstract class GetByIdQueryHandler<TRequest, TEntity, TViewModel> : IRequestHandler<TRequest, Response<TViewModel>>
       where TViewModel : BaseViewModel<int>, new()
       where TEntity : BaseEntity, new()
       where TRequest : IGetByIdQuery<TEntity, TViewModel>
    {
        private readonly IGenericRepositoryAsync<TEntity> _repositoryAsync;
        private readonly IMapper _mapper;
        public GetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryAsync = unitOfWork.Repository<TEntity>();
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

        public IGenericRepositoryAsync<TEntity> Repository => _repositoryAsync;
        public IMapper Mapper => _mapper;


    }
    #endregion

    #region generic get all
    public interface IGetAllQuery<TViewModel> : IQuery<IEnumerable<TViewModel>>
      where TViewModel : BaseViewModel<int>
    {
    }
    public abstract class GetAllQueryHandler<TRequest, TEntity, TViewModel> : IRequestHandler<TRequest, Response<IEnumerable<TViewModel>>>
      where TRequest : class, IGetAllQuery<TViewModel>, new()
      where TViewModel : BaseViewModel<int>, new()
      where TEntity : BaseEntity, new()
    {
        private readonly IGenericRepositoryAsync<TEntity> _repositoryAsync;
        private readonly IMapper _mapper;
        public GetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryAsync = unitOfWork.Repository<TEntity>();
            _mapper = mapper;
        }

        public virtual async Task<Response<IEnumerable<TViewModel>>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var entities = await _repositoryAsync.GetAllAsync();
            var data = _mapper.Map<IEnumerable<TViewModel>>(entities);
            return new Response<IEnumerable<TViewModel>>(data);
        }
        public IGenericRepositoryAsync<TEntity> Repository => _repositoryAsync;
        public IMapper Mapper => _mapper;
    }
    #endregion
}
