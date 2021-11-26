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
    public class GetNotificationByIdQuery : IGetByIdQuery<Notification, NotificationViewModel>
    {
        public int Id { get; set; }
        public class NotificationByIdQueryHandler : GetByIdQueryHandler<GetNotificationByIdQuery, Notification, NotificationViewModel>
        {
            public NotificationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }
        }
    }
}
