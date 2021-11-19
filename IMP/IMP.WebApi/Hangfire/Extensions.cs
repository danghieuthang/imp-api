using Hangfire;
using Hangfire.Common;
using Hangfire.MemoryStorage;
using HangfireBasicAuthenticationFilter;
using IMP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMP.WebApi.Hangfire
{
    public static class Extensions
    {
        private static readonly ICampaignService ICampaignService;
        private static readonly BackgroundJobs jobSchedule = new(ICampaignService);

        public static void AddHangfireExtension(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddHangfire(config =>
                {
                    config.UseMemoryStorage();
                });
            }
            else
            {
                services.AddHangfire(config =>
                {
                    config.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"));
                });
                GlobalConfiguration.Configuration.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"));
            }
            services.AddHangfireServer();

            var manager = new RecurringJobManager();
            manager.AddOrUpdate("Update campaign status: Runs Every Day", Job.FromExpression(() => jobSchedule.CampaignJobsAsync()), Cron.Hourly());

        }
        public static void UseHangfireDashboardExtension(this IApplicationBuilder app, IConfiguration configuration)
        {

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                AppPath = null,
                DashboardTitle = "Hangfire Dashboard",
                Authorization = new[]{ new HangfireCustomBasicAuthenticationFilter
                {
                    User = configuration.GetSection("HangfireCredentials:UserName").Value,
                    Pass = configuration.GetSection("HangfireCredentials:Password").Value,
                }
                },
            });
        }
    }
}
