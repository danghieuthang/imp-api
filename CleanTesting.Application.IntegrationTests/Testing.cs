using IMP.Infrastructure.Persistence.Contexts;
using IMP.WebApi;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTesting.Application.IntegrationTests
{
    [SetUpFixture]
    public class Testing
    {
        private static IConfiguration _configuration;
        private static IServiceScopeFactory _scopeFactory;

        [OneTimeSetUp]
        public void RunBeforeAnyTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" && w.ApplicationName == "IMP.WebApi"));

            var startup = new Startup(_configuration);

            startup.ConfigureServices(services);

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        }

        public static async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Add(entity);
            await context.SaveChangesAsync();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var meditor = scope.ServiceProvider.GetService<IMediator>();

            return await meditor.Send(request);
        }
    }
}
