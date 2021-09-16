using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMP.Application.Features.Pages.Commands.CreatePage
{
    public class CreatePageCommand : ICommand<PageViewModel>
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public string Title { get; set; }
        public string BackgroundPhoto { get; set; }
        public int PositionPage { get; set; }

        public class CreatePageCommandHandler : IRequestHandler<CreatePageCommand, Response<PageViewModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepositoryAsync<Page> _pageRepositoryAsync;
            private readonly IMapper _mapper;

            public CreatePageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _pageRepositoryAsync = _unitOfWork.Repository<Page>();
                _mapper = mapper;
            }

            public async Task<Response<PageViewModel>> Handle(CreatePageCommand request, CancellationToken cancellationToken)
            {

                var entity = _mapper.Map<Page>(request);
                entity = await _pageRepositoryAsync.AddAsync(entity);
                await _unitOfWork.CommitAsync();
                var view = _mapper.Map<PageViewModel>(entity);
                return new Response<PageViewModel>(view);
            }
        }
    }
}