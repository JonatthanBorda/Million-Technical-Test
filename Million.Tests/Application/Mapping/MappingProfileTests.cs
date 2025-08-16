using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Million.Application.Mapping;

namespace Million.Tests.Application.Mapping
{
    public class MappingProfileTests
    {
        [Test]
        public void AutoMapper_profile_should_be_valid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.Invoking(c => c.AssertConfigurationIsValid()).Should().NotThrow();
        }
    }
}
