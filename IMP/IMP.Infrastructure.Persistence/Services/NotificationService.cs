using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IUnitOfWork _unitOfWork;
        public NotificationService(IFirebaseService firebaseService, IUnitOfWork unitOfWork)
        {
            _firebaseService = firebaseService;
            _unitOfWork = unitOfWork;
        }

        private async Task<Notification> AddNotification(int applicationUserId, int redirectId, NotificationType notificationType, string message = null)
        {
            Notification notification = new Notification
            {
                ApplicationUserId = applicationUserId,
                RedirectId = redirectId,
                Type = (int)notificationType,

            };
            switch (notificationType)
            {
                case NotificationType.InfluencerJoinCampaign:
                    notification.Message = "Có thành viên mới thăm gia chiến dịch.";
                    break;
                case NotificationType.BrandCreatedCampaign:
                    notification.Message = "Chiến dịch chờ duyệt.";
                    break;
                case NotificationType.AdminApprovedCampaign:
                    notification.Message = "Chiến dịch đã được duyệt.";
                    break;
                case NotificationType.AdminRejectCampaign:
                    notification.Message = "Chiến dịch bị từ chối.";
                    break;
                case NotificationType.AdminPendingCampaign:
                    notification.Message = "Chiến dịch đang được xem xét lại.";
                    break;
                case NotificationType.BrandApprovedJoinCampaign:
                    notification.Message = "Bạn đã được nhãn hàng đồng ý tham gia chiến dịch.";
                    break;
                case NotificationType.BrandCancelJoinCampaign:
                    notification.Message = "Bạn đã bị nhãn hàng từ chối tham gia chiến dịch.";
                    break;
                case NotificationType.InfluencerCommentMemberActivity:
                    notification.Message = "Có bình luật mới từ thành viên chiến dịch.";

                    var memberAcitity = await _unitOfWork.Repository<MemberActivity>().FindSingleAsync(x => x.Id == redirectId, x => x.CampaignActivity, x => x.CampaignActivity.Campaign);

                    notification.Url = $"/campaign/{memberAcitity.CampaignActivity.CampaignId}/member/{memberAcitity.CampaignMemberId}";
                    notification.ApplicationUserId = memberAcitity.CampaignActivity.Campaign.CreatedById;
                    break;
                case NotificationType.BrandCommentMemberActivity:

                    memberAcitity = await _unitOfWork.Repository<MemberActivity>().FindSingleAsync(x => x.Id == redirectId,
                            include: x => x.Include(y => y.CampaignMember).ThenInclude(y => y.Campaign).ThenInclude(y => y.Brand));

                    notification.Message = $"Nhãn hàng - {memberAcitity.CampaignMember.Campaign.Brand.CompanyName} vừa bình luận.";

                    notification.Url = $"/campaign/{memberAcitity.CampaignMember.CampaignId}/member/{memberAcitity.CampaignMemberId}";
                    notification.ApplicationUserId = memberAcitity.CampaignMember.InfluencerId;
                    break;

                case NotificationType.BrandApprovedMemberActivity:
                    memberAcitity = await _unitOfWork.Repository<MemberActivity>().FindSingleAsync(x => x.Id == redirectId,
                           include: x => x.Include(y => y.CampaignMember).ThenInclude(y => y.Campaign).ThenInclude(y => y.Brand));

                    notification.Message = $"Nhãn hàng - {memberAcitity.CampaignMember.Campaign.Brand.CompanyName} vừa đánh giá bạn hoàn thành hoạt động.";

                    notification.Url = $"/campaign/{memberAcitity.CampaignMember.CampaignId}/member/{memberAcitity.CampaignMemberId}";
                    break;

                case NotificationType.BrandRejectMemberActivity:
                    memberAcitity = await _unitOfWork.Repository<MemberActivity>().FindSingleAsync(x => x.Id == redirectId,
                        include: x => x.Include(y => y.CampaignMember).ThenInclude(y => y.Campaign).ThenInclude(y => y.Brand));

                    notification.Message = $"Nhãn hàng - {memberAcitity.CampaignMember.Campaign.Brand.CompanyName} vừa đánh giá bạn chưa hoàn thành hoạt động.";

                    notification.Url = $"/campaign/{memberAcitity.CampaignMember.CampaignId}/member/{memberAcitity.CampaignMemberId}";
                    break;
                case NotificationType.BrandInvitedToCampaign:
                    notification.Message = message ?? "Nhãn hàng vừa mời bạn tham gia chiến dịch.";
                    break;
                case NotificationType.InfluencerAcceptedInvited:
                    notification.Message = message ?? "Influencer đồng ý tham gia chiến dịch.";
                    break;
                case NotificationType.InfluencerSubmitMemberActivity:
                    notification.Message = message ?? "Influencer vừa cập chiến hoạt động.";
                    break;
                default:
                    notification.Message = "";
                    break;
            }

            _unitOfWork.Repository<Notification>().Add(notification);
            await _unitOfWork.CommitAsync();
            return notification;
        }

        public async Task PutNotication(int applicationUserid, int redirectId, NotificationType notificationType, string message = null)
        {
            var notification = await AddNotification(applicationUserid, redirectId, notificationType, message);
            int totalNotification = await _unitOfWork.Repository<Notification>().CountAsync(x => x.ApplicationUserId == notification.ApplicationUserId && x.IsRead == false);
            string data = JsonConvert.SerializeObject(new
            {
                TotalNotification = totalNotification,
                LastNotificationId = notification.Id
            });
            _ = Task.Run(() =>
            {
                _firebaseService.PushTotification(data, notification.ApplicationUserId.ToString());
            });
        }
        public async Task PutNotication(Notification notification)
        {
            _unitOfWork.Repository<Notification>().Add(notification);
            await _unitOfWork.CommitAsync();
            int totalNotification = await _unitOfWork.Repository<Notification>().CountAsync(x => x.ApplicationUserId == notification.ApplicationUserId && x.IsRead == false);
            string data = JsonConvert.SerializeObject(new
            {
                TotalNotification = totalNotification,
                LastNotificationId = notification.Id
            });
            _ = Task.Run(() =>
            {
                _firebaseService.PushTotification(data, notification.ApplicationUserId.ToString());
            });
        }

    }
}