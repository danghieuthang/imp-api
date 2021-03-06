using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Locations.Queries.GetLocationByCode
{
    public class GetLocationByCodeQuery : IRequest<Response<LocationViewModel>>
    {
        public string Code { get; set; }
        public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByCodeQuery, Response<LocationViewModel>>
        {
            private readonly IGenericRepository<Location> _locationRepository;
            private readonly IMapper _mapper;

            public GetLocationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _locationRepository = unitOfWork.Repository<Location>();
                _mapper = mapper;
            }

            public async Task<Response<LocationViewModel>> Handle(GetLocationByCodeQuery request, CancellationToken cancellationToken)
            {
                var location = await _locationRepository.FindSingleAsync(x => x.Code == request.Code);
                if (location == null)
                {
                    throw new KeyNotFoundException($"{request.Code} không tồn tại.");
                }
                var locationView = _mapper.Map<LocationViewModel>(location);
                return new Response<LocationViewModel>(locationView);
            }
        }
    }
}
