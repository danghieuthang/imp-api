using AutoMapper;
using IMP.Application.Models.ViewModels;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IMP.Application.Interfaces;

namespace IMP.Application.Features.Platforms.Queries
{
    public class GetPlatformByIdQuery : IGetByIdQuery<Platform, PlatformViewModel>
    {
        public int Id { get; set; }
        public class GetPlatformByIDQueryHandler : GetByIdQueryHandler<GetPlatformByIdQuery, Platform, PlatformViewModel>
        {
            public GetPlatformByIDQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }
        }
    }

}