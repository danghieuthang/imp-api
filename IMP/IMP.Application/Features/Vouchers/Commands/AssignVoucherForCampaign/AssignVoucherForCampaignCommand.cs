using AutoMapper;
using IMP.Application.Interfaces;
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
    public class AssignVoucherToCampaignCommand: ICommand<CampaignVoucherViewModel>
    {
        public int CampaignId { get; set; }
        public int VoucherId { get; set; }
        public int Quantity { get; set; }
        public class AssignVoucherForCampaignCommandHandler : CommandHandler<AssignVoucherToCampaignCommand, CampaignVoucherViewModel>
        {
            public AssignVoucherForCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<CampaignVoucherViewModel>> Handle(AssignVoucherToCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaignVoucher = Mapper.Map<CampaignVoucher>(request);

                await UnitOfWork.Repository<CampaignVoucher>().AddAsync(campaignVoucher);
                await UnitOfWork.CommitAsync();

                var campaignVoucherView = Mapper.Map<CampaignVoucherViewModel>(campaignVoucher);
                return new Response<CampaignVoucherViewModel>(campaignVoucherView);
            }
        }
    }
}
