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
            RuleFor(x => x.Openning).MustGreaterThanNow();

            RuleFor(x => x.Applying).Must((x, y) =>
            {
                return y.CompareTo(x.Openning) > 0;
            }).WithMessage("Ngày nộp đơn phải lớn hơn ngày bắt đầu chiến dịch.");

            RuleFor(x => x.Advertising).Must((x, y) =>
            {
                return y.CompareTo(x.Applying) > 0;
            }).WithMessage("Ngày quảng cáo phải lớn hơn ngày nộp đơn.");


            RuleFor(x => x.Evaluating).Must((x, y) =>
            {
                return y.CompareTo(x.Advertising) > 0;
            }).WithMessage("Ngày đánh giá chiến dịch phải lơn hơn ngày quảng cáo.");

            RuleFor(x => x.Announcing).Must((x, y) =>
            {
                return y.CompareTo(x.Evaluating) > 0;
            }).WithMessage("Ngày thông báo phải lớn hơn ngày đánh giá.");

            RuleFor(x => x.Closed).Must((x, y) =>
            {
                return y.CompareTo(x.Announcing) > 0;
            }).WithMessage("Ngày kết thúc chiến dịch phải lớn hơn ngày thông báo.");

            // Product/ service

            RuleFor(x => x.CampaignTypeId).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<CampaignType>().IsExistAsync(x);
            });

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
