using AutoMapper;
using IMP.Application.Interfaces;
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

namespace IMP.Application.Features.ApplicationUsers.Queries.GetInfluencerById
{
    public class GetInfluencerByIdCommand : ICommand<InfluencerViewModel>
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
    }

    public class GetInfluencerByIdCommandHandler : CommandHandler<GetInfluencerByIdCommand, InfluencerViewModel>
    {
        public GetInfluencerByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public override async Task<Response<InfluencerViewModel>> Handle(GetInfluencerByIdCommand request, CancellationToken cancellationToken)
        {

            var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x =>
                (request.Nickname == null && x.Id == request.Id)
                || (request.Nickname != null && x.Nickname.ToLower().Equals(request.Nickname.ToLower())),
                include: x => x.Include(y => y.PaymentInfor).ThenInclude(y => y.Bank)
                    .Include(y => y.Ranking).ThenInclude(y => y.RankLevel)
                    .Include(y => y.InfluencerPlatforms).ThenInclude(y => y.Platform)
                    .Include(y => y.Pages)
            );
            if (user == null)
            {
                //var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                throw new KeyNotFoundException();
            }

            var view = Mapper.Map<InfluencerViewModel>(user);
            view.BioLinks = user.Pages.Select(x => x.BioLink).ToList();
            return new Response<InfluencerViewModel>(view);
        }
    }
}
