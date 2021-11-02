using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.ActivityTypes.Queries.GetAllActivityTypes
{
    public class GetAllActivityTypesQuery : IGetAllQuery<ActivityTypeViewModel>
    {
        public class GetAllActivityTypesQueryHandler : GetAllQueryHandler<GetAllActivityTypesQuery, ActivityType, ActivityTypeViewModel>
        {
            public GetAllActivityTypesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }
        }
    }
}
