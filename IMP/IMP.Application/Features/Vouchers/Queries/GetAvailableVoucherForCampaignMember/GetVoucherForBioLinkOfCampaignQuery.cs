using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
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

namespace IMP.Application.Features.Vouchers.Queries.GetAvailableVoucherForCampaignMember
{
    public class GetVoucherForBioLinkOfCampaignQuery : IQuery<IEnumerable<VoucherViewModel>>
    {
        [FromQuery(Name = "bio_link")]
        public string Biolink { get; set; }
        [FromQuery(Name = "campaign_id")]
        public int CampaignId { get; set; }

        public class GetVoucherForBioLinkOfCampaignQueryHandler : QueryHandler<GetVoucherForBioLinkOfCampaignQuery, IEnumerable<VoucherViewModel>>
        {

            public GetVoucherForBioLinkOfCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<VoucherViewModel>>> Handle(GetVoucherForBioLinkOfCampaignQuery request, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Pages.Any(y => y.BioLink.ToLower() == request.Biolink.ToLower()));

                if (user == null)
                {
                    throw new ValidationException(new ValidationError("bio_link", "Không tồn tại"));
                }

                var vouchers = await UnitOfWork.Repository<VoucherCode>().GetAll(
                        predicate: x => x.CampaignMember.InfluencerId == user.Id && x.CampaignMember.CampaignId == request.CampaignId,
                        selector: x => x.Voucher,
                        include: x => x.Include(y => y.Voucher)).Distinct().ToListAsync();

                var voucherViews = Mapper.Map<IEnumerable<VoucherViewModel>>(vouchers);
                return new Response<IEnumerable<VoucherViewModel>>(voucherViews);
            }
        }
    }
}
