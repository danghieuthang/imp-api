﻿using IMP.Application.Enums;
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
                case NotificationType.BrandCreatedCampaign:
                    notification.Message = "Chiến dịch chờ duyệt.";
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
            await AddNotification(applicationUserid, redirectId, notificationType);
            int totalNotification = await _unitOfWork.Repository<Notification>().CountAsync();
            string data = JsonConvert.SerializeObject(new { TotalNotification = totalNotification });
            _ = Task.Run(() =>
            {
                _firebaseService.PushTotification(data, applicationUserid.ToString());
            });
        }
    }
}