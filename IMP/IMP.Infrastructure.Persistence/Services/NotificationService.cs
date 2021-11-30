using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Domain.Entities;
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

        private async Task<Notification> AddNotification(int applicationUserId, int redirectId, NotificationType notificationType)
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
                    notification.Message = "Bạn đã được nhãn hàng đồng ý thăm gia chiến dịch.";
                    break;
                case NotificationType.BrandCancelJoinCampaign:
                    notification.Message = "Bạn đã bị nhãn hàng từ chối thăm gia chiến dịch.";
                    break;
                case NotificationType.InfluencerCommentMemberActivity:
                    notification.Message = "Có bình luật mới từ thành viên chiến dịch.";

                    var memberAcitity = await _unitOfWork.Repository<MemberActivity>().FindSingleAsync(x => x.Id == redirectId, x => x.CampaignActivity, x => x.CampaignActivity.Campaign);

                    notification.Url = $"/campaign/{memberAcitity.CampaignActivity.CampaignId}/member/{memberAcitity.CampaignMemberId}";
                    notification.ApplicationUserId = memberAcitity.CampaignActivity.Campaign.CreatedById;
                    break;
                case NotificationType.BrandCommentMemberActivity:
                    notification.Message = "Nhãn hàng vừa bình luật.";

                    memberAcitity = await _unitOfWork.Repository<MemberActivity>().FindSingleAsync(x => x.Id == redirectId, x => x.CampaignMember);

                    notification.Url = $"/campaign/{memberAcitity.CampaignMember.CampaignId}/member/{memberAcitity.CampaignMemberId}";
                    notification.ApplicationUserId = memberAcitity.CampaignMember.InfluencerId;
                    break;
                default:
                    notification.Message = "";
                    break;
            }

            _unitOfWork.Repository<Notification>().Add(notification);
            await _unitOfWork.CommitAsync();
            return notification;
        }

        public async Task PutNotication(int applicationUserid, int redirectId, NotificationType notificationType)
        {
            var notification = await AddNotification(applicationUserid, redirectId, notificationType);
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