using AutoMapper;
using IMP.Application.Models.ViewModels;
using IMP.Application.Features.CampaignTypes.Queries.GetAllCampaignTypes;
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

namespace IMP.Application.Features.CampaignTypes.Queries
{
    public class GetAllCampaignTypeQuery : IGetAllQuery<CampaignTypeViewModel>
    {
        public class GetAllCampaignTypeQueryHandler : GetAllQueryHandler<GetAllCampaignTypeQuery, CampaignType, CampaignTypeViewModel>
        {
            public GetAllCampaignTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }
        }
    }
}
