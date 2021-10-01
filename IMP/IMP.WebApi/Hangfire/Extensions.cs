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
        private static ICampaignService ICampaignService;
        private static BackgroundJobs jobSchedule = new(ICampaignService);

        public static void AddHangfireExtension(this IServiceCollection services)
        {
            services.AddHangfire(config =>
            {
                config.UseMemoryStorage();
            });
            services.AddHangfireServer();
            GlobalConfiguration.Configuration.UseMemoryStorage();
            var manager = new RecurringJobManager();
            manager.AddOrUpdate("Update campaign status: Runs Every Day", Job.FromExpression(() => jobSchedule.CampaignJobsAsync()), "*/5 * * * *");

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
