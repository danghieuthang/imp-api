using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.VoucherCodes.Commands.AssignVoucherCodeForCampaignMember
{
    public class AssignVoucherCodesForCampaignCommand : ICommand<bool>
    {
        public int CampaignId { get; set; }
        public int? NumberVoucherCodePerMember { get; set; }
        public bool NoCheckQuantity { get; set; }

        public class AssignVoucherCodesForCampaignCommandHandler : CommandHandler<AssignVoucherCodesForCampaignCommand, bool>
        {
            private readonly IGenericRepository<VoucherCode> _voucherCodeRepository;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public AssignVoucherCodesForCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
                _voucherCodeRepository = UnitOfWork.Repository<VoucherCode>();
            }

            public override async Task<Response<bool>> Handle(AssignVoucherCodesForCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().GetByIdAsync(request.CampaignId);

                if (campaign == null)
                {
                    throw new ValidationException(new ValidationError("campaign_id", $"Không tồn tại chiến dịch."));
                }

                if (campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("campaign_id", $"Không có quyền thêm voucher code cho chiến dịch này."));
                }

                if (!request.NumberVoucherCodePerMember.HasValue)
                {
                    request.NumberVoucherCodePerMember = 1;
                }

                var vouchers = await UnitOfWork.Repository<CampaignVoucher>().GetAll(
                        predicate: x => x.CampaignId == request.CampaignId,
                        selector: x => x.Voucher,
                        include: x => x.Include(y => y.Voucher).ThenInclude(z => z.VoucherCodes)
                    ).ToListAsync();
                vouchers.ForEach(x =>
                {
                    x.VoucherCodes = x.VoucherCodes.Where(y => y.CampaignMemberId == null).ToList();
                });

                var voucherCodes = vouchers.SelectMany(x => x.VoucherCodes).ToList();

                int totalVoucherCodes = voucherCodes.Count;

                var campaignMembers = await UnitOfWork.Repository<CampaignMember>().GetAll(
                    predicate: x =>
                        x.CampaignId == request.CampaignId // của campaign
                        && x.Status == (int)CampaignMemberStatus.Approved // đã được duyệt
                        && x.VoucherCodes.Count() < request.NumberVoucherCodePerMember, // chưa đủ số lượng voucher code assign
                    include: x => x.Include(y => y.VoucherCodes)

                        ).ToListAsync();

                int totalCampaignMembers = campaignMembers.Count;

                if (!request.NoCheckQuantity)
                {
                    if (totalCampaignMembers * request.NumberVoucherCodePerMember > totalVoucherCodes)
                    {
                        throw new ValidationException(new ValidationError("", $"Tổng voucher code {totalVoucherCodes} không đủ cho {totalCampaignMembers} thành viên(mỗi thành viên {request.NumberVoucherCodePerMember} code)."));
                    }
                }

                int voucherCodeIndex = 0; // Index của voucher code 
                foreach (var member in campaignMembers)
                {
                    int numberVoucherCodeAssign = member.VoucherCodes.Count();
                    for (int i = numberVoucherCodeAssign; i < request.NumberVoucherCodePerMember; i++) // Duyệt số lượng code chưa đủ
                    {
                        voucherCodes[voucherCodeIndex].CampaignMemberId = member.Id;
                        _voucherCodeRepository.Update(voucherCodes[voucherCodeIndex]);
                        voucherCodeIndex++;
                        if (voucherCodes.Count >= voucherCodeIndex)
                        {
                            break;
                        }
                    }
                }

                await UnitOfWork.CommitAsync();
                return new Response<bool>(data: true);
            }
        }
    }
}
