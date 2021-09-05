﻿using IMP.Application.Interfaces;
using IMP.Domain.Settings;
using IMP.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMP.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            #region Registers for configutation

            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.Configure<FirebaseSettings>(_config.GetSection("FirebaseSettings"));
            services.Configure<FileSettings>(_config.GetSection("FileSettings"));

            #endregion Registers for configutation

            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFirebaseService, FirebaseService>();
            services.AddTransient<IGoogleServices, GoogleServices>();
        }
    }
}
