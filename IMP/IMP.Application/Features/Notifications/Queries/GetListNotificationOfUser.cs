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
    public class GetListNotificationOfUserQuery : PageRequest, IListQuery<NotificationViewModel>
    {
        [FromForm(Name = "is_read")]
        public bool? IsRead { get; set; }
        internal class GetListNotificationOfUserQueryHandler : ListQueryHandler<GetListNotificationOfUserQuery, NotificationViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetListNotificationOfUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<IPagedList<NotificationViewModel>>> Handle(GetListNotificationOfUserQuery request, CancellationToken cancellationToken)
            {
                var notifications = await UnitOfWork.Repository<Notification>().GetPagedList(
                        predicate: x => x.ApplicationUserId == _authenticatedUserService.ApplicationUserId
                            && (request.IsRead == null || (request.IsRead.HasValue && x.IsRead == request.IsRead.Value)),
                        orderBy: request.OrderField,
                        orderByDecensing: request.OrderBy == Enums.OrderBy.DESC
                        );

                var view = notifications.ToResponsePagedList<NotificationViewModel>(Mapper);
                return new Response<IPagedList<NotificationViewModel>>(view);
            }
        }
    }

}
