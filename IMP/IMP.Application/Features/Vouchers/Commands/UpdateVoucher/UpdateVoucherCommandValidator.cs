
using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Vouchers.Commands.UpdateVoucher
{
    public class UpdateVoucherCommandValidator : AbstractValidator<UpdateVoucherCommand>
    {
        public UpdateVoucherCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Id).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<Voucher>().IsExistAsync(x);
            });

            RuleFor(x => x.VoucherName).MustRequired(256);
            RuleFor(x => x.Image).MustValidUrl(allowNull: true);
            RuleFor(x => x.Target).MustMaxLength(256);
            //RuleFor(x => x.FromDate).MustValidDate();
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