using AutoMapper;
using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.ActivityComments.Commands.CreateActivityComment
{
    public class CreateActivityCommentCommand : ICommand<ActivityCommentViewModel>
    {
        public int MemberActivityId { get; set; }
        public string Comment { get; set; }

        public class CreateActivityCommentCommandValidator : AbstractValidator<CreateActivityCommentCommand>
        {
            public CreateActivityCommentCommandValidator(IUnitOfWork unitOfWork)
            {
                RuleFor(x => x.Comment).MustMaxLength(256);
                RuleFor(x => x.MemberActivityId).MustExistEntityId(async (id, c) =>
                {
                    return await unitOfWork.Repository<MemberActivity>().IsExistAsync(id);
                });
            }
        }

        public class CreateActivityCommentCommandHandler : CommandHandler<CreateActivityCommentCommand, ActivityCommentViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public CreateActivityCommentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<ActivityCommentViewModel>> Handle(CreateActivityCommentCommand request, CancellationToken cancellationToken)
            {
                var memberACtivity = await UnitOfWork.Repository<MemberActivity>().GetByIdAsync(request.MemberActivityId);

                if (memberACtivity == null) throw new IMP.Application.Exceptions.ValidationException(new ValidationError("", "Không tồn tại."));

                var activityComment = new ActivityComment
                {
                    MemberActivityId = memberACtivity.Id,
                    ApplicationUserId = _authenticatedUserService.ApplicationUserId,
                    Comment = request.Comment
                };

                UnitOfWork.Repository<ActivityComment>().Add(activityComment);
                await UnitOfWork.CommitAsync();


                activityComment = await UnitOfWork.Repository<ActivityComment>().FindSingleAsync(x => x.Id == activityComment.Id,
                    include: x => x.Include(y => y.ApplicationUser));

                var activityCommentView = Mapper.Map<ActivityCommentViewModel>(activityComment);
                return new Response<ActivityCommentViewModel>(activityCommentView);
            }
        }
    }
}
