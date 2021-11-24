using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Notifications.Queries
{
    public class GetListNotificationOfUserQuery : IGetAllQuery<NotificationViewModel>
    {
        [FromQuery(Name = "number")]
        public int Number { get; set; }

        internal class GetListNotificationOfUserQueryHandler : GetAllQueryHandler<GetListNotificationOfUserQuery, Notification, NotificationViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetListNotificationOfUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<IEnumerable<NotificationViewModel>>> Handle(GetListNotificationOfUserQuery request, CancellationToken cancellationToken)
            {
                var notifications = Repository.GetAll(
                 predicate: x => x.ApplicationUserId == _authenticatedUserService.ApplicationUserId,
                 orderBy: x => x.OrderByDescending(y => y.Created)).Take(request.Number).ToList();

                var notificationViews = Mapper.Map<IEnumerable<NotificationViewModel>>(notifications);
                return new Response<IEnumerable<NotificationViewModel>>(notificationViews);
            }
        }
    }

}
