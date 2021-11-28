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

namespace IMP.Application.Features.VoucherCodes.Commands.AssignVoucherCodeForCampaignMember
{
    public class AssignVoucherCodeForCampaignMemberCommand : ICommand<bool>
    {
        public int CampaignMemberId { get; set; }
        public int VoucherCodeId { get; set; }

        public class AssignVoucherCodeForCampaignMemberCommandHandler : CommandHandler<AssignVoucherCodeForCampaignMemberCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public AssignVoucherCodeForCampaignMemberCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<bool>> Handle(AssignVoucherCodeForCampaignMemberCommand request, CancellationToken cancellationToken)
            {
                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.Id == request.CampaignMemberId, x => x.Campaign, x => x.Campaign.Vouchers);
                if (campaignMember == null)
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Không tồn tại."));
                }
                if (campaignMember.Campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Không có quyền thêm voucher code trong chiến dịch này."));
                };
                var vouchers = campaignMember.Campaign.Vouchers.Where(x => x.IsDefaultReward == false && x.IsBestInfluencerReward == false).Select(x => x.VoucherId).ToList();

                var voucherCode = await UnitOfWork.Repository<VoucherCode>().GetByIdAsync(request.VoucherCodeId);

                if (voucherCode == null)
                {
                    throw new ValidationException(new ValidationError("voucher_code_id", "Không tồn tại."));
                }

                if (!vouchers.Contains(voucherCode.VoucherId))
                {
                    throw new ValidationException(new ValidationError("voucher_code_id", "Voucher code này không nằm trong các voucher được cài đặt cho chiến dịch."));
                }

                if (voucherCode.CampaignMemberId != null)
                {
                    throw new ValidationException(new ValidationError("voucher_code_id", "Voucher code này đã được giao cho thành viên khác của chiến dịch."));

                }
                voucherCode.CampaignMemberId = request.CampaignMemberId;
                UnitOfWork.Repository<VoucherCode>().Update(voucherCode);
                await UnitOfWork.CommitAsync();
                return new Response<bool>(data: true);
            }
        }
    }
}
