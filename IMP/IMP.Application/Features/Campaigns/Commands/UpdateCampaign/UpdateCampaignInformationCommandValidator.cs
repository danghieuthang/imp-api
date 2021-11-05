using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.UpdateCampaign
{
    public class UpdateCampaignInformationCommandValidator : AbstractValidator<UpdateCampaignInformationCommand>
    {
        public UpdateCampaignInformationCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Title).MustMaxLength(256);
            RuleFor(x => x.Description).MustMaxLength(2000);
            RuleFor(x => x.AdditionalInformation).MustMaxLength(2000);

            //timeline
            RuleFor(x => x.Openning.Value).MustGreaterThanNow().When(x => x.Openning.HasValue);

            RuleFor(x => x.Applying.Value).Must((x, y) =>
            {
                return y.CompareTo(x.Openning) > 0;
            }).WithMessage("Ngày nộp đơn phải lớn hơn ngày bắt đầu chiến dịch.")
                .When(x => x.Applying.HasValue);

            RuleFor(x => x.Advertising.Value).Must((x, y) =>
            {
                return y.CompareTo(x.Applying) > 0;
            }).WithMessage("Ngày quảng cáo phải lớn hơn ngày nộp đơn.")
                .When(x => x.Advertising.HasValue);


            RuleFor(x => x.Evaluating.Value).Must((x, y) =>
            {
                return y.CompareTo(x.Advertising) > 0;
            }).WithMessage("Ngày đánh giá chiến dịch phải lơn hơn ngày quảng cáo.")
                .When(x => x.Evaluating.HasValue);

            RuleFor(x => x.Announcing.Value).Must((x, y) =>
            {
                return y.CompareTo(x.Evaluating) > 0;
            }).WithMessage("Ngày thông báo phải lớn hơn ngày đánh giá.")
                .When(x => x.Announcing.HasValue);

            RuleFor(x => x.Closed.Value).Must((x, y) =>
            {
                return y.CompareTo(x.Announcing) > 0;
            }).WithMessage("Ngày kết thúc chiến dịch phải lớn hơn ngày thông báo.")
                .When(x => x.Closed.HasValue);

            // Product/ service

            RuleFor(x => x.CampaignTypeId.Value).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<CampaignType>().IsExistAsync(x);
            }).When(x => x.CampaignTypeId.HasValue);

            RuleForEach(x => x.Products).ChildRules((products) =>
            {
                products.RuleFor(x => x.Name).MustMaxLength(255);
                products.RuleFor(x => x.Price).GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0.");
            });
            RuleFor(x => x.ProductInformation).MustMaxLength(2000);
            RuleFor(x => x.SampleContent).MustMaxLength(2000);

            //Reward
            RuleForEach(x => x.DefaultRewards).SetValidator(new CampaignRewardValidator());
            RuleForEach(x => x.BestInfluencerRewards).SetValidator(new CampaignRewardValidator());
        }
        public class CampaignRewardValidator : AbstractValidator<CampaignRewardRequest>
        {
            public CampaignRewardValidator()
            {
                RuleFor(x => x.Name).MustMaxLength(255);
                RuleFor(x => x.Price).GreaterThan(0).WithMessage("Giá phần thưởng phải lớn hơn 0.");
            }
        }
    }
}
