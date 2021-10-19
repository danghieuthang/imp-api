using System.Data;
using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Vouchers.Commands.CreateVoucher
{
    public class CreateVoucherCommandValidator : AbstractValidator<CreateVoucherCommand>
    {
        public CreateVoucherCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.CampaignId).MustAsync(async (campaign, id, cancellationToken) =>
              {
                  var camp = await unitOfWork.Repository<Campaign>().GetByIdAsync(id);
                  if (camp != null && camp.BrandId == campaign.BrandId) return true;
                  return false;
              }).WithMessage("Không hợp lệ (không tồn tại hoặc không có quyền).");

            RuleFor(x => x.VoucherName).MustRequired(256);
            RuleFor(x => x.Image).MustValidUrl(allowNull: true);
            RuleFor(x => x.Target).MustMaxLength(256);
            RuleFor(x => x.FromDate).MustValidDate();
            RuleFor(x => x.ToDate).Must((voucher, toDate) =>
            {
                if (!toDate.HasValue) return false;
                return toDate.Value.CompareTo(voucher.FromDate.Value) > 0;
            }).WithMessage("ToDate phải lớn hơn FromDate.");
            RuleFor(x => x.FromTime).MustValidTime();
            RuleFor(x => x.ToTime).MustValidTime();
            RuleFor(x => x.Quantity).MustPositiveInteger().WithMessage("Số lượng phải lớn hơn 0.");
        }
    }
}