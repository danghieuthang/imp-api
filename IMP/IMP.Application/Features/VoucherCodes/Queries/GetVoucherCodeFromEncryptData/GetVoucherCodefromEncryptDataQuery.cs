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

namespace IMP.Application.Features.VoucherCodes.Queries.GetVoucherCodeFromEncryptData
{
    public class GetVoucherCodeFromEncryptDataQuery : IQuery<CheckVoucherCodeViewModel>
    {
        [FromQuery(Name = "data")]
        public string Data { get; set; }
        public class GetVoucherCodeFromEncryptDataQueryHandler : QueryHandler<GetVoucherCodeFromEncryptDataQuery, CheckVoucherCodeViewModel>
        {
            public GetVoucherCodeFromEncryptDataQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<CheckVoucherCodeViewModel>> Handle(GetVoucherCodeFromEncryptDataQuery request, CancellationToken cancellationToken)
            {

                string decryptData = Utils.Utils.Decrypt(request.Data);
                string[] data = decryptData.Split(";");
                if (data.Length < 4)
                {
                    return new Response<CheckVoucherCodeViewModel>(message: "Data không đúng");
                }

                string code = data[0];
                int cammpaignId = 0;
                int influencerId = 0;
                int voucherId = 0;

                int.TryParse(data[1], out cammpaignId);
                int.TryParse(data[2], out influencerId);
                int.TryParse(data[3], out voucherId);

                var voucherCode = await UnitOfWork.Repository<VoucherCode>().FindSingleAsync(x => x.Code.ToLower() == code.ToLower() && x.VoucherId == voucherId, x => x.Voucher);

                if (voucherCode != null)
                {
                    var view = new CheckVoucherCodeViewModel
                    {
                        Code = voucherCode.Code,
                        Expired = voucherCode.Quantity == 1 ? voucherCode.Expired : voucherCode.Voucher.FromDate,
                        Quantity = voucherCode.Quantity,
                        QuantityUsed = voucherCode.QuantityUsed,
                        InfluencerId = influencerId,
                        CampaignId = cammpaignId,
                    };
                    return new Response<CheckVoucherCodeViewModel>(view);
                }
                return new Response<CheckVoucherCodeViewModel>(message: "Data không đúng");
            }
        }
    }
}
