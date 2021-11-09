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

namespace IMP.Application.Features.ApplicationUsers.Queries.GetInfluencerById
{
    public class GetInfluencerByIdCommand : ICommand<InfluencerViewModel>
    {
        public int Id { get; set; }
    }

    public class GetInfluencerByIdCommandHandler : CommandHandler<GetInfluencerByIdCommand, InfluencerViewModel>
    {
        public GetInfluencerByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public override async Task<Response<InfluencerViewModel>> Handle(GetInfluencerByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Id == request.Id,
                    x => x.PaymentInfor,
                    x => x.PaymentInfor.Bank,
                    x => x.Ranking,
                    x => x.Ranking.RankLevel,
                    x => x.Location,
                    x => x.InfluencerPlatform);
            if (user == null)
            {
                //var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                throw new KeyNotFoundException();
            }

            var view = Mapper.Map<InfluencerViewModel>(user);
            view.InterestsR = user.Interests?.Split(";").ToList();
            view.JobsR = user.Job?.Split(";").ToList();
            view.PetsR = user.Pet?.Split(";").ToList();
            return new Response<InfluencerViewModel>(view);
        }
    }
}
