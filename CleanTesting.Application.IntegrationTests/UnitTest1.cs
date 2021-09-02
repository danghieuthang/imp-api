using FluentAssertions;
using IMP.Application.DTOs.ViewModels;
using IMP.Application.Features.Platforms.Queries.GetAllPlatforms;
using IMP.Application.Wrappers;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CleanTesting.Application.IntegrationTests
{
    using static Testing;
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldReturnAllListPlatforms()
        {
            var query = new GetAllPlatformsQuery();

            var result = await SendAsync<PagedResponse<IEnumerable<PlatformViewModel>>>(query);

            result.Data.Should().HaveCountGreaterThan(0);
        }
    }
}