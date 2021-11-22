using AutoMapper;
using FluentValidation;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Validations;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.ApplicationUsers.Queries.GetAllUser
{
    public class GetAllUserQuery : PageRequest, IListQuery<ApplicationUserViewModel>
    {

        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "status")]
        public UserStatus? Status { get; set; }
        private bool? IsInfluencer { get; set; }
        public void SetIsInfluencer(bool isInfluencer)
        {
            IsInfluencer = isInfluencer;
        }

        public class GetAllUserQueryValidator : PageRequestValidator<GetAllUserQuery, ApplicationUserViewModel>
        {
            public GetAllUserQueryValidator()
            {
                RuleFor(x => x.Status.Value).IsInEnum().WithMessage("Trạng thái tìm kiếm không tồn tại.").When(x => x.Status.HasValue);
            }
        }

        public class GetAllUserQueryHandler : ListQueryHandler<GetAllUserQuery, ApplicationUserViewModel>
        {
            public GetAllUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IPagedList<ApplicationUserViewModel>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
            {
                string name = request.Name != null ? request.Name.ToLower().Trim() : "";
                var page = await UnitOfWork.Repository<ApplicationUser>().GetPagedList(
                        predicate: x =>
                            (!request.Status.HasValue || (request.Status.HasValue && (int)request.Status.Value == x.Status))
                            && (!request.IsInfluencer.HasValue
                                    || (request.IsInfluencer.HasValue && request.IsInfluencer.Value && !x.BrandId.HasValue)
                                    || (request.IsInfluencer.HasValue && !request.IsInfluencer.Value && x.BrandId.HasValue))
                            && (x.FirstName.ToLower().Contains(name) || x.LastName.ToLower().Contains(name) || x.Nickname.ToLower().Contains(name)),
                        pageIndex: request.PageIndex,
                        pageSize: request.PageSize,
                        orderBy: request.OrderField,
                        orderByDecensing: request.OrderBy == OrderBy.DESC);

                var pageView = page.ToResponsePagedList<ApplicationUserViewModel>(Mapper);
                return new Response<IPagedList<ApplicationUserViewModel>>(pageView);
            }
        }
    }
}
