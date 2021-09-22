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

namespace IMP.Application.Features.Locations.Queries.GetAllLocations
{
    public class GetAllLocationQuery : IRequest<Response<IEnumerable<LocationViewModel>>>
    {
        public class GetAllLocationQueryHandler : IRequestHandler<GetAllLocationQuery, Response<IEnumerable<LocationViewModel>>>
        {
            private readonly IGenericRepository<Location> _locationRepository;
            private readonly IMapper _mapper;

            public GetAllLocationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _locationRepository = unitOfWork.Repository<Location>();
                _mapper = mapper;
            }

            public async Task<Response<IEnumerable<LocationViewModel>>> Handle(GetAllLocationQuery request, CancellationToken cancellationToken)
            {
                var locations = await _locationRepository.GetAllAsync();
                var locationViews = _mapper.Map<IEnumerable<LocationViewModel>>(locations);
                return new Response<IEnumerable<LocationViewModel>>(locationViews);
            }
        }
    }
}
