using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Vouchers.Commands.RemoveVoucherFromCampaign
{
    public class RemoveVoucherFromCampaignCommand : ICommand<bool>
    {
        public int VoucherId { get; set; }
        public int CampaignId { get; set; }

        public class RemoveVoucherFromCampaignCommandHandler : CommandHandler<RemoveVoucherFromCampaignCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public RemoveVoucherFromCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<bool>> Handle(RemoveVoucherFromCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaignVoucher = await UnitOfWork.Repository<CampaignVoucher>().FindSingleAsync(x => x.CampaignId == request.CampaignId && x.VoucherId == request.VoucherId && x.IsDefaultReward == false && x.IsBestInfluencerReward == false);
                if (campaignVoucher != null)
                {
                    UnitOfWork.Repository<CampaignVoucher>().DeleteCompletely(campaignVoucher);
                    await UnitOfWork.CommitAsync();
                    return new Response<bool>(true);
                }
                throw new ValidationException(new ValidationError("", "Không tồn tại."));
            }
        }
    }
}
