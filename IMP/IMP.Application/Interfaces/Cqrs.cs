using System.Windows.Input;
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
    /// <summary>
    /// Provides the interface(s) for Command
    /// </summary>
    /// <typeparam name="T">The type of response data</typeparam>
    public interface ICommand<T> : IRequest<Response<T>>
        where T : notnull
    {

    }
    /// <summary>
    /// Provides the interface(s) for Query
    /// </summary>
    /// <typeparam name="T">The type of response</typeparam>
    public interface IQuery<T> : IRequest<Response<T>>
        where T : notnull
    {

    }
    /// <summary>
    /// Provides the interface(s) for Delete Command
    /// </summary>
    /// <typeparam name="TId">The type Id of entity</typeparam>
    /// <typeparam name="TResponse">The type of response</typeparam>
    public interface IDeleteCommand<TId, TResponse> : ICommand<TResponse>
       where TId : struct
       where TResponse : notnull
    {
        TId Id { get; set; }
    }

    /// <summary>
    /// Provides the interface(s) for Query List
    /// </summary>
    /// <typeparam name="T">The type of item response</typeparam>
    public interface IListQuery<T> : IQuery<IPagedList<T>>
        where T : notnull
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        string OrderField { get; set; }
        OrderBy OrderBy { get; set; }
    }
    /// <summary>
    /// Provides interface(s) for Query Item
    /// </summary>
    /// <typeparam name="TId">The type id of item</typeparam>
    /// <typeparam name="TResponse">The type of response</typeparam>
    public interface IItemQuery<TId, TResponse> : IQuery<TResponse>
      where TId : struct
      where TResponse : notnull
    {
        public TId Id { get; set; }
    }

    #endregion

    #region generic abstract class

    /// <summary>
    /// Provider abstract class for Command Handler
    /// </summary>
    /// <typeparam name="TCommand">The type of command</typeparam>
    /// <typeparam name="TResponse">The type of response</typeparam>
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

    /// <summary>
    /// Provides abstract class for Query Handler
    /// </summary>
    /// <typeparam name="TQuery">The type of query</typeparam>
    /// <typeparam name="TResponse">The type of response</typeparam>
    public abstract class QueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Response<TResponse>>
        where TQuery : IQuery<TResponse>, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public abstract Task<Response<TResponse>> Handle(TQuery request, CancellationToken cancellationToken);
        public IUnitOfWork UnitOfWork => _unitOfWork;
        public IMapper Mapper => _mapper;

    }
    /// <summary>
    /// Provides abstract class for Query List Handler
    /// </summary>
    /// <typeparam name="TQuery">The type of list query</typeparam>
    /// <typeparam name="TViewModel">The type of response</typeparam>
    public abstract class ListQueryHandler<TQuery, TViewModel> : IRequestHandler<TQuery, Response<IPagedList<TViewModel>>>
       where TQuery : IListQuery<TViewModel>, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public abstract Task<Response<IPagedList<TViewModel>>> Handle(TQuery request, CancellationToken cancellationToken);
        public IUnitOfWork UnitOfWork => _unitOfWork;
        public IMapper Mapper => _mapper;
    }
    #endregion

    #region generic delete
    /// <summary>
    /// Providers interface(s) for Delete Command
    /// </summary>
    /// <typeparam name="TEntity">The type of entity</typeparam>
    public interface IDeleteCommand<TEntity> : IDeleteCommand<int, int> where TEntity : BaseEntity, new()
    {
    }
    /// <summary>
    /// Provides abstract class for Delete Command Handler
    /// </summary>
    /// <typeparam name="TEntity">The type of entity delete</typeparam>
    /// <typeparam name="TCommand">The type of Delete Command</typeparam>
    public abstract class DeleteCommandHandler<TEntity, TCommand> : IRequestHandler<TCommand, Response<int>>
       where TEntity : BaseEntity, new()
       where TCommand : class, IDeleteCommand<TEntity>, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _repository;
        public DeleteCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            _repository = unitOfWork.Repository<TEntity>();
        }

        public virtual async Task<Response<int>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                throw new ValidationException(error);
            }

            _repository.Delete(entity);
            await _unitOfWork.CommitAsync();
            return new Response<int>(entity.Id);
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;
        public IGenericRepository<TEntity> Repository => _repository;

    }
    #endregion

    #region generic get by id
    /// <summary>
    /// Provides inteface(s) for Get Item By Id Query
    /// </summary>
    /// <typeparam name="TEntity">The type of Entity </typeparam>
    /// <typeparam name="TViewModel">The type View Model of Entity</typeparam>
    public interface IGetByIdQuery<TEntity, TViewModel> : IItemQuery<int, TViewModel>
       where TEntity : BaseEntity, new()
       where TViewModel : BaseViewModel<int>
    {
    }
    /// <summary>
    /// Provides abstract class for Get Item By Id Query Handler
    /// </summary>
    /// <typeparam name="TRequest">The type of Get By Id Query</typeparam>
    /// <typeparam name="TEntity">The type of Entity</typeparam>
    /// <typeparam name="TViewModel">The type View Model of Entity</typeparam>
    public abstract class GetByIdQueryHandler<TRequest, TEntity, TViewModel> : IRequestHandler<TRequest, Response<TViewModel>>
       where TViewModel : BaseViewModel<int>, new()
       where TEntity : BaseEntity, new()
       where TRequest : IGetByIdQuery<TEntity, TViewModel>
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.Repository<TEntity>();
            _mapper = mapper;
        }

        public virtual async Task<Response<TViewModel>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                //var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                throw new KeyNotFoundException();
            }

            var data = _mapper.Map<TViewModel>(entity);
            return new Response<TViewModel>(data);
        }

        public IGenericRepository<TEntity> Repository => _repository;
        public IMapper Mapper => _mapper;
        public IUnitOfWork UnitOfWork => _unitOfWork;


    }
    #endregion

    #region generic get all
    /// <summary>
    /// Provides interface(s) for Get All Query
    /// </summary>
    /// <typeparam name="TViewModel">The type View Model of entity</typeparam>
    public interface IGetAllQuery<TViewModel> : IQuery<IEnumerable<TViewModel>>
      where TViewModel : BaseViewModel<int>
    {
    }
    /// <summary>
    /// Provides abstract class for Get All Query Handler
    /// </summary>
    /// <typeparam name="TRequest">The type of GetAllQuery</typeparam>
    /// <typeparam name="TEntity">The type of entity</typeparam>
    /// <typeparam name="TViewModel">The type View Model of Entity</typeparam>
    public abstract class GetAllQueryHandler<TRequest, TEntity, TViewModel> : IRequestHandler<TRequest, Response<IEnumerable<TViewModel>>>
      where TRequest : class, IGetAllQuery<TViewModel>, new()
      where TViewModel : BaseViewModel<int>, new()
      where TEntity : BaseEntity, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<Response<IEnumerable<TViewModel>>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var entities = await Repository.GetAllAsync();
            var data = _mapper.Map<IEnumerable<TViewModel>>(entities);
            return new Response<IEnumerable<TViewModel>>(data);
        }
        public IGenericRepository<TEntity> Repository => _unitOfWork.Repository<TEntity>();
        public ICachedRepository<TEntity> CacheRepository => _unitOfWork.CacheRepository<TEntity>();
        public IMapper Mapper => _mapper;
    }
    #endregion
}
