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
            int totalNotification = await _unitOfWork.Repository<Notification>().CountAsync(x => x.ApplicationUserId == applicationUserid && x.IsRead == false);
            string data = JsonConvert.SerializeObject(new
            {
                TotalNotification = totalNotification,
                LastNotificationId = notification.Id
            });
            _ = Task.Run(() =>
            {
                _firebaseService.PushTotification(data, applicationUserid.ToString());
            });
        }
    }
}