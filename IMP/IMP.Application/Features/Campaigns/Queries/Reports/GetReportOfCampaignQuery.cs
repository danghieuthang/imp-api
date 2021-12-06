using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
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

namespace IMP.Application.Features.Campaigns.Queries.Reports
{
    public class GetReportOfCampaignQuery : IQuery<CampaignReportViewModel>
    {
        public int CampaignId { get; set; }
        public class GetReportOfCampaignQueryHandler : QueryHandler<GetReportOfCampaignQuery, CampaignReportViewModel>
        {
            public GetReportOfCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<CampaignReportViewModel>> Handle(GetReportOfCampaignQuery request, CancellationToken cancellationToken)
            {
                CampaignReportViewModel report = new CampaignReportViewModel();
                var influencers = await UnitOfWork.Repository<CampaignMember>().GetAll(
                        predicate: x => x.CampaignId == request.CampaignId,
                        selector: x => new { x.Id, x.Status }).ToListAsync();

                var vouchers = await UnitOfWork.Repository<CampaignVoucher>().GetAll(
                        predicate: x => x.CampaignId == request.CampaignId,
                        selector: x => x.Voucher).Distinct().ToListAsync();

                List<int> voucherIds = vouchers.Select(x => x.Id).ToList();

                var voucherCodes = await UnitOfWork.Repository<VoucherCode>().GetAll(
                        predicate: x => x.CampaignMember.CampaignId == request.CampaignId && voucherIds.Contains(x.VoucherId)
                        ).ToListAsync();

                var campaignMembersA = await UnitOfWork.Repository<CampaignMember>().GetAll(
                      predicate: x => x.CampaignId == request.CampaignId
                          && x.Status != (int)CampaignMemberStatus.Cancelled
                          && x.Status != (int)CampaignMemberStatus.RefuseInvitated,
                      include: x => x.Include(y => y.Influencer).ThenInclude(z => z.VoucherInfluencers.Where(vi => voucherIds.Contains(vi.VoucherId)))).ToListAsync();

                var campaignMembers = await UnitOfWork.Repository<CampaignMember>().GetAll(
                          predicate: x => x.CampaignId == request.CampaignId
                              && x.Status != (int)CampaignMemberStatus.Cancelled
                              && x.Status != (int)CampaignMemberStatus.RefuseInvitated,
                          include: x => x.Include(y => y.Influencer).ThenInclude(z => z.VoucherInfluencers.Where(vi => voucherIds.Contains(vi.VoucherId))),
                          selector: x => new
                          {
                              Influencer = x.Influencer,
                              Status = x.Status,
                              QuantityVoucherGet = x.Influencer.VoucherInfluencers.Sum(y => y.QuantityGet),
                              QuantityVoucherUsed = x.Influencer.VoucherInfluencers.Sum(y => y.QuantityUsed)
                          })
                      .ToListAsync();

                int maxQuantityUsed = campaignMembers.Count == 0 ? 0 : campaignMembers.Max(x => x.QuantityVoucherUsed);

                report.CampaignMembers = campaignMembers.Select(x => new CampaignMemberReportViewModel
                {
                    Influencer = Mapper.Map<UserBasicViewModel>(x.Influencer),
                    Status = x.Status,
                    QuantityVoucherGet = x.QuantityVoucherGet,
                    QuantityVoucherUsed = x.QuantityVoucherUsed,
                    IsBestInfluencer = (x.QuantityVoucherUsed == maxQuantityUsed) && maxQuantityUsed > 0
                }).ToList();


                report.NumberOfInfluencer = influencers.Count;
                report.NumberOfInfluencerCompleted = influencers.Where(x => x.Status == (int)CampaignMemberStatus.Completed).Count();
                report.NumberOfVoucher = vouchers.Count;
                report.NumberOfVoucherCode = voucherCodes.Count;
                report.TotalNumberVoucherCodeUsed = voucherCodes.Sum(x => x.QuantityUsed);
                report.TotalNumberVoucherCodeGet = campaignMembers.Sum(x => x.QuantityVoucherGet);

                return new Response<CampaignReportViewModel>(report);

            }
        }
    }
}
