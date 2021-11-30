using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Notifications.Queries
{
    public class GetNumberOfUnreadNotificationQuery : IQuery<CountUnreadNotification>
    {
        public class GetNumBerOfUnreadNotificationQueryHandler : QueryHandler<GetNumberOfUnreadNotificationQuery, CountUnreadNotification>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetNumBerOfUnreadNotificationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<CountUnreadNotification>> Handle(GetNumberOfUnreadNotificationQuery request, CancellationToken cancellationToken)
            {
                int number = await UnitOfWork.Repository<Notification>().CountAsync(x => x.ApplicationUserId == _authenticatedUserService.ApplicationUserId && x.IsRead == false);
                return new Response<CountUnreadNotification>(new CountUnreadNotification { Number = number });
            }
        }
    }
}
