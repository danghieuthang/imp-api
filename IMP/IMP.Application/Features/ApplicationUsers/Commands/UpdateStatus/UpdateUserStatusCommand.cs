using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.ApplicationUsers.Commands.UpdateStatus
{
    public class UpdateUserStatusCommand : ICommand<ApplicationUserViewModel>
    {
        public int Id { get; set; }
        public UserStatus Status { get; set; }
        public class UpdateUserStatusCommandHandler : CommandHandler<UpdateUserStatusCommand, ApplicationUserViewModel>
        {
            public UpdateUserStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<ApplicationUserViewModel>> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.Repository<ApplicationUser>().GetByIdAsync(request.Id);
                if (user == null)
                {
                    throw new ValidationException(new ValidationError("id", "Người dùng không tồn tại."));
                }

                if (user.Status == (int)request.Status)
                {
                    throw new ValidationException(new ValidationError("id", "Trạng thái này đã được cập nhật cho người dùng."));
                }

                user.Status = (int)request.Status;

                UnitOfWork.Repository<ApplicationUser>().Update(user);
                await UnitOfWork.CommitAsync();

                var userView = Mapper.Map<ApplicationUserViewModel>(user);
                return new Response<ApplicationUserViewModel>(userView);
            }
        }
    }
}
