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
            private readonly IGenericRepositoryAsync<int, Location> _locationRepositoryAsync;
            private readonly IMapper _mapper;

            public GetAllLocationQueryHandler(IGenericRepositoryAsync<int, Location> locationRepositoryAsync, IMapper mapper)
            {
                _locationRepositoryAsync = locationRepositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<IEnumerable<LocationViewModel>>> Handle(GetAllLocationQuery request, CancellationToken cancellationToken)
            {
                var locations = await _locationRepositoryAsync.GetAllAsync();
                var locationViews = _mapper.Map<IEnumerable<LocationViewModel>>(locations);
                return new Response<IEnumerable<LocationViewModel>>(locationViews);
            }
        }
    }
}
