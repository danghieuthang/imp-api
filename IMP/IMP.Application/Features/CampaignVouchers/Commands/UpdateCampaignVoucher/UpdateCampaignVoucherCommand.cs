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

namespace IMP.Application.Features.CampaignVouchers.Commands.UpdateCampaignVoucher
{
    public class UpdateCampaignVoucherCommand : ICommand<CampaignVoucherViewModel>
    {
        public int Id { get; set; }
        public int? QuantityForInfluencer { get; set; }
        public int? PercentForInfluencer { get; set; }
        public class UpdateCampaignVoucherCommandHandler : CommandHandler<UpdateCampaignVoucherCommand, CampaignVoucherViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public UpdateCampaignVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<CampaignVoucherViewModel>> Handle(UpdateCampaignVoucherCommand request, CancellationToken cancellationToken)
            {
                var campaignVoucher = await UnitOfWork.Repository<CampaignVoucher>().FindSingleAsync(x => x.Id == request.Id, x => x.Campaign);
                if (campaignVoucher == null)
                {
                    throw new ValidationException(new ValidationError("id", "Không tồn tại."));
                }

                if (campaignVoucher.Campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("id", "Không có quyền chỉnh sửa."));
                }
                campaignVoucher.QuantityForInfluencer = request.QuantityForInfluencer ?? campaignVoucher.QuantityForInfluencer;
                campaignVoucher.PercentForInfluencer = request.PercentForInfluencer ?? campaignVoucher.PercentForInfluencer;

                UnitOfWork.Repository<CampaignVoucher>().Update(campaignVoucher);
                await UnitOfWork.CommitAsync();

                var view = Mapper.Map<CampaignVoucherViewModel>(campaignVoucher);
                return new Response<CampaignVoucherViewModel>(view);
            }
        }
    }
}
