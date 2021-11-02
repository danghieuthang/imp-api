using AutoMapper;
using FluentValidation;
using IMP.Application.Extensions;
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

namespace IMP.Application.Features.ActivityTypes.Commands.CreateActivityType
{
    public class CreateActivityTypeCommand : ICommand<ActivityTypeViewModel>
    {
        public string Name { get; set; }
        public class CreateActivityTypeCommandValidator : AbstractValidator<CreateActivityTypeCommand>
        {
            public CreateActivityTypeCommandValidator()
            {
                RuleFor(x => x.Name).MustRequired(256);
            }
        }
        public class CreateActivityTypeCommandHandler : CommandHandler<CreateActivityTypeCommand, ActivityTypeViewModel>
        {
            public CreateActivityTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<ActivityTypeViewModel>> Handle(CreateActivityTypeCommand request, CancellationToken cancellationToken)
            {
                var activity = Mapper.Map<ActivityType>(request);

                await UnitOfWork.Repository<ActivityType>().AddAsync(activity);
                await UnitOfWork.CommitAsync();

                var activityView = Mapper.Map<ActivityTypeViewModel>(activity);
                return new Response<ActivityTypeViewModel>(activityView);
            }
        }
    }
}
