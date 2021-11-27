using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Notifications.Commands.MakeNotificationRead
{
    public class MakeNotificationIsReadCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public class MakeNotificationIsReadCommandHandler : CommandHandler<MakeNotificationIsReadCommand, bool>
        {
            public MakeNotificationIsReadCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<bool>> Handle(MakeNotificationIsReadCommand request, CancellationToken cancellationToken)
            {
                var notification = await UnitOfWork.Repository<Notification>().GetByIdAsync(request.Id);
                if (notification == null)
                {
                    return new Response<bool>(false);
                }

                notification.IsRead = true;
                UnitOfWork.Repository<Notification>().Update(notification);
                await UnitOfWork.CommitAsync();
                return new Response<bool>(true);
            }
        }
    }
}
