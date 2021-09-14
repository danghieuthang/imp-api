using AutoMapper;
using IMP.Application.Models.ViewModels;
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

namespace IMP.Application.Features.CampaignTypes.Queries.GetCampaignTypeById
{
    public class GetCampaignTypeByIdQuery : IGetByIdQuery<CampaignType, CampaignTypeViewModel>
    {
        public int Id { get; set; }
        public class GetCampaignTypeByIdQueryHandler : GetByIdQueryHandler<GetCampaignTypeByIdQuery, CampaignType, CampaignTypeViewModel>
        {
            public GetCampaignTypeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }
        }
    }
}
