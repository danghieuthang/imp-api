using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace IMP.Application.Features.Pages.Commands.UpdatePage
{
    public class UpdatePageCommand : ICommand<PageViewModel>
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public string Title { get; set; }
        public string BackgroundPhoto { get; set; }
        public int PositionPage { get; set; }
        public class UpdatePageCommandHandler : IRequestHandler<UpdatePageCommand, Response<PageViewModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepositoryAsync<Page> _pageRepositoryAsync;
            private readonly IMapper _mapper;

            public UpdatePageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _pageRepositoryAsync = _unitOfWork.Repository<Page>();
                _mapper = mapper;
            }

            public async Task<Response<PageViewModel>> Handle(UpdatePageCommand request, CancellationToken cancellationToken)
            {

                var entity = await _pageRepositoryAsync.GetByIdAsync(request.Id);
                if (entity != null)
                {
                    _mapper.Map(request, entity);
                    _pageRepositoryAsync.Update(entity);
                    await _unitOfWork.CommitAsync();
                }

                var view = _mapper.Map<PageViewModel>(entity);
                return new Response<PageViewModel>(view);
            }
        }
    }
}