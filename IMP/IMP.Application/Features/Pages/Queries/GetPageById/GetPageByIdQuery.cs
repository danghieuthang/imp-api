using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Pages.Queries.GetPageById
{
    public class GetPageByIdQuery : IGetByIdQuery<Page, PageViewModel>
    {
        public int Id { get; set; }
        public class GetPageByIdQueryHandler : GetByIdQueryHandler<GetPageByIdQuery, Page, PageViewModel>
        {
            public GetPageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<PageViewModel>> Handle(GetPageByIdQuery request, CancellationToken cancellationToken)
            {
                var page = await Repository.FindSingleAsync(x => x.Id == request.Id,
                    include: x =>
                        x.Include(page => page.Blocks)
                            .ThenInclude(block => block.Items)
                         .Include(y => y.Blocks)
                            .ThenInclude(block => block.ChildBlocks));

                var pageView = Mapper.Map<PageViewModel>(page);
                return new Response<PageViewModel>(pageView);
            }
        }
    }
}
