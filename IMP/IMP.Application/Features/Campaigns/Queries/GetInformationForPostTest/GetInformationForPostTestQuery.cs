using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace IMP.Application.Features.Campaigns.Queries.GetInformationForPostTest
{
    public class GetInformationForPostTestQuery : IQuery<InformationPostTestViewModel>
    {
        [FromQuery(Name = "campaign_id")]
        public int CampaignId { get; set; }
        public class GetInformationForPostTestQueryHandler : QueryHandler<GetInformationForPostTestQuery, InformationPostTestViewModel>
        {
            public GetInformationForPostTestQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<InformationPostTestViewModel>> Handle(GetInformationForPostTestQuery request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().FindSingleAsync(x => x.Id == request.CampaignId, x => x.Products);
                if (campaign != null)
                {
                    var vouchers = await UnitOfWork.Repository<CampaignVoucher>().GetAll(
                        predicate: x => x.CampaignId == request.CampaignId,
                        include: x => x.Include(y => y.Voucher).ThenInclude(voucherCodes => voucherCodes.VoucherCodes),
                        selector: x => x.Voucher)
                        .ToListAsync();

                    InformationPostTestViewModel rs = new InformationPostTestViewModel
                    {
                        Products = campaign.Products.ToList(),
                        Vouchers = vouchers.Select(x => new VoucherTestInformation
                        {
                            DiscountValue = x.DiscountValue,
                            DiscountValueType = x.DiscountValueType,
                            Codes = x.VoucherCodes.Select(x => x.Code).ToList()
                        }).ToList()
                    };
                    return new Response<InformationPostTestViewModel>(rs);
                }
                return null;

            }


        }
    }


    public class InformationPostTestViewModel
    {
        public List<Product> Products { get; set; }
        public List<VoucherTestInformation> Vouchers { get; set; }
    }

    public class VoucherTestInformation
    {
        public decimal DiscountValue { get; set; }
        public int DiscountValueType { get; set; }
        public List<string> Codes { get; set; }
    }
}
