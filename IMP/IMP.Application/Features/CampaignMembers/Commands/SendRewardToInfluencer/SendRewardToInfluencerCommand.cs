using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignMembers.Commands.SendRewardToInfluencer
{
    public class SendRewardToInfluencerCommand : ICommand<bool>
    {
        public int CampaignMemberId { get; set; }
        public class SendRewardToInfluencerCommandHandler : CommandHandler<SendRewardToInfluencerCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IGenericRepository<Wallet> _walletRepository;
            private readonly IGenericRepository<CampaignMember> _campaignMemberRepository;
            private readonly IGenericRepository<ApplicationUser> _applicatinUserRepository;
            private readonly IGenericRepository<Brand> _brandRepository;
            private readonly IGenericRepository<VoucherCode> _voucherCodeRepository;
            private readonly INotificationService _notificationService;

            public SendRewardToInfluencerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService, INotificationService notificationService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
                _campaignMemberRepository = unitOfWork.Repository<CampaignMember>();
                _walletRepository = unitOfWork.Repository<Wallet>();
                _applicatinUserRepository = unitOfWork.Repository<ApplicationUser>();
                _brandRepository = unitOfWork.Repository<Brand>();
                _voucherCodeRepository = unitOfWork.Repository<VoucherCode>();
                _notificationService = notificationService;
            }

            public override async Task<Response<bool>> Handle(SendRewardToInfluencerCommand request, CancellationToken cancellationToken)
            {
                var campaignMember = await _campaignMemberRepository.FindSingleAsync(x => x.Id == request.CampaignMemberId,
                       include: x => x.Include(y => y.Campaign).ThenInclude(z => z.CampaignRewards)
                            .Include(y => y.Campaign)
                                .ThenInclude(z => z.Vouchers.Where(cv => cv.IsDefaultReward == false && cv.IsBestInfluencerReward == false))
                            .Include(y => y.VoucherCodes));

                if (campaignMember == null)
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Không tồn tại."));

                }

                if (campaignMember.Campaign.BrandId != _authenticatedUserService.BrandId) // check authorize
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Không có quyền thanh toán."));

                }

                if (campaignMember.IsPayReward)
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Đã thanh toán cho thành viên này."));
                }
                if (!campaignMember.ActivityProgess) // check this member was completed all acvitity
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Các hoạt động của thành viên chưa được hoàn thành hết."));
                }

                // get wallet of brand
                var walletFrom = (await _brandRepository.FindSingleAsync(x => x.Id == _authenticatedUserService.BrandId, x => x.Wallet)).Wallet;

                var walletTo = await _walletRepository.FindSingleAsync(x => x.ApplicationUserId == campaignMember.InfluencerId);

                // Get default earning from campaign
                decimal amount = campaignMember.Campaign.CampaignRewards.Where(x => x.IsDefaultReward).Sum(x => x.Price);

                // Caculate earning from voucher code
                amount += campaignMember.Money;

                // Check the balance of the brand
                if (walletFrom.Balance < amount)
                {
                    throw new ValidationException(new ValidationError("wallet.balance", "Số dư không đủ! Vui lòng nạp thêm tiền."));
                }

                var walletTransaction = new WalletTransaction
                {
                    Amount = amount,
                    SenderId = _authenticatedUserService.ApplicationUserId, // sender
                    ReceiverId = campaignMember.InfluencerId, // receiver
                    TransactionInfo = $"Thanh toán tiền thưởng thăm gia hoạt động",
                    TransactionType = (int)TransactionType.Transfer,
                    TransactionStatus = (int)WalletTransactionStatus.Successful,
                    PayDate = DateTime.UtcNow,
                    WalletToId = walletTo.Id,
                    WalletFromId = walletFrom.Id,
                    SenderBalance = walletFrom.Balance - amount,
                    ReceiverBalance = walletTo.Balance + amount
                };

                // add transaction
                UnitOfWork.Repository<WalletTransaction>().Add(walletTransaction);

                // Recaculate balance of sender and receiver wallet
                walletFrom.Balance -= amount;
                walletTo.Balance += amount;


                // Update campaign member
                campaignMember.IsPayReward = true;
                _campaignMemberRepository.Update(campaignMember);

                // commit
                await UnitOfWork.CommitAsync();

                await _notificationService.PutNotication(applicationUserid: campaignMember.InfluencerId, redirectId: walletTransaction.Id, NotificationType.ReceivedMoney);
                return new Response<bool>(data: true);
            }

            private void ValidationBeforeCreateTransaction()
            {

            }
        }
    }
}
