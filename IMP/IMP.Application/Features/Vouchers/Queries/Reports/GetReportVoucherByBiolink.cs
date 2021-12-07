using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Vouchers.Queries.Reports
{
    public class GetReportVoucherByBiolinkQuery : IQuery<VoucherReportViewModel>
    {
        [FromQuery(Name = "bio_link")]
        public string Biolink { get; set; }
        [FromQuery(Name = "campaign_id")]
        public int CampaignId { get; set; }
        [FromQuery(Name = "voucher_id")]
        public int VoucherId { get; set; }

        public class GetReportByBiolinkQueryHandler : QueryHandler<GetReportVoucherByBiolinkQuery, VoucherReportViewModel>
        {
            public GetReportByBiolinkQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<VoucherReportViewModel>> Handle(GetReportVoucherByBiolinkQuery request, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Pages.Any(y => y.BioLink == request.Biolink));
                if (user == null)
                {
                    return new Response<VoucherReportViewModel>();
                }

                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.CampaignId == request.CampaignId && x.InfluencerId == user.Id, x => x.VoucherCodes);
                if (campaignMember == null)
                {
                    return new Response<VoucherReportViewModel>();
                }
                var voucher = await UnitOfWork.Repository<Voucher>().FindSingleAsync(x => x.Id == request.VoucherId, x => x.VoucherCodes);
                var view = Mapper.Map<VoucherReportViewModel>(voucher);
                view.Quantity = voucher.VoucherCodes.Sum(x => x.Quantity);
                view.QuantityGet = voucher.VoucherCodes.Sum(x => x.QuantityGet);
                view.QuantityUsed = voucher.VoucherCodes.Sum(x => x.QuantityUsed);
                view.VoucherCodes = null;
                return new Response<VoucherReportViewModel>(view);
            }
        }
    }
}
