using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Validations;
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

namespace IMP.Application.Features.VoucherTransactions.Queries.GetAllVoucherTransactionOfCampaign
{
    public class GetAllVoucherTransactionOfCampaignQuery : PageRequest, IListQuery<VoucherTransactionViewModel>
    {
        [FromQuery(Name = "campaign_id")]
        public int CampaignId { get; set; }
        [FromQuery(Name = "name")]
        public string Search { get; set; }
        public class GetAllVoucherTransactionOfCampaignQueryValidator : PageRequestValidator<GetAllVoucherTransactionOfCampaignQuery, VoucherTransactionViewModel>
        {

        }
        public class GetAllVoucherTransactionOfCampaignQueryHandler : ListQueryHandler<GetAllVoucherTransactionOfCampaignQuery, VoucherTransactionViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetAllVoucherTransactionOfCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<IPagedList<VoucherTransactionViewModel>>> Handle(GetAllVoucherTransactionOfCampaignQuery request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().FindSingleAsync(x => x.Id == request.CampaignId, x => x.Vouchers);
                if (campaign == null)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Chiến dịch không tồn tại."));
                }
                if (campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Không có quyền."));
                }
                string name = request.Search?.ToLower() ?? "";

                var voucherIds = campaign.Vouchers.Select(x => x.VoucherId).ToList();
                var page = await UnitOfWork.Repository<VoucherTransaction>().GetPagedList(
                        predicate: x => voucherIds.Contains(x.VoucherCode.VoucherId)
                            && (x.Order.Contains(name) || x.VoucherCode.Code.ToLower().Contains(name)),
                        orderBy: request.OrderField,
                        orderByDecensing: request.OrderBy == Enums.OrderBy.DESC,
                        pageIndex: request.PageIndex,
                        pageSize: request.PageSize,
                        cancellationToken: cancellationToken,
                        include: x => x.Include(y => y.VoucherCode).ThenInclude(z => z.Voucher)

                        );

                var pageView = page.ToResponsePagedList<VoucherTransactionViewModel>(Mapper);
                return new Response<IPagedList<VoucherTransactionViewModel>>(data: pageView);
            }
        }
    }
}
