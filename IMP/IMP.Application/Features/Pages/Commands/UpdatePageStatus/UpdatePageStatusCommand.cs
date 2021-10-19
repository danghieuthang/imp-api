using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Pages.Commands.UpdatePageStatus
{
    public class UpdatePageStatusCommand : ICommand<PageViewModel>
    {
        public int Id { get; set; }
        public PageStatus Status { get; set; }

        public class UpdatePageStatusCommandHandler : CommandHandler<UpdatePageStatusCommand, PageViewModel>
        {
            private readonly IGenericRepository<Page> _pageRepository;

            public UpdatePageStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _pageRepository = unitOfWork.Repository<Page>();
            }

            public override async Task<Response<PageViewModel>> Handle(UpdatePageStatusCommand request, CancellationToken cancellationToken)
            {
                var pageDomain = await _pageRepository.FindSingleAsync(predicate: x => x.Id == request.Id);

                if (pageDomain != null)
                {
                    pageDomain.Status = (int)request.Status;

                    _pageRepository.Update(pageDomain);
                    await UnitOfWork.CommitAsync();

                    var view = Mapper.Map<PageViewModel>(pageDomain);
                    return new Response<PageViewModel>(view);
                }
                return new Response<PageViewModel>(error: new Models.ValidationError("id", "KHông tồn tại."));
            }
        }

    }
}
