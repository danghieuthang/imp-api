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
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0.");

            RuleFor(x => x.CampaignId).MustExistEntityId(async (campaignId, cancellationToken) =>
            {
                return await unitOfWork.Repository<Campaign>().IsExistAsync(campaignId);
            });
            
            RuleFor(x => x.VoucherId).MustExistEntityId(async (voucherId, cancellationToken) =>
            {
                return await unitOfWork.Repository<Voucher>().IsExistAsync(voucherId);
            });
        }
    }
}