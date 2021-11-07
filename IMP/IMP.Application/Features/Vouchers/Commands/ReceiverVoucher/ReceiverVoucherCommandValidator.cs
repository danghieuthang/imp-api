//using FluentValidation;
//using IMP.Application.Enums;
//using IMP.Application.Interfaces;
//using IMP.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IMP.Application.Features.Vouchers.Commands.ReceiverVoucher
//{
//    public class ReceiverVoucherCommandValidator : AbstractValidator<ReceiverVoucherCommand>
//    {
//        public ReceiverVoucherCommandValidator(IUnitOfWork unitOfWork)
//        {
//            RuleFor(x => x.CampaignId).MustAsync(async (id, cancellationToken) =>
//              {
//                  var campaign = await unitOfWork.Repository<Campaign>().GetByIdAsync(id);
//                  if (campaign == null) return false;
//                  if (campaign.Status == (int)CampaignStatus.Advertising || campaign.Status == (int)CampaignStatus.Announcing)
//                  {
//                      return true;
//                  }
//                  return false;
//              }).WithMessage("Không thể lấy code chiến dịch.");
//        }
//    }
//}
