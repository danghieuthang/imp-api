using AutoMapper;
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

namespace IMP.Application.Features.Vouchers.Commands.AssignVoucherForCampaign
{
    public class AssignVoucherToCampaignCommand : ICommand<CampaignVoucherViewModel>
    {
        public int VoucherId { get; set; }
        public int CampaignId { get; set; }
        public int Quantity { get; set; }
        public class AssignVoucherForCampaignCommandHandler : CommandHandler<AssignVoucherToCampaignCommand, CampaignVoucherViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public AssignVoucherForCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<CampaignVoucherViewModel>> Handle(AssignVoucherToCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().GetByIdAsync(request.CampaignId);
                if (campaign != null && campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Không có quyền thêm voucher cho chiến dịch này."));
                }

                var voucher = await UnitOfWork.Repository<Voucher>().GetByIdAsync(request.VoucherId);
                if (voucher != null && voucher.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("voucher_id", "Không có quyền assign voucher này."));
                }


                var campaignVoucher = await UnitOfWork.Repository<CampaignVoucher>().FindSingleAsync(x => x.CampaignId == request.CampaignId && x.VoucherId == request.VoucherId && x.IsDefaultReward == false && x.IsBestInfluencerReward == false);
                if (campaignVoucher != null)
                {
                    throw new ValidationException(new ValidationError("voucher_id", "Voucher này đã được assign cho chiến dịch"));
                }

                campaignVoucher = Mapper.Map<CampaignVoucher>(request);
                campaignVoucher.IsDefaultReward = false;
                campaignVoucher.IsBestInfluencerReward = false;

                UnitOfWork.Repository<CampaignVoucher>().Add(campaignVoucher);
                await UnitOfWork.CommitAsync();
                var campaignVoucherView = Mapper.Map<CampaignVoucherViewModel>(campaignVoucher);
                return new Response<CampaignVoucherViewModel>(campaignVoucherView);
            }
        }
    }
}
