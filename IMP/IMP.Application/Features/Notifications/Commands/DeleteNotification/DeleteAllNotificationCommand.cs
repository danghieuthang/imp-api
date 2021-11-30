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

namespace IMP.Application.Features.Notifications.Commands.DeleteNotification
{
    public class DeleteAllNotificationCommand : ICommand<bool>
    {
        public class DeleteAllNotificationCommandHandler : CommandHandler<DeleteAllNotificationCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public DeleteAllNotificationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<bool>> Handle(DeleteAllNotificationCommand request, CancellationToken cancellationToken)
            {
                var repository = UnitOfWork.Repository<Notification>();
                var notifications = await repository.GetAll(predicate: x => x.ApplicationUserId == _authenticatedUserService.ApplicationUserId).ToListAsync();
                foreach (var notification in notifications)
                {
                    repository.DeleteCompletely(notification);
                }
                await UnitOfWork.CommitAsync();
                return new Response<bool>(true);
            }
        }
    }
}
