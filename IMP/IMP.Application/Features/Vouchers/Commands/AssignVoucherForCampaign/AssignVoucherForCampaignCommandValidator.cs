using System.Data;
using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Vouchers.Commands.AssignVoucherForCampaign
{
    public class AssignVoucherToCampaignCommandValidator : AbstractValidator<AssignVoucherToCampaignCommand>
    {
        public AssignVoucherToCampaignCommandValidator(IUnitOfWork unitOfWork)
        {

            RuleFor(x => x.CampaignId).MustExistEntityId(async (campaignId, cancellationToken) =>
            {
                return await unitOfWork.Repository<Campaign>().IsExistAsync(campaignId);
            });

            RuleFor(x => x.VoucherId).MustExistEntityId(async (voucherId, cancellationToken) =>
            {
                return await unitOfWork.Repository<Voucher>().IsExistAsync(voucherId);
            });

            RuleFor(x => x.VoucherId).MustAsync(async (voucher, id, c) =>
            {
                bool isExist = await unitOfWork.Repository<CampaignVoucher>().IsExistAsync(x => x.CampaignId == voucher.CampaignId && x.VoucherId == voucher.VoucherId);
                return !isExist;
            }).WithMessage("Voucher đã được assign cho campaign.");
        }
    }
}
