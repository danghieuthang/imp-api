using AutoMapper;
using IMP.Application.Models.Compaign;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Campaigns.Queries.GetCampaignById
{
    public class GetCampaignByIdQuery : IGetByIdQuery<Campaign, CampaignViewModel>
    {
        public int Id { get; set; }
        public class GetCampaignByIdQueryHandler : GetByIdQueryHandler<GetCampaignByIdQuery, Campaign, CampaignViewModel>
        {
            public GetCampaignByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }
        }
    }


}
