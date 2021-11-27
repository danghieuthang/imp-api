using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignVouchers.Commands.UpdateCampaignVoucher
{
    public class UpdateCampaignVoucherCommandValidator : AbstractValidator<UpdateCampaignVoucherCommand>
    {
        public UpdateCampaignVoucherCommandValidator()
        {
            RuleFor(x => x.PercentForInfluencer).GreaterThan(0).WithMessage("Tỉ lệ phải lớn hơn 0.").LessThan(100).WithMessage("Tỉ lệ phải nhỏ hơn 100.").When(x => x.PercentForInfluencer.HasValue);
            RuleFor(x => x.QuantityForInfluencer).GreaterThan(0).WithMessage("Số lượng phải lớn hơn không.").When(x => x.QuantityForInfluencer.HasValue);

        }
    }
}
