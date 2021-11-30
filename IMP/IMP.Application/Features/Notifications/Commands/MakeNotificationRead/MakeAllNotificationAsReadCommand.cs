using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Notifications.Commands.MakeNotificationRead
{
    public class MakeAllNotificationAsReadCommand : ICommand<bool>
    {
        public class MakeAllNotificationAsReadCommandHandler : CommandHandler<MakeAllNotificationAsReadCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public MakeAllNotificationAsReadCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<bool>> Handle(MakeAllNotificationAsReadCommand request, CancellationToken cancellationToken)
            {
                var repository = UnitOfWork.Repository<Notification>();
                var notifications = await repository.GetAll(predicate: x => x.ApplicationUserId == _authenticatedUserService.ApplicationUserId && x.IsRead == false).ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                    repository.Update(notification);
                }

                await UnitOfWork.CommitAsync();
                return new Response<bool>(data: true);
            }
        }
    }
}
