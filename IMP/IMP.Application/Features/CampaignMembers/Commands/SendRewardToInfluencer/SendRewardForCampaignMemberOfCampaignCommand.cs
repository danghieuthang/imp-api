using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Models;
using IMP.Application.Features.CampaignMembers.Queries.GetRewards;

namespace IMP.Application.Features.CampaignMembers.Commands.SendRewardToInfluencer
{
    public class SendRewardForCampaignMemberOfCampaignCommand : ICommand<bool>
    {
        public int CampaignId { get; set; }
        public class SendRewardForCampaignMemberOfCampaignCommandHandler : CommandHandler<SendRewardForCampaignMemberOfCampaignCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IGenericRepository<Wallet> _walletRepository;
            private readonly IGenericRepository<CampaignMember> _campaignMemberRepository;
            private readonly IGenericRepository<Campaign> _campaignRepository;
            private readonly IGenericRepository<ApplicationUser> _applicatinUserRepository;
            private readonly IGenericRepository<Brand> _brandRepository;
            private readonly IGenericRepository<VoucherCode> _voucherCodeRepository;
            private readonly INotificationService _notificationService;
            private readonly ICampaignService _campaignService;

            public SendRewardForCampaignMemberOfCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICampaignService campaignService, INotificationService notificationService, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _campaignService = campaignService;
                _notificationService = notificationService;
                _authenticatedUserService = authenticatedUserService;

                _campaignMemberRepository = unitOfWork.Repository<CampaignMember>();
                _campaignRepository = unitOfWork.Repository<Campaign>();
                _walletRepository = unitOfWork.Repository<Wallet>();
                _applicatinUserRepository = unitOfWork.Repository<ApplicationUser>();
                _brandRepository = unitOfWork.Repository<Brand>();
                _voucherCodeRepository = unitOfWork.Repository<VoucherCode>();
            }

            public override async Task<Response<bool>> Handle(SendRewardForCampaignMemberOfCampaignCommand request, CancellationToken cancellationToken)
            {
                // Find campaign
                var campaign = await _campaignRepository.FindSingleAsync(
                        predicate: x => x.Id == request.CampaignId,
                        include: x => x.Include(y => y.CampaignMembers).Include(y => y.CampaignRewards)
                       );

                if (campaign.Status != (int)CampaignStatus.Announcing)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Chiến dịch trong thời gian thông báo mới có thể thanh toán"));
                }
                var campaignMembers = campaign.CampaignMembers.Where(x => x.Status == (int)CampaignMemberStatus.Completed);

                #region caculate money earning for member of campaign

                decimal defaultReward = campaign.CampaignRewards.Where(x => x.IsDefaultReward).Sum(x => x.Price);
                decimal bestInfluencerReward = campaign.CampaignRewards.Where(x => x.IsDefaultReward == false).Sum(x => x.Price);
                //Get best influencer total product amount
                var bestInfluencerTotalProductAmount = await _campaignService.BestCampaignMemberTotalProductAmount(request.CampaignId);

                var memberRewardEarnings = campaignMembers.Select(x => new MemberRewardEarningViewModel
                {
                    DefaultReward = defaultReward,
                    EarningMoney = x.Money,
                    CampaignMemberId = x.Id,
                }).ToList();

                var transactions = await UnitOfWork.Repository<VoucherCode>().GetAll(
                    predicate: x => x.CampaignMember.CampaignId == request.CampaignId)
                    .Select(x => new
                    {
                        x.CampaignMemberId,
                        TotalProductAmount = x.VoucherTransactions.Sum(y => y.TotalProductAmount)
                    }).Where(x => x.TotalProductAmount == bestInfluencerTotalProductAmount).ToListAsync();

                memberRewardEarnings.ForEach(x =>
                {
                    if (transactions.Any(y => y.CampaignMemberId == x.CampaignMemberId))
                    {
                        x.BestInfluencerReward = bestInfluencerReward;
                    };
                    x.SubTotal = x.DefaultReward + x.BestInfluencerReward + x.EarningMoney;
                    x.Tax = 10 * x.SubTotal / (decimal)100.0;
                    x.Total = x.SubTotal + x.Tax;
                });
                #endregion

                #region send reward
                var walletFrom = (await _brandRepository.FindSingleAsync(x => x.Id == _authenticatedUserService.BrandId, x => x.Wallet)).Wallet;
                if (walletFrom.Balance < memberRewardEarnings.Sum(x => x.Total)) // if wallet not enough balance for tranfers
                {
                    throw new ValidationException(new ValidationError("wallet.balance", "Số dư không đủ! Vui lòng nạp thêm tiền."));
                }

                foreach (var memberRewardEarning in memberRewardEarnings)
                {
                    decimal amountSend = memberRewardEarning.Total;
                    decimal amountReceive = memberRewardEarning.SubTotal;

                    var campaignMember = campaignMembers.Where(x => x.Id == memberRewardEarning.CampaignMemberId).FirstOrDefault();

                    var walletTo = await _walletRepository.FindSingleAsync(x => x.ApplicationUserId == campaignMember.InfluencerId);

                    var walletTransaction = new WalletTransaction
                    {
                        Amount = memberRewardEarning.Total,
                        SenderId = _authenticatedUserService.ApplicationUserId, // sender
                        ReceiverId = campaignMember.InfluencerId, // receiver
                        TransactionInfo = $"Thanh toán tiền thưởng thăm gia hoạt động",
                        TransactionType = (int)TransactionType.Transfer,
                        TransactionStatus = (int)WalletTransactionStatus.Successful,
                        PayDate = DateTime.UtcNow,
                        WalletToId = walletTo.Id,
                        WalletFromId = walletFrom.Id,
                        SenderBalance = walletFrom.Balance - amountSend,
                        ReceiverBalance = walletTo.Balance + amountReceive
                    };

                    // add transaction
                    UnitOfWork.Repository<WalletTransaction>().Add(walletTransaction);

                    // Recaculate balance of sender and receiver wallet
                    walletFrom.Balance -= amountSend;
                    walletTo.Balance += amountReceive;


                    // Update campaign member
                    campaignMember.IsPayReward = true;
                    _campaignMemberRepository.Update(campaignMember);
                }

                await UnitOfWork.CommitAsync();

                #endregion
                return new Response<bool>(true);
            }
        }
    }
}
