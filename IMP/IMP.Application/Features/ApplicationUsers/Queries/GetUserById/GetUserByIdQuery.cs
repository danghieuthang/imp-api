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

namespace IMP.Application.Features.ApplicationUsers.Queries.GetUserById
{
    public class GetUserByIdQuery : IGetByIdQuery<ApplicationUser, ApplicationUserViewModel>
    {
        public int Id { get; set; }
        public class GetUserByIdQueryHandler : GetByIdQueryHandler<GetUserByIdQuery, ApplicationUser, ApplicationUserViewModel>
        {
            public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<ApplicationUserViewModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _repositoryAsync.FindSingleAsync(x => x.Id == request.Id, x => x.PaymentInfor, x => x.PaymentInfor.Bank);
                if (entity == null)
                {
                    //var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                    throw new KeyNotFoundException();
                }

                var view = _mapper.Map<ApplicationUserViewModel>(entity);
                view.InterestsR = entity.Interests?.Split(";").ToList();
                view.JobsR = entity.Job?.Split(";").ToList();
                view.PetsR = entity.Pet?.Split(";").ToList();

                return new Response<ApplicationUserViewModel>(view);
            }
        }
    }
}
