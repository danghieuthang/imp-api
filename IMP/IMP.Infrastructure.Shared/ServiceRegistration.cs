using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Shared;
using IMP.Domain.Settings;
using IMP.Infrastructure.Shared.Caches;
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
            services.Configure<FacebookSettings>(_config.GetSection("Authentication").GetSection("Facebook"));
            services.Configure<InstagramSettings>(_config.GetSection("Authentication").GetSection("Instagram"));
            services.Configure<OtpSettings>(_config.GetSection("OtpSettings"));
            services.Configure<VnPaySettings>(_config.GetSection("VnpaySettings"));
            services.Configure<ZaloSettings>(_config.GetSection("ZaloSettings"));

            #endregion Registers for configutation

            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFirebaseService, FirebaseService>();
            services.AddTransient<IGoogleService, GoogleService>();
            services.AddTransient<IFacebookService, FacebookService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ITiktokService, TiktokService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IZaloService, ZaloService>();

            #region Register Caching service
            services.Configure<CacheSettings>(_config.GetSection("CacheSettings"));
            services.AddMemoryCache();
            services.AddTransient<ICacheService, MemoryCacheService>();
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = "localhost:4455";
            //});
            #endregion
        }
    }
}
