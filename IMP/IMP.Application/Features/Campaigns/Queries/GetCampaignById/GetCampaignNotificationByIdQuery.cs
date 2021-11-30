using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Queries.GetCampaignById
{
    public class GetCampaignNotificationByIdQuery : IGetByIdQuery<Campaign, CampaignNotificationViewModel>
    {
        public int Id { get; set; }
        public class GetCampaignNotificationByIdQueryHandler : GetByIdQueryHandler<GetCampaignNotificationByIdQuery, Campaign, CampaignNotificationViewModel>
        {
            public GetCampaignNotificationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<CampaignNotificationViewModel>> Handle(GetCampaignNotificationByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await Repository.FindSingleAsync(x => x.Id == request.Id,
                    include: c => c.Include(x => x.CampaignImages)
                            );

                if (entity == null)
                {
                    //var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                    throw new KeyNotFoundException();
                }


                return new Response<CampaignNotificationViewModel>(new CampaignNotificationViewModel { Id = entity.Id, Description = entity.Description, Title = entity.Title, Image = entity.CampaignImages.FirstOrDefault()?.Url });
            }
        }
    }
}
